using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.TraversalModel.Trades;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Http;
using CourseStudio.Lib.Exceptions.Trades;

namespace CourseStudio.Api.Services.Trades
{
	public class SalesOrderServices : BaseService, ISalesOrderServices
    {
		private readonly ISalesOrderRepository _salesOrderRepository;
		private readonly ICouponRepository _couponRepository;
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly PaymentProcessConfig _paymentProcessConfigs;
        
        public SalesOrderServices(
            ISalesOrderRepository salesOrderRepository,
			ICouponRepository couponRepository,
			IShoppingCartRepository shoppingCartRepository,
			IOptions<PaymentProcessConfig> paymentProcessConfigs,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
            _salesOrderRepository = salesOrderRepository;
			_couponRepository = couponRepository;
			_shoppingCartRepository = shoppingCartRepository;
			_paymentProcessConfigs = paymentProcessConfigs.Value;
        }
        
		public async Task<PaginationDto<SalesOrderDto>> GetOrdersByCurrentUserAsync(int pageNumber, int pageSize) 
		{
			var user = await GetCurrentUser();
			return Mapper.Map<PaginationDto<SalesOrderDto>>(await _salesOrderRepository.GetPagedOrdersByUserIdAsync(user.Id, pageNumber, pageSize));
		}

		public async Task<SalesOrderDto> GetOrderByOrderNumberAsync(string orderNumber)
		{
			// get userorder
			var order = await _salesOrderRepository.GetOrderByNumberAsync(orderNumber);

			// check permission
			var user = await GetCurrentUser();
			if (order.UserId != user.Id)
            {
                throw new ForbiddenException();
            }

			return Mapper.Map<SalesOrderDto>(order);
		}

		public async Task<SalesOrderDto> PlaceOrderAsync(string orderNumber)
		{
			// 1: get user and order
			var user = await GetCurrentUser();
			if (user.PaymentProfile == null) 
			{
				throw new NotFoundException("User payment profile not found.");
			} 
			var order = await _salesOrderRepository.GetOrderByNumberAsync(orderNumber);
			if (order == null)
			{
				throw new NotFoundException("Order not found.");
			}
            
			// 3. Place order
			order.PlaceOrder();
			await _salesOrderRepository.SaveAsync();

			// 4. send payment request
			var url = string.Format(_paymentProcessConfigs.PostPaymentUrl, _paymentProcessConfigs.RootUrl);
			var headers = new List<(string, string)> { PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.PaymentPasscode) };
			var body = PaymentHelper.PostPaymentRequestBody(
				order.OrderNumber, 
				order.AmountTotal, 
				user.PaymentProfile.CustomerCode, 
				user.PaymentProfile.PaymentToken
			);

			var paymentResponse = await HttpRequestHelper.PostAsync(url, headers, body);
			if(paymentResponse.StatusCode != HttpStatusCode.OK) 
			{
				throw new OrderActiveException(await paymentResponse.Content.ReadAsStringAsync()); 
			}

			var tansctionMeta = await paymentResponse.Content.ReadAsStringAsync();
			PaymentProcessingResponseDto tansctionDto = JsonConvert.DeserializeObject<PaymentProcessingResponseDto>(tansctionMeta);

			// 4. Active Order
			ActiveOrder(tansctionDto, tansctionMeta, order);
			await _salesOrderRepository.SaveAsync();

            // 4. async send bambora request
			return Mapper.Map<SalesOrderDto>(order);
        }


		public async Task<SalesOrderDto> PlaceOrderWithCreditCardAsync(string orderNumber, CreditCardDto creditCardDto) 
		{
			// 1: get user and order
            var user = await GetCurrentUser();
            var order = await _salesOrderRepository.GetOrderByNumberAsync(orderNumber);
            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            // 3. Place order
            order.PlaceOrder();
            await _salesOrderRepository.SaveAsync();

			// 4. send payment request         
            var url = string.Format(_paymentProcessConfigs.PostPaymentUrl, _paymentProcessConfigs.RootUrl);
            var headers = new List<(string, string)>{ PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.PaymentPasscode)};
			var body = PaymentHelper.PostPaymentWithCardRequestBody(
				order.OrderNumber, 
				order.AmountTotal, 
				creditCardDto.Name, 
				creditCardDto.Number, 
				creditCardDto.Expiry_month, 
				creditCardDto.Expiry_year, 
				creditCardDto.Cvd
			);

            var paymentResponse = await HttpRequestHelper.PostAsync(url, headers, body);
            if (paymentResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new OrderActiveException(await paymentResponse.Content.ReadAsStringAsync());
            }

            var tansctionMeta = await paymentResponse.Content.ReadAsStringAsync();
            PaymentProcessingResponseDto tansctionDto = JsonConvert.DeserializeObject<PaymentProcessingResponseDto>(tansctionMeta);

			// 4. Active Order
			ActiveOrder(tansctionDto, tansctionMeta, order);
            await _salesOrderRepository.SaveAsync();

            // 4. async send bambora request
            return Mapper.Map<SalesOrderDto>(order);
		}

		private void ActiveOrder(PaymentProcessingResponseDto tansctionDto, string tansctionMeta, Order order) 
		{
			// 4. Active Order
            if (!Enum.TryParse(tansctionDto.Type, out TransactionTypeEnum transactionType))
            {
                transactionType = TransactionTypeEnum.Unknown;
                // TODO: should throw exception?
                //throw new OrderValidationException("Transaction Type invalide.");
            }
            if (!DateTime.TryParse(tansctionDto.Created, out DateTime transactionCreateTime))
            {
                transactionCreateTime = DateTime.UtcNow;
                // TODO: should throw exception?
                //throw new OrderValidationException("Transaction time invalide.");
            }

            order.ActiveOrder(
                tansctionDto.Id,
                tansctionDto.Approved == "1",
                transactionType,
                transactionCreateTime,
                tansctionMeta
            );
		}
    }
}

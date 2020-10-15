using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.Trades
{
	public class OrderService : BaseService, IOrderService
    {
		private readonly ISalesOrderRepository _saleOrderRepository;

        public OrderService(
			ISalesOrderRepository saleOrderRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_saleOrderRepository = saleOrderRepository;
        }

		public async Task<PaginationDto<SalesOrderDto>> GetPagedOrderAsync(string userId, DateTime? from, DateTime? to, int pageNumber, int pageSize)
		{
			var orders = await _saleOrderRepository.GetPagedOrdersAsync(userId, from, to, pageNumber, pageSize);
			return Mapper.Map<PaginationDto<SalesOrderDto>>(orders);
		}

		public async Task<SalesOrderDto> GetOrderByNumberAsync(string orderNumber)
		{
			return Mapper.Map<SalesOrderDto>(await _saleOrderRepository.GetOrderByNumberAsync(orderNumber));
        }
        
		public async Task ActivateOrderAsync(string orderNumber)
		{
			throw new Exception();
			//var order = await _saleOrderRepository.GetOrderByNumberAsync(orderNumber);
			//if (order == null)
			//{
			//	throw new NotFoundException("order not found");	
			//}

			//order.Approve("Manully approved");
			//await _saleOrderRepository.SaveAsync();
        }

		public async Task<PaginationDto<SalesOrderDto>> GetSilentPostOrderAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize)
		{
			var orders = await _saleOrderRepository.GetSilentPostOrderAsync(from, to, pageNumber, pageSize);
            return Mapper.Map<PaginationDto<SalesOrderDto>>(orders);
		}

    }
}

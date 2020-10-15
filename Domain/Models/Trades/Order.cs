using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stateless;
using CourseStudio.Domain.Events.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.TraversalModel.Trades;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Doamin.Models.Trades
{
    public class Order: Entity, IAggregateRoot
    {
        public Order()
        {
            this.OrderItems = new List<LineItem>();
			this.TransactionRecords = new List<TransactionRecord>();
			this.OrderCoupons = new List<LineCoupon>();
            
            StateMachineInit();
        }

        public int Id { get; set; }
		[MaxLength(50)]
		public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public OrderStateEnum State { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
		[Range(0.0, 100.0)]
		public double DiscountPercent { get; private set; } // confilict with DiscountAmount
		[Range(0.0, Double.MaxValue)]
		public decimal DiscountAmount { get; private set; } // confilict with DiscountPercent
		public decimal AmountOriginal => OrderItems.Sum(i => i.PriceOriginal);
		public decimal AmountTotal => OrderItems.Sum(i => i.Price) * (decimal)(1.0 - DiscountPercent / 100.0) - DiscountAmount;
		public bool IsDiscounted => AmountTotal != AmountOriginal;

		// Navigation Property
		public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
		public ICollection<LineCoupon> OrderCoupons { get; set; }
		public ICollection<TransactionRecord> TransactionRecords { get; set; }

		/// <summary>
		/// Domain Logic
		/// </summary>
		public static Order Create(ApplicationUser user, ShoppingCart shoppingCart)
		{
			return new Order()
			{
				State = OrderStateEnum.Pending,
				User = user,
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow,
				OrderNumber = string.Format("CS{0}-{1}", user.Id.Substring(user.Id.Length - 8), DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
				DiscountPercent = shoppingCart.DiscountPercent,
				DiscountAmount = shoppingCart.DiscountAmount,
				OrderItems = shoppingCart.ShoppingCartItems,
				OrderCoupons = shoppingCart.ShoppingCartCoupons
			};
		}
        
		public void ActiveOrder(string tansctionId, bool isApproved, TransactionTypeEnum tansctionType, DateTime tansctionTime, string tansctionMeta) 
		{   
			var tansction = TransactionRecord.Create(
				tansctionId,
				isApproved,
				tansctionType,
				tansctionTime, 
                tansctionMeta
            );
			TransactionRecords.Add(tansction);

            if (isApproved) 
			{
                // redeem coupons if user used, and make sure apply to this user one time only
                var coupons = OrderCoupons.Select(oc => oc.Coupon);
                foreach(var c in coupons)
                {
                    c.Redeem(User);
                }
                Approve();
			}
			else 
			{
				Reject();
			}

		}

		public void PlaceOrder()
        {
			LastUpdateDateUTC = DateTime.UtcNow;
            if (State != OrderStateEnum.Processing)
            {
                _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
            }
        }

		public void PaymentFail()
        {
			LastUpdateDateUTC = DateTime.UtcNow;
			_machine.Fire(OrderStateTriggerEnum.PaymentFail);
        }

        public void Cancel()
        {
			LastUpdateDateUTC = DateTime.UtcNow;
            _machine.Fire(OrderStateTriggerEnum.Cancel);
        }

        public void Approve()
        {
			LastUpdateDateUTC = DateTime.UtcNow;
            _machine.Fire(OrderStateTriggerEnum.Approve);
			AddDomainEvent(new OrderCompleteDomainEvent(this));
        }
        
		public void Reject()
        {
			LastUpdateDateUTC = DateTime.UtcNow;
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

		/// <summary>
        /// State Machine Method
        /// </summary>
        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;

        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(() => State, s => State = s);

            // 理论上,每个user有且只有一个 pending order,但是代码无法体现
            _machine.Configure(OrderStateEnum.Pending)
                    .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Processing)
                    .Permit(OrderStateTriggerEnum.PaymentFail, OrderStateEnum.Pending);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Completed)
                    .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);

            _machine.OnUnhandledTrigger((state, trigger) =>
            {
                throw new StateUpdateException("state cannot be " + trigger.ToString() + " in " + state.ToString() + " state.");
            });
        }
    }
}

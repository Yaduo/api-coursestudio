using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Lib.Exceptions.Trades;
using CourseStudio.Lib.Utilities.Coupon;
using CourseStudio.Lib.Utilities.RuleEngine;

namespace CourseStudio.Doamin.Models.Trades
{
	public class Coupon : Entity, IAggregateRoot
	{
		public Coupon()
		{
			this.OrderCoupons = new List<OrderCoupon>();
			this.CouponRules = new List<CouponRule>();
			this.Scopes = new List<Scope>();
            this.CouponUsers = new List<CouponUser>();
        }

        public int Id { get; set; }
		[MaxLength(50)]
		public string Code { get; set; }
		[MaxLength(200)]
		public string Title { get; set; }
		[MaxLength(500)]
		public string Description { get; set; }
		public bool IsActivate { get; set; }
		public DateTime CreatDateUTC { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
        public DateTime? StartTimeUTC { get; set; }
        public DateTime? EndTimeUTC { get; set; }
        /// Navigation properties
        public ICollection<OrderCoupon> OrderCoupons { get; set; }
		public ICollection<CouponRule> CouponRules { get; set; }
		public ICollection<Scope> Scopes { get; set; }
        public ICollection<CouponUser> CouponUsers { get; set; }

        public static Coupon Create(string title, string description)
        {
            return new Coupon()
            {
                Code = CouponHelper.GenerateCouponCode(),
                IsActivate = false,
                CreatDateUTC = DateTime.UtcNow,
                LastUpdateDateUTC = DateTime.UtcNow
            };
        }

        public void Update(Coupon newCoupon) 
		{
			Title = newCoupon.Title;
			Description = newCoupon.Description;
			CouponRules = UpdateRules(newCoupon.CouponRules, CouponRules);
			Scopes = UpdateScopes(newCoupon.Scopes, Scopes);
            LastUpdateDateUTC = DateTime.UtcNow;
        }

        // TODO: 不明白删除rules时候, 为什么不能直接修改传入参数, 就先这样吧，以后再修改
        private IList<T> UpdateRules<T>(IEnumerable<T> newRules, ICollection<T> currentRules) where T: IRule
        {
            // 1. update exsiting rules' attributes
			foreach (var rule in currentRules)
            {
                foreach (var newRule in newRules)
                {
                    if (newRule.Id == rule.Id)
                    {
                        rule.Update(newRule);
                    }
                }
            }
            // 2. add rules
			var newRuleIds = newRules.Select(r => r.Id).Except(currentRules.Select(r => r.Id));
            foreach (var id in newRuleIds)
            {
				currentRules.Add(newRules.SingleOrDefault(r => r.Id == id));
            }
			// 3. remove rules
			var list = currentRules.ToList();
			var deprecatedRuleIds = currentRules.Select(r => r.Id).Except(newRules.Select(r => r.Id)).ToList();
            foreach (var id in deprecatedRuleIds)
            {
				list.Remove(currentRules.SingleOrDefault(r => r.Id == id));
            }
			return list;
        }

		// TODO: 不明白删除scope时候, 为什么不能直接修改传入参数, 就先这样吧，以后再修改
		private IList<Scope> UpdateScopes(IEnumerable<Scope> newScopes, ICollection<Scope> scopes)
        {
            // 1. update exsiting scope' attributes
			foreach (var scope in scopes)
            {
                foreach (var newScope in newScopes)
                {
                    if (newScope.Id == scope.Id)
                    {
                        scope.Update(newScope);
                    }
                }
            }
            // 2. add scopes
			var newScopeIds = newScopes.Select(s => s.Id).Except(scopes.Select(s => s.Id));
            foreach (var id in newScopeIds)
            {
				scopes.Add(newScopes.SingleOrDefault(s => s.Id == id));
            }
            // 3. remove scopes
            var deprecatedScopeIds = Scopes.Select(s => s.Id).Except(newScopes.Select(s => s.Id));
            foreach (var id in deprecatedScopeIds)
            {
				scopes.Remove(Scopes.SingleOrDefault(s => s.Id == id));
            }
			return scopes.ToList();
        }     

		public void Validate(ApplicationUser user) 
		{
            if (CouponUsers.Select(cu => cu.ApplicationUserId).Any(id => id == user.Id))
            {
                throw new CouponValidationException("The coupon has been used");
            }
            // Step 1: check the coupon states
            if (!IsActivate)
            {
				throw new CouponValidationException("The coupon " + Title + " is not valid.");
            }
            // Step 2: verify expird day
            if (EndTimeUTC < DateTime.UtcNow)
            {
				throw new CouponValidationException("The coupon "+ Title + " expired.");
            }
		}

		public bool IsCouponRulesApplicable(IList<LineItem> lineItems)
        {
            var preparedLineItems = new PreparedLineItemsForConpon
                (
					lineItems.Select(i => i.CourseId).ToList(),
					lineItems.Sum(i => i.Quantity),
					lineItems.Sum(i => i.Price)
                );
			var compiledRules = CouponRules.Select(RuleEngine.CompileRule<PreparedLineItemsForConpon>).ToList();
            return compiledRules.All(r => r(preparedLineItems));
        }

		public void Apply(ShoppingCart shoppingCart, ApplicationUser user) 
		{
			this.Validate(user);
			if (shoppingCart.ShoppingCartCoupons.Count > 0)
            {
                throw new CouponValidationException("Only one coupon can be applied.");
            }
			if (!IsCouponRulesApplicable(shoppingCart.ShoppingCartItems.ToList()))
            {
				throw new CouponValidationException("Coupon " + Title + "cannot be applied. " + Description);
            }
			foreach (var scope in Scopes)
			{
				scope.Apply(shoppingCart);
			}
		}

		public void Remove(ShoppingCart shoppingCart)
        {
            foreach (var scope in Scopes)
            {
				scope.Remove(shoppingCart);
            }
        }

        public void Redeem(ApplicationUser user) 
        {
            CouponUsers.Add(CouponUser.Create(user));
        }
    }
}

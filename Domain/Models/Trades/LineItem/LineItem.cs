using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Lib.Exceptions.Trades;

namespace CourseStudio.Doamin.Models.Trades
{
    public class LineItem: Entity
    {
        public int Id { get; set; }
        public int? ShoppingCartId { get; set; }
        public int? OrderId { get; set; }
        public int CourseId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		[Range(0.0, 100.0)]
		public double DiscountPercent { get; set; }
		[Range(0.0, Double.MaxValue)]
		public decimal DiscountAmount { get; set; }
		public decimal PriceOriginal => UnitPrice * Quantity;
		public decimal Price => UnitPrice * Quantity * (decimal)(1.0 - DiscountPercent / 100.0) - DiscountAmount;
		public bool IsDiscounted => PriceOriginal != Price;

        // Navigation Property
        public Course Course { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public Order Order { get; set; }

		public static LineItem Create(int quantity, ShoppingCart shoppingCart, Course course)
        {
			return new LineItem()
			{
				Quantity = quantity,
				ShoppingCart = shoppingCart,
				Course = course,
				DiscountAmount = course.DiscountAmount,
				DiscountPercent = course.DiscountPercent,
				UnitPrice = course.UnitPrice,
			};
        }

		public void Validate() 
		{
			if (!Course.IsActivate)
            {
				throw new ShoppingCartValidationException("The course " + Course.Title + " is invalid.");
            }
			if (Price < 0) 
			{
				throw new ShoppingCartValidationException("The price of " + Course.Title + " is invalid.");
			}
		}

		public void ApplyDiscount(double discountPercent, decimal discountAmount)
        {
			// TODO: 不可为负数！
			DiscountPercent += discountPercent;
			DiscountAmount += discountAmount;
        }

		public void RemoveDiscount(double discountPercent, decimal discountAmount)
		{
			// TODO: 需要check课程原本的折扣，不可以覆盖课程原本折扣
			DiscountPercent -= discountPercent;
			DiscountAmount -= discountAmount;
		}
    }
}

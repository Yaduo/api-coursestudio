using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Application.Dtos.Trades
{
    public class LineItemDto
    {
		public int Id { get; set; }
        public int? ShoppingCartId { get; set; }
        public int? OrderId { get; set; }
        public int CourseId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountAmount { get; set; }
		public double PriceOriginal { get; set; }
		public double Price { get; set; }
		public bool IsDiscounted { get; set; }
        public CourseDto Course { get; set; }
    }
}

using System;
namespace CourseStudio.Application.Dtos.Trades
{
    public class AddShoppingCartItemDto
    {
		public int CourseId { get; set; }
        public int Quantity { get; set; }
    }
}

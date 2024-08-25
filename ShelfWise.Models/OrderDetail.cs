using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShelfWise.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }

		[Required]
		public int OrderHeaderId { get; set; }
		[ForeignKey("OrderHeaderId")]
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }
		
		[Required]
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }

		public int Count { get; set; } // Count for each product
		public double Price { get; set; } // In case the price of the product changes in the future, we want to keep log of the price at the time of purchase
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPNETCoreDbFirst.DbModels;

namespace ASPNETCoreDbFirst.Models
{
    public class OrderViewModel
    {
        public class OrderTabVM
        {
            public int OrderId { get; set; }
            [Required]
            public string? OrderNumber { get; set; }
            [ForeignKey("Customer")]
            public int CustomerId { get; set; }
            [Required]
            public DateTime OrderDate { get; set; }
            [Required]
            public decimal? SubTotal { get; set; }
            [Required]
            public decimal? Discount { get; set; }
            [Required]
            public decimal? ShippingFee { get; set; }
            [Required]
            public decimal? NetTotal { get; set; }
            [Required]
            public int StatusId { get; set; }

        }
        public class OrderItemVM
        {
            [Key]
            public int OrderItemId { get; set; }
            [ForeignKey("OrderTab")]
            public int OrderId { get; set; }
            [ForeignKey("Product")]
            public int? ProductId { get; set; }

            public int? Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal TotalPrice { get; set; }
        }
    }
}

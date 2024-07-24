using ASPNETCoreDbFirst.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreDbFirst.Models
{
    public class OrderTabVM
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string? OrderNumber { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Discount { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal? NetTotal { get; set; }

        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        [Required]
        [ForeignKey("StatusTab")]
        public int? StatusId { get; set; }
        public int OrderItemId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

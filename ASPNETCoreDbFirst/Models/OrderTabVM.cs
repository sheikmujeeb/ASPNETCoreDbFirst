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

        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal? SubTotal { get; set; }
        [Required]
        public decimal? Discount { get; set; }
        [Required]
        public decimal? ShippingFee { get; set; }
        [Required]
        public decimal? NetTotal { get; set; }

        public string ProductName { get; set; } =null!;
        public string CustomerName { get; set; } = null!;
        [Required]
        public int? StatusId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

   
    }
}

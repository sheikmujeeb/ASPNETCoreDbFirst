using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreDbFirst.Models
{
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

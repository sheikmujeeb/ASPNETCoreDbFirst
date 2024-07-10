using ASPNETCoreDbFirst.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreDbFirst.Models
{
    public class OrderTabVM
    {
        [Key]
        public int OrderId { get; set; }

        public string? OrderNumber { get; set; }
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Discount { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal? NetTotal { get; set; }
        [ForeignKey("StatusTab")]
        public int StatusId { get; set; }
    }
}

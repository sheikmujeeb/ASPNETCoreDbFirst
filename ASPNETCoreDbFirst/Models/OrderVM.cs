using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPNETCoreDbFirst.DbModels;

namespace ASPNETCoreDbFirst.Models
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal? TotalAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsDeleted { get; set; }
        [Required]
        public bool IsActive { get; set; }
        //[Required]
        //public virtual Customer Customer { get; set; } = null!;
        //[Required]
        //public virtual Product Product { get; set; } = null!;
    }
}

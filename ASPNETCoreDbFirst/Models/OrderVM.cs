using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPNETCoreDbFirst.DbModels;

namespace ASPNETCoreDbFirst.Models
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        [Required]
   
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        [Required]
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
      
        public decimal? TotalAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsDeleted { get; set; }
    }
}

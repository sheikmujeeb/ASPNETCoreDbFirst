using System.ComponentModel.DataAnnotations;




namespace ASPNETCoreDbFirst.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Code { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsDeleted { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}

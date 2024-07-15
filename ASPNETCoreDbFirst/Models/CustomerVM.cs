using System.ComponentModel.DataAnnotations;


namespace ASPNETCoreDbFirst.Models
{
    public class CustomerVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        [StringLength(10)]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsDeleted { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}

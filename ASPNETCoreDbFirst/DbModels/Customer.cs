using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ASPNETCoreDbFirst.DbModels;

public partial class Customer
{
    public int CustomerId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsDeleted { get; set; }
    [Required]
    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

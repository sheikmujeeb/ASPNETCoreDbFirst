using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreDbFirst.DbModels;

public partial class Product
{
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Code { get; set; } 

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsDeleted { get; set; }
    [Required]
    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

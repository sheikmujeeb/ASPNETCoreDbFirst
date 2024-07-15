using System;
using System.Collections.Generic;

namespace ASPNETCoreDbFirst.DbModels;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } 

    public string Code { get; set; } 

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

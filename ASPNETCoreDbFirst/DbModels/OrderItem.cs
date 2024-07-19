using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreDbFirst.DbModels;

public partial class OrderItem
{
    public int OrderItemId { get; set; }
    [ForeignKey("OrderTab")]
    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual OrderTab Order { get; set; } = null!;

    public virtual Product? Product { get; set; }
}

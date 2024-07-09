using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreDbFirst.DbModels;

public partial class OrderTab
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

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual StatusTab Status { get; set; } = null!;
}


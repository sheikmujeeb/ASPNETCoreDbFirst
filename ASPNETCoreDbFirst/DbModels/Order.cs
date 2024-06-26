﻿using System;
using System.Collections.Generic;

namespace ASPNETCoreDbFirst.DbModels;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public DateTime OrderDate { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

﻿using System;
using System.Collections.Generic;

namespace ASPNETCoreDbFirst.DbModels;

public partial class Customer
{
    
    public int CustomerId { get; set; }

    public string Name { get; set; } 

    public string Email { get; set; } 

    public string PhoneNumber { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public DateTime? IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

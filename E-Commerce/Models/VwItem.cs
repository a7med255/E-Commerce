using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class VwItem
{
    public string ItemName { get; set; } = null!;

    public decimal PurchasePrice { get; set; }

    public decimal SalesPrice { get; set; }

    public int CategoryId { get; set; }

    public string? ImageName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int CurrentState { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }

    public int? ItemTypeId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string ItemTypeName { get; set; } = null!;

    public int ItemId { get; set; }
}

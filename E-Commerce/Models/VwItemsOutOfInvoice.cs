using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class VwItemsOutOfInvoice
{
    public string? ItemName { get; set; }

    public string CategoryName { get; set; } = null!;

    public decimal? InvoicePrice { get; set; }

    public decimal? PurchasePrice { get; set; }
}

using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class TbItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal SalesPrice { get; set; }

    public decimal PurchasePrice { get; set; }

    public int CategoryId { get; set; }

    public string? ImageName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int CurrentState { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }

    public int? ItemTypeId { get; set; }

    public virtual TbCategory Category { get; set; } = null!;

    public virtual TbItemType? ItemType { get; set; }

    public virtual ICollection<TbItemDiscount> TbItemDiscounts { get; set; } = new List<TbItemDiscount>();

    public virtual ICollection<TbPurchaseInvoiceItem> TbPurchaseInvoiceItems { get; set; } = new List<TbPurchaseInvoiceItem>();

    public virtual ICollection<TbSalesInvoiceItem> TbSalesInvoiceItems { get; set; } = new List<TbSalesInvoiceItem>();

    public virtual ICollection<TbCustomer> Customers { get; set; } = new List<TbCustomer>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

}

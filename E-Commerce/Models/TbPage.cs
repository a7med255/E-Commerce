using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class TbPage
{
    public int PageId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string MetaKeyWord { get; set; } = null!;

    public string MetaDescriptiuon { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public int CurrentState { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }
}

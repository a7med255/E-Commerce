using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class TbWishlistItem
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int ProductId { get; set; }

    public DateTime DateAdded { get; set; }
}

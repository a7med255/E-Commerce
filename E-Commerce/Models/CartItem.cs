namespace E_Commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public TbItem Item { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
    }
}
    
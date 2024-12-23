namespace E_Commerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public TbItem Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}

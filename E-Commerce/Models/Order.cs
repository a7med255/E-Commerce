namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }  // e.g., Pending, Completed
    }

}

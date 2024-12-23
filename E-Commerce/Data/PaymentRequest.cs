namespace E_Commerce.Data
{
    public class PaymentRequest
    {
        public List<PaymentItem> Items { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
    public class PaymentItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}

namespace Motor.Models
{
    public class OrderDetail
    {
        public string Id { get; set; }
        public string? orderId { get; set; }

        public string? motorId { get; set; }

        public string? price { get; set; }

        public int Quantity { get; set; }
    }
}

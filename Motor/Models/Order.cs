namespace Motor.Models
{
    public class Order
    {
        public string? orderId { get; set; }

        public string? Createdby { get; set; }

        public DateTime Createddate { get; set; }

        public string? totalPrice { get; set; }

        public string? address { get; set; }


        public int? Status { get; set; }
    }
}

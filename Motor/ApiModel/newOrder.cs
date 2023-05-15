namespace Motor.ApiModel
{
    public class newOrder
    {
        public List<CartOrder> carts { get; set; } = null!;
        public string address { get; set; } = null!;
    }
}

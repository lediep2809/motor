using Motor.Models;

namespace Motor.ApiModel
{
    public class listChartOrder
    {
        public List<Order>? orders { get; set; }
        public int? total { get; set; }
    }
}

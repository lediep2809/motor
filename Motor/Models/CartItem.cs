using System.ComponentModel.DataAnnotations;

namespace Motor.Models
{
    public class CartItem
    {

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public string motorId { get; set; }

        public DateTime DateCreated { get; set; }

        public string createBy { get; set; }

        public string totalprice { get; set; }
    }
}

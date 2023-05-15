namespace Motor.Models
{
    public class Paging
    {
  
        public string? SearchQuery { get; set; }
        public string? Price { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int  PriceSale{ get; set; }
    }
}

using Motor.Models;

namespace Motor.ApiModel
{
    public class listBlogs
    {
        public List<Blog>? blogs { get; set; }
        public int? total { get; set; }
    }
}

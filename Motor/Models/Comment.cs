namespace Motor.Models
{
    public class Comment
    {
        public string? Id { get; set; }
        public string? Createdby { get; set; }
        public string? motorId { get; set; }
        public string? comment { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifyDate { get; set; }
    }
}

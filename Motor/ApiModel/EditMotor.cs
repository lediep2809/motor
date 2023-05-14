namespace Motor.ApiModel
{
    public class EditMotor
    {

        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string? Description { get; set; }

        public string Price { get; set; } = null!;

        public string? Createdby { get; set; }

        public DateTime Createddate { get; set; }

        public int? Status { get; set; }

        public string[] imgMotor { get; set; }


        public int? farmous { get; set; }

        public string? salePrice { get; set; }

    }
}

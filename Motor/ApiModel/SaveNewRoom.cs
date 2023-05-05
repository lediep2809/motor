﻿namespace Motor.ApiModel
{
    public class SaveNewRoom
    {

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string Category { get; set; } = null!;

        public string Area { get; set; } = null!;

        public string Capacity { get; set; } = null!;

        public string? Description { get; set; }

        public string Price { get; set; } = null!;

        public string? Deposit { get; set; }

        public string? Electricprice { get; set; }

        public string? Waterprice { get; set; }

        public string? Otherprice { get; set; }

        public string? Houseowner { get; set; }

        public string? Ownerphone { get; set; }

        public string[] imgRoom { get; set; }

        public string? noSex { get; set; }

        public string[] utilities { get; set; }
    }
}

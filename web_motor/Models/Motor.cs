using System;
using System.Collections.Generic;

namespace web_motor.Models;

public partial class Motor
{
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public string Price { get; set; } = null!;

    public string? Createdby { get; set; }
    
    public DateTime Createddate { get; set; }

    public int? Status { get; set; }
}

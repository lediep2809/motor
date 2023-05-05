using System;
using System.Collections.Generic;

namespace web_motor.Models;

public partial class Type
{
    public string? Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Status { get; set; } 

}

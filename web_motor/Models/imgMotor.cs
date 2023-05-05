using System;
using System.Collections.Generic;

namespace web_motor.Models;

public partial class imgMotor
{
    public string Id { get; set; } = null!;

    public string? Imgbase64 { get; set; }

    public string? idMotor { get; set; }
}

using System;
using System.Collections.Generic;

namespace web_motor.Models;

public class editUser
{

    public string Phone { get; set; } = null!;

    // public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public int? Status { get; set; }

    public string Roleid { get; set; }

    public string? Email { get; set; }
}

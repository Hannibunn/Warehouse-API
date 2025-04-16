using System;
using System.Collections.Generic;

namespace Warehouse_API_Test.Models;

public partial class Box
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Qrcode { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}

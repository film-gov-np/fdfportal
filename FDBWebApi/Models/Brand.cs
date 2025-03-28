using System;
using System.Collections.Generic;

namespace FDBWebApi.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandCode { get; set; } = null!;

    public string BrandName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ApiUsername { get; set; }

    public string? ApiPassword { get; set; }

    public int StatusValue { get; set; }

    public bool IsTest { get; set; }

    public DateTime AddedOn { get; set; }

    public string AddedBy { get; set; } = null!;

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsModified { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }
}

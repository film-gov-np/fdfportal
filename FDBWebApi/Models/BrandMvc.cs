using System;
using System.Collections.Generic;

namespace FDBWebApi.Models;

public partial class BrandMvc
{
    public int Id { get; set; }

    public string? BrandCode { get; set; }

    public string? BrandName { get; set; }

    public string? Email { get; set; }

    public string? ApiUsername { get; set; }

    public string? ApiPassword { get; set; }

    public int? StatusValue { get; set; }

    public bool? IsTest { get; set; }

    public DateTime? AddedOn { get; set; }

    public string? AddedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsModified { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public int? BrandId { get; set; }
}

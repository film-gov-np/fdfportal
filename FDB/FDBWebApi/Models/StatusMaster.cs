using System;
using System.Collections.Generic;

namespace FDBWebApi.Models;

public partial class StatusMaster
{
    public int Id { get; set; }

    public string? StatusCodeCategory { get; set; }

    public int StatusCodeId { get; set; }

    public string? StatusCodeName { get; set; }

    public string? StatusCodeDescription { get; set; }

    public int? DisplayOrder { get; set; }
}

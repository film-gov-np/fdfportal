using System;
using System.Collections.Generic;

namespace FDBWebApi.Models;

public partial class Theatre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public string? LastUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? TheatreCode { get; set; }

    public string? BrandCode { get; set; }

    public string? Email { get; set; }

    public long? Pannumber { get; set; }

    public long? RegNumber { get; set; }

    public int? TheatreId { get; set; }

    public long? Vatnumber { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    public virtual ICollection<ReceiptUpload> ReceiptUploads { get; set; } = new List<ReceiptUpload>();
}

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace AdminLTE.MVC.Models
{
    public class Theatre
    {
        public int Id { get; set; }
        public int? TheatreId { get; set; }
        public string TheatreCode  { get; set; }
        public string BrandCode  { get; set; }
        public string Email  { get; set; }
        public string RegNumber  { get; set; }
        public string PANNumber { get; set; }
        public string VATNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public int? IRDOfficeId { get; set; }


        public IRDOffice IRDOffice { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        
        public DateTime LastUpdatedAt { get; set; }

        public string LastUpdatedBy { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public List<ReceiptUpload> ReceiptUploads { get; set; }

    }
}

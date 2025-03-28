using System;
using System.Collections.Generic;

namespace AdminLTE.MVC.Models
{
    public class IRDOffice
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string AccountNo { get; set; }
        public string AccountOperatingOffice { get; set; } 
        public string PANNumber { get; set; }
        public string VATNumber { get; set; }
        public string Email { get; set; }
        public string RegNumber { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Theatre> Theatres { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Models;

namespace AdminLTE.MVC.ViewModels
{
    public class TheaterVM
    {
        public int Id { get; set; }
        public string TheatreCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }
        public string Phone { get; set; }

        public string CreatedBy { get; set; }
        public int? TheaterId { get; set; }
        public int? IRDOfficeId { get; set; }
        public IRDOffice IRDOffice { get; set; }


        public string LastUpdatedBy { get; set; }
        public string PANNumber { get; set; }
        


        public string Vatnumber { get; set; } 
        public string RegNumber { get; set; } 

        public string Email { get; set; } 
        public string BrandCode { get; set; }

       
       



        public List<ApplicationUser> Users { get; set; }
        
    }
}
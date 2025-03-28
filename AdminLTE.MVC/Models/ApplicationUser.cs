using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace AdminLTE.MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }
        public int? TheatreId { get; set; }
        public bool IsActive { get; set; } = true;
        public Theatre Theatre { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

    }
}

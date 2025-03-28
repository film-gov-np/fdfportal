using System.ComponentModel.DataAnnotations;
using AdminLTE.MVC.Utilites;
using Microsoft.AspNetCore.Http;

namespace AdminLTE.MVC.ViewModels
{
    public class RegisterVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Roles { get; set; }

        public int? TheaterId { get; set; }
        public string PictureUrl { get; set; }
        public IFormFile UserImg { get; set; }
        public string Address { get; set; }



    }
}

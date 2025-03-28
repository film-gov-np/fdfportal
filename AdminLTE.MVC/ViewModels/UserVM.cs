using Microsoft.AspNetCore.Http;

namespace AdminLTE.MVC.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string TheaterName { get; set; }
        public int? TheaterId { get; set; }
        public string PictureUrl { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public IFormFile UserImg { get; set; }

    }
}

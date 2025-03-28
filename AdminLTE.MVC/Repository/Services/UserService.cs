using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Services
{
    public class UserService : IUserService
    {
        public Task<List<TheaterVM>> GetAllTheaterRoleAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}

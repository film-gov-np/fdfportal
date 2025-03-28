using AdminLTE.MVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface IUserService
    {
        Task<List<TheaterVM>> GetAllTheaterRoleAsync();

    }
}

using AdminLTE.MVC.Repository.Services;
using AdminLTE.MVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface IMovieService
    {
        Task<List<MovieVM>> GetAllMovieAsync();
       
        Task<MovieVM> GetMovieByIdAsync(int Id);
        Task<string> AddMovieAsync(MovieVM ItemModel);
        Task<string> UpdateMovieAsync(int Id, MovieVM ItemModel);
        Task<string> DeleteMovieAsync(int Id);
        Task<int> GetFBDMovieById();

        Task<List<MovieResponseModel>> GetMovieDataFromApiAsync(int movieId);

        
    }
}

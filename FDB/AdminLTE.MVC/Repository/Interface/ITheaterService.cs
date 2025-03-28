using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.ViewModels;
using Microsoft.AspNetCore.JsonPatch;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface ITheaterService
    {
        Task<List<TheaterVM>> GetAllTheaterAsync();
        Task<TheaterVM> GetTheaterByIdAsync(int Id);
        Task<string> AddTheaterAsync(TheaterVM ItemModel);
        Task<string> UpdateTheaterAsync(int Id, TheaterVM ItemModel);
        Task<string> DeleteTheaterAsync(int Id);
        Task<int> GetFBDTheaterId();
        Task<List<TheaterResponseModel>> GetTheaterDataFromApiAsync(int theaterId);
        

        
    }
}
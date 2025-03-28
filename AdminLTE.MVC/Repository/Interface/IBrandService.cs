using AdminLTE.MVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface IBrandService
    {
        Task<List<BrandVM>> GetAllBrandAsync();

        Task<BrandVM> GetBrandByIdAsync(int Id);
        Task<string> AddBrandAsync(BrandVM ItemModel);
        Task<string> UpdateBrandAsync(int Id, BrandVM ItemModel);
        Task<string> DeleteBrandAsync(int Id);
        Task<int> GetFBDBrandById();

        Task<List<BrandResponseModel>> GetBrandDataFromApiAsync(int brandId);
    }
}

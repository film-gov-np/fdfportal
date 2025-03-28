using AdminLTE.MVC.Data;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public BrandService(ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                ILogger<BrandService> logger,
                                IHttpClientFactory clientFactory)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<string> AddBrandAsync(BrandVM ItemModel)
        {
            try
            {
                var brand = new BrandMVC()
                {
                    BrandCode = ItemModel.BrandCode,
                    BrandName = ItemModel.BrandName,
                    Email = ItemModel.Email,
                    ApiUsername = ItemModel.ApiUsername,
                    ApiPassword = ItemModel.ApiPassword,
                    StatusValue = ItemModel.StatusValue,
                    IsTest = ItemModel.IsTest,
                    AddedOn = DateTime.Now,
                    AddedBy = ItemModel.AddedBy,    
                    IsModified = ItemModel.IsModified,
                    IsDeleted = ItemModel.IsDeleted,
                    BrandID = ItemModel.BrandID,
                };
                await _context.AddAsync(brand);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                return "Error";
            }


        }

       
        public async Task<string> DeleteBrandAsync(int id)
        {
            try
            {
                var brand = await _context.BrandMVCs.FirstOrDefaultAsync(x => x.Id == id);

                _context.BrandMVCs.Remove(brand);
                await _context.SaveChangesAsync();
                return "Success";
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the brand with Id {BrandId}", id);
                return "An error occurred while processing your request. Please try again.";
            }

        }

        public async Task<List<BrandVM>> GetAllBrandAsync()
        {
            try
            {
                var result = await _context.BrandMVCs.Select(x => new BrandVM
                {
                    Id = x.Id,
                    BrandCode = x.BrandCode,
                    BrandName = x.BrandName,
                    Email = x.Email,
                    ApiUsername = x.ApiUsername,
                    ApiPassword = x.ApiPassword,
                    StatusValue = x.StatusValue,
                    IsTest = x.IsTest,
                    AddedOn = x.AddedOn,
                    AddedBy = x.AddedBy,
                    UpdatedOn = x.UpdatedOn,
                    UpdatedBy = x.UpdatedBy,

                    IsModified = x.IsModified,
                    DeletedOn = x.DeletedOn,
                    DeletedBy = x.DeletedBy,
                    IsDeleted = x.IsDeleted,
                    BrandID = x.BrandID,

                }).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<BrandVM> GetBrandByIdAsync(int Id)
        {
            var result = await _context.BrandMVCs.FindAsync(Id);
            return _mapper.Map<BrandVM>(result);
        }

        public async Task<List<BrandResponseModel>> GetBrandDataFromApiAsync(int brandId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5076/api/Brand/{brandId}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BrandResponseModel>>(responseData);
            }
            return new List<BrandResponseModel>();
        }

        public async Task<int> GetFBDBrandById()
        {
            try
            {
                // Fetch the list of TheatreIds
                var BrandIds = await _context.BrandMVCs.Select(x => (int?)x.BrandID).ToListAsync();

                // Find the maximum TheatreId or return 0 if the list is empty
                return BrandIds.DefaultIfEmpty(0).Max() ?? 0;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateBrandAsync(int Id, BrandVM ItemModel)
        {
            try
            {
                var result = await _context.BrandMVCs.FindAsync(Id);
                if (result == null)
                {
                    return "Brand not Found";

                }
                result.BrandCode = ItemModel.BrandCode;
                result.BrandName = ItemModel.BrandName;
                result.Email = ItemModel.Email;
                result.ApiUsername = ItemModel.ApiUsername;
                result.ApiPassword = ItemModel.ApiPassword;
                result.StatusValue = ItemModel.StatusValue;
                result.IsTest = ItemModel.IsTest;
                result.AddedOn = DateTime.Now;
                result.AddedBy = ItemModel.AddedBy;
                result.UpdatedOn = DateTime.Now;
                result.UpdatedBy = ItemModel.UpdatedBy;
                result.IsModified = ItemModel.IsModified;
                result.DeletedOn = DateTime.Now;
                result.DeletedBy = ItemModel.DeletedBy;
                result.IsDeleted = ItemModel.IsDeleted;
                result.BrandID = ItemModel.BrandID;
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

    }
}

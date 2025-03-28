using AdminLTE.MVC.Data;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieService> _logger;
            private readonly IHttpClientFactory _clientFactory;

        public MovieService(ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                ILogger<MovieService> logger,
                                 IHttpClientFactory clientFactory)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager; 
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<string> AddMovieAsync(MovieVM ItemModel)
        {
            try
            {
                if (_context.MovieMVCs.Any(x => x.Name == ItemModel.Name))
                {
                    return "Movie name Cannot be Same";
                }
                var model = new MovieMVC()
                {
                    MovieID = ItemModel.MovieID,
                    MovieCode = ItemModel.MovieCode,
                    Name = ItemModel.Name,
                    GradeCode = ItemModel.GradeCode,
                    ProductionHouseCode = ItemModel.ProductionHouseCode,
                    LanguageCode = ItemModel.LanguageCode,
                    ProductionType = ItemModel.ProductionType,
                    ReleaseDate = ItemModel.ReleaseDate,
                    StatusValue = ItemModel.StatusValue,
                    Slug = ItemModel.Slug,
                    IsModified = ItemModel.IsModified,
                    IsDeleted = ItemModel.IsDeleted,
                    CanSpecifyMovieName = ItemModel.CanSpecifyMovieName,
                    SendMovieCodesToExhibitorEmail = ItemModel.SendMovieCodesToExhibitorEmail,
                    SendNotificationToDistributors = ItemModel.SendNotificationToDistributors,
                    SendNotificationToProducers = ItemModel.SendNotificationToProducers,
                    AddedBy = ItemModel.AddedBy,
                    AddedOn = DateTime.Now,
                };

                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return "Success";
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                return "Error";
            }
        }
         
        public async Task<string> DeleteMovieAsync(int Id)
        {
            try
            {
                var result = await _context.MovieMVCs!.FirstOrDefaultAsync(x => x.Id == Id);
                if(result == null)
                {
                    return "No Movie Found";
                }
                _context.MovieMVCs.Remove(result);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while processing the request.");

                throw;
            }
        }

        public async Task<List<MovieVM>> GetAllMovieAsync()
        {
            try
            {
                var result = await _context.MovieMVCs.Select(x => new MovieVM
                { 
                    Id = x.Id,  
                    MovieID = x.MovieID,
                    MovieCode = x.MovieCode,
                    Name = x.Name,
                    GradeCode = x.GradeCode,
                    ProductionHouseCode = x.ProductionHouseCode,
                    LanguageCode = x.LanguageCode,
                    ProductionType = x.ProductionType,
                    ReleaseDate = x.ReleaseDate,
                    StatusValue = x.StatusValue,
                    Slug = x.Slug,
                    IsModified = x.IsModified,
                    IsDeleted = x.IsDeleted,
                    CanSpecifyMovieName = x.CanSpecifyMovieName,
                    SendMovieCodesToExhibitorEmail =x.SendMovieCodesToExhibitorEmail,
                    SendNotificationToProducers =x.SendNotificationToProducers,
                    SendNotificationToDistributors = x.SendNotificationToDistributors,
                    AddedOn = x.AddedOn,
                    AddedBy = x.AddedBy,

                }).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<MovieVM> GetMovieByIdAsync(int Id)
        {
            var result= await _context.MovieMVCs.FindAsync(Id);
            return _mapper.Map<MovieVM>(result);
        }

        public async Task<string> UpdateMovieAsync(int Id, MovieVM ItemModel)
        {
            try
            {
                var result = await _context.MovieMVCs.FindAsync(Id);
                if (result == null)
                {
                    return "Movie not Found";

                }
                result.MovieID = ItemModel.MovieID;
                result.MovieCode = ItemModel.MovieCode;
                result.Name = ItemModel.Name;
                result.GradeCode = ItemModel.GradeCode;
                result.ProductionHouseCode = ItemModel.ProductionHouseCode;
                result.LanguageCode = ItemModel.LanguageCode;
                result.ProductionType = ItemModel.ProductionType;
                result.ReleaseDate = DateOnly.FromDateTime(DateTime.Now);
                result.StatusValue = ItemModel.StatusValue;
                result.Slug = ItemModel.Slug;
                result.IsModified = ItemModel.IsModified;
                result.IsDeleted = false;
                result.CanSpecifyMovieName = ItemModel.CanSpecifyMovieName;
                result.SendMovieCodesToExhibitorEmail = ItemModel.SendMovieCodesToExhibitorEmail;
                result.SendNotificationToProducers = ItemModel.SendNotificationToProducers;
                result.SendNotificationToDistributors = ItemModel.SendNotificationToDistributors;
                result.UpdatedOn = DateTime.Now;
                result.UpdatedBy = ItemModel.UpdatedBy;
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
        public async Task<int> GetFBDMovieById()
            {
                try
                {
                    // Fetch the list of TheatreIds
                    var MovieIds = await _context.MovieMVCs.Select(x => (int?)x.MovieID).ToListAsync();

                    // Find the maximum TheatreId or return 0 if the list is empty
                    return MovieIds.DefaultIfEmpty(0).Max() ?? 0;
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        public async Task<List<MovieResponseModel>> GetMovieDataFromApiAsync(int movieId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5076/api/Movie/{movieId}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MovieResponseModel>>(responseData);
            }
            return new List<MovieResponseModel>();
        }
       
}
}

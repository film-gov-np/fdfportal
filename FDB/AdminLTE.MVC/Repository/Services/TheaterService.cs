using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

namespace AdminLTE.MVC.Repository.Services
{
    public class TheaterService : ITheaterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<TheaterService> _logger;
    private readonly IHttpClientFactory _clientFactory;


        public TheaterService(ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                ILogger<TheaterService> logger,
                                IHttpClientFactory clientFactory)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<string> AddTheaterAsync(TheaterVM TheaterModel)
        {
            try
            {
                // Check if a theater with the same name already exists
                if (_context.Theatres.Any(x => x.Name == TheaterModel.Name))
                {
                    return "Theater Name Already Exists";
                }
                // Create a new theater model from the view model
                var Model = new Theatre()
                {
                    Name = TheaterModel.Name,
                    TheatreCode = TheaterModel.TheatreCode,
                    Location = TheaterModel.Location,
                    Phone = TheaterModel.Phone,
                    Description = TheaterModel.Description,
                    CreatedBy = TheaterModel.CreatedBy,
                    BrandCode = TheaterModel.BrandCode,
                    PANNumber = TheaterModel.PANNumber,
                    RegNumber = TheaterModel.RegNumber,
                    VATNumber = TheaterModel.Vatnumber,
                    IRDOfficeId = TheaterModel.IRDOfficeId,
                    Email = TheaterModel.Email,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    TheatreId = TheaterModel.TheaterId,
                    

                };

                // Add the new theater to the context
                await _context.AddAsync(Model);
                await _context.SaveChangesAsync();

                // Return success message
                return "Success";
            }
            catch (Exception ex) // Catch the exception and define the variable
            {
                // Log the exception (you can log it to a file, database, etc. as per your logging strategy)
                _logger.LogError(ex, "An error occurred while adding a theater");

                // Return error message
                return "Error";
            }
        }


        public async Task<string> DeleteTheaterAsync(int Id)
        {
            try
            {
                var theater = await _context.Theatres!.Include(x=>x.Users).Include(x=> x.ReceiptUploads).FirstOrDefaultAsync(x => x.Id == Id);
                
                if (theater == null)
                {
                    return "Theater not found";
                }

                if (theater.Users.Count != 0)
                {
                    return "User is present for this Theater";
                }
                if (theater.ReceiptUploads.Count != 0)
                {
                    return "Receipt is present for this Theater";
                }

                _context.Theatres!.Remove(theater!);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }


        public async Task<List<TheaterVM>> GetAllTheaterAsync()
        {
            try
            {
                
                var result = await _context.Theatres.Include(x=>x.IRDOffice).Select(x=>new TheaterVM{
                    Id = x.Id,
                    Name = x.Name,
                    TheatreCode = x.TheatreCode,
                    Location=x.Location,
                    CreatedBy = x.CreatedBy,
                    Phone = x.Phone,
                    PANNumber = x.PANNumber,
                    IRDOffice = x.IRDOffice,

                    BrandCode = x.BrandCode,
                    IRDOfficeId = x.IRDOfficeId,
                    

                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<int> GetFBDTheaterId()
            {
                try
                {
                    // Fetch the list of TheatreIds
                    var theatreIds = await _context.Theatres.Select(x => (int?)x.TheatreId).ToListAsync();

                    // Find the maximum TheatreId or return 0 if the list is empty
                    return theatreIds.DefaultIfEmpty(0).Max() ?? 0;
                }
                catch (System.Exception)
                {
                    throw;
                }
            }


        public async Task<TheaterVM> GetTheaterByIdAsync(int Id)
        {
            var result = await _context.Theatres.Include(x=>x.IRDOffice).Where(x=>x.Id == Id).FirstOrDefaultAsync();
            return _mapper.Map<TheaterVM>(result);//here book model is a target class and the book is the source
        }


        public async Task<string> UpdateTheaterAsync(int Id, TheaterVM ItemModel)
        {
            try
            {
                if (_context.Theatres.Any(x => x.Name == ItemModel.Name && x.Id != Id))
                {
                    return "Theater Name Already Exists";
                }
                var result = await _context.Theatres.FindAsync(Id);
                if (result == null)
                {
                    return "Theater Not Found";
                }
                result.Name = ItemModel.Name;
                result.PANNumber = ItemModel.PANNumber;
                result.Location = ItemModel.Location;
                result.Description = ItemModel.Description;
                result.Phone = ItemModel.Phone;
                result.TheatreCode = ItemModel.TheatreCode;
                result.IRDOfficeId = ItemModel.IRDOfficeId;
                result.LastUpdatedBy = ItemModel.LastUpdatedBy;
                result.LastUpdatedAt = DateTime.Now;
                result.BrandCode = ItemModel.BrandCode;
                result.IRDOfficeId = ItemModel.IRDOfficeId;
               

                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
        public async Task<List<TheaterResponseModel>> GetTheaterDataFromApiAsync(int theaterId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5076/api/Theater/{theaterId}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TheaterResponseModel>>(responseData);
            }
            return new List<TheaterResponseModel>();
        }
    }
}
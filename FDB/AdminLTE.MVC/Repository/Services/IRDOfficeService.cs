using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;

namespace AdminLTE.MVC.Repository.Services
{
    public class IRDOfficeService : IIRDOfficeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<IRDOfficeService> _logger;
        private readonly IHttpClientFactory _clientFactory;


        public IRDOfficeService(ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                ILogger<IRDOfficeService> logger,
                                IHttpClientFactory clientFactory)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<string> AddIRDOfficeAsync(IRDOfficeVM ItemModel)
        {
            try
            {
                // Check if a theater with the same name already exists
                if (_context.IRDOffices.Any(x => x.AccountOperatingOffice == ItemModel.AccountOperatingOffice))
                {
                    return "IRD Office Name Already Exists";
                }

                var Model = new IRDOffice()
                {
                    BankName = ItemModel.BankName,
                    BankBranchName = ItemModel.BankBranchName,
                    AccountNo = ItemModel.AccountNo,
                    AccountOperatingOffice = ItemModel.AccountOperatingOffice,
                    PANNumber = ItemModel.PANNumber,
                    VATNumber = ItemModel.VATNumber,
                    Email = ItemModel.Email,
                    RegNumber = ItemModel.RegNumber,
                    Location = ItemModel.Location,
                    Phone = ItemModel.Phone,
                    IsActive = true,
                    CreatedBy = ItemModel.CreatedBy,

                    CreatedAt = DateTime.Now,
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

        public async Task<string> DeleteIRDOfficeAsync(int Id)
        {
            try
            {
                var IRDOffice = await _context.IRDOffices!.Include(x => x.Theatres).FirstOrDefaultAsync(x => x.Id == Id);

                if (IRDOffice == null)
                {
                    return "IRD Office not found";
                }

                if (IRDOffice.Theatres.Count != 0)
                {
                    return "Threater is present for this IRD Office";
                }
               

                _context.IRDOffices!.Remove(IRDOffice!);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<List<IRDOfficeVM>> GetAllIRDOfficeAsync()
        {
            try
            {

                var result = await _context.IRDOffices.Include(x=>x.Theatres).Select(x => new IRDOfficeVM
                {
                    Id = x.Id,
                    BankName = x.BankName,  
                    BankBranchName = x.BankBranchName,
                    AccountOperatingOffice = x.AccountOperatingOffice,
                    Email = x.Email,
                    Location = x.Location,
                    CreatedBy = x.CreatedBy,
                    Phone = x.Phone,
                    Theatres = x.Theatres,
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<IRDOfficeVM> GetIRDOfficeByIdAsync(int Id)
        {
            var result = await _context.IRDOffices.FindAsync(Id);
            return _mapper.Map<IRDOfficeVM>(result);//here book model is a target class and the book is the source
        }

        public async Task<string> UpdateIRDOfficeAsync(int Id, IRDOfficeVM ItemModel)
        {
            try
            {
                if (_context.IRDOffices.Any(x => x.AccountOperatingOffice == ItemModel.AccountOperatingOffice && x.Id != Id))
                {
                    return "IRD Office Name Already Exists";
                }
                var result = await _context.IRDOffices.FindAsync(Id);
                if (result == null)
                {
                    return "IRD Office Not Found";
                }
                result.AccountOperatingOffice = ItemModel.AccountOperatingOffice;
                result.PANNumber = ItemModel.PANNumber;
                result.Location = ItemModel.Location;
                result.Email = ItemModel.Email;
                result.Phone = ItemModel.Phone;
                result.BankName = ItemModel.BankName;
                result.BankBranchName = ItemModel.BankBranchName;
                result.AccountNo = ItemModel.AccountNo;
                result.LastUpdatedBy = ItemModel.LastUpdatedBy;
                result.LastUpdatedAt = DateTime.Now;

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
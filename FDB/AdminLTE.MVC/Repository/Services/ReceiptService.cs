using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdminLTE.MVC.Data;
using AdminLTE.MVC.DTO;
using AdminLTE.MVC.Enums;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.ViewModels;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Repository.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiptService> _logger;
        public ReceiptService(ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                ILogger<ReceiptService> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> AddReceiptAsync(ReceiptVM ItemModel)
        {
            try
            {
                if( _context.ReceiptUploads.Any(x => x.ChequeDraftNo == ItemModel.ChequeDraftNo))
                {
                    return "Cheque Draft Number Cannot be Same";
                }
                string commaSeparatedMonths = string.Join(", ", ItemModel.NepaliMonth);


                // Create a new theater model from the view model
                var Model = new ReceiptUpload()
                {
                    BankName = ItemModel.BankName,
                    BankBranchName = ItemModel.BankBranchName,
                    VoucherNo = ItemModel.VoucherNo,
                    AccountNo = ItemModel.AccountNo,
                    AccountOperatingOffice = ItemModel.AccountOperatingOffice,
                    ChequeDraftNo = ItemModel.ChequeDraftNo,
                    Amount = ItemModel.Amount,
                    AmountInWord = ItemModel.AmountInWord,
                    Vapata = ItemModel.Vapata,
                    DepositoryOfficeName = ItemModel.DepositoryOfficeName,
                    DepositoryOfficeCode = ItemModel.DepositoryOfficeCode,
                    DepositorName = ItemModel.DepositorName,
                    DepositorPhone = ItemModel.DepositorPhone,
                    TransactionDate = ItemModel.TransactionDate,
                    TransactionDateNep = ItemModel.TransactionDateNep,
                    PanNo = ItemModel.PanNo,
                    Status = ItemModel.Status,
                    Remarks = ItemModel.Remarks,
                    CreatedBy = ItemModel.CreatedBy,
                    CreatedAt = DateTime.Now,
                    TheatreId = ItemModel.TheatreId,
                    GrossCollection = ItemModel.GrossCollection,
                    FineAmount = ItemModel.FineAmount,
                    NepaliMonth = commaSeparatedMonths,
                    AmountWithTax = ItemModel.Amount * (15m / 100m),
                };
                if (ItemModel.VoucherImgFront != null)
                {
                    Model.VoucherImgFront = UploadImage(ItemModel.VoucherImgFront);
                } 
                if (ItemModel.FineVoucherReceiptFile != null)
                {
                    Model.FineVoucherReceipt = UploadImage(ItemModel.FineVoucherReceiptFile);
                }
                if (ItemModel.MonthlySalesReportFile != null)
                {
                    Model.MonthlySalesReport = UploadImage(ItemModel.MonthlySalesReportFile);
                }
                var GetTheaterName = await _context.Theatres.Where(x => x.TheatreId == ItemModel.TheatreId).Select(x => x.Name).FirstOrDefaultAsync();
                if(ItemModel.Status == StatusEnum.Pending)
                {
                    var notify =new NotificationAlert(){
                        ToShow = UserRoles.SuperAdmin,
                        DateTime = DateTime.Now,
                        Title ="Pending Receipt",
                        Message = "There is a new Pending Receipt By Theater"+GetTheaterName,
                        IsRead = false,
                    };
                    await _context.AddAsync(notify);
                }

                // Add the new theater to the context
                await _context.AddAsync(Model);
                await _context.SaveChangesAsync();

                // Return success message
                return "Success";
            }
            catch (Exception ex) // Catch the exception and define the variable
            {
                // Log the exception (you can log it to a file, database, etc. as per your logging strategy)
                // Example: _logger.LogError(ex, "An error occurred while adding a theater");

                // Return error message
                                    _logger.LogError(ex, "An error occurred while processing the request.");

                return "Error";
            }
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ReceiptUpload");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using(FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }


        public async Task<string> DeleteReceiptAsync(int Id)
        {
            try
            {
                var result = await _context.ReceiptUploads!.FirstOrDefaultAsync(x => x.Id == Id);

                // var result = new ReceiptUpload() 
                // {
                //     Id= Id 
                // };
                _context.ReceiptUploads.Remove(result);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                throw;
            }
        }

        public async Task<List<ReceiptVM>> GetAllReceiptAsync(int? theaterId)
        {
            var listOfReceipts = new List<ReceiptUpload>();
            try
            {   
                if(theaterId == null || theaterId == 0)
                {
                    listOfReceipts = await _context.ReceiptUploads!.Include(x => x.Theatre).ToListAsync();
                }
                else
                {
                    listOfReceipts = await _context.ReceiptUploads!.Include(x => x.Theatre).Where(x => x.TheatreId == theaterId).ToListAsync();
                }           
                var result = listOfReceipts.Select(x=>new ReceiptVM{
                    Id = x.Id,
                    BankName = x.BankName,
                    BankBranchName = x.BankBranchName,
                    VoucherNo = x.VoucherNo,
                    AccountNo = x.AccountNo,
                    AccountOperatingOffice = x.AccountOperatingOffice,
                    ChequeDraftNo = x.ChequeDraftNo,
                    Amount = x.Amount,
                    AmountInWord = x.AmountInWord,
                    Vapata = x.Vapata,
                    DepositoryOfficeName = x.DepositoryOfficeName,
                    DepositoryOfficeCode = x.DepositoryOfficeCode,
                    DepositorName = x.DepositorName,
                    DepositorPhone = x.DepositorPhone,
                    TransactionDate = x.TransactionDate,
                    TransactionDateNep = x.TransactionDateNep,
                    PanNo = x.PanNo,
                    Status = x.Status,
                    Remarks = x.Remarks,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    TheatreId = x.TheatreId,
                    TheatreName = x.Theatre?.Name,
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                    _logger.LogError(ex, "An error occurred while processing the request.");

                throw ;
            }
        }

        public async Task<ReceiptVM> GetReceiptByIdAsync(int Id)
        {
            var record = await _context.ReceiptUploads
                               .Include(x => x.Theatre)
                               .Where(x => x.Id == Id)
                               .Select(x => new {
                                   x.Id,
                                   x.BankName,
                                   x.BankBranchName,
                                   x.VoucherNo,
                                   x.AccountNo,
                                   x.AccountOperatingOffice,
                                   x.ChequeDraftNo,
                                   x.Amount,
                                   x.AmountInWord,
                                   x.Vapata,
                                   x.DepositoryOfficeName,
                                   x.DepositoryOfficeCode,
                                   x.DepositorName,
                                   x.DepositorPhone,
                                   x.TransactionDate,
                                   x.TransactionDateNep,
                                   x.PanNo,
                                   x.VoucherImgFront,
                                   x.VoucherImgBack,
                                   x.FineVoucherReceipt,
                                   x.MonthlySalesReport,
                                   x.GrossCollection,
                                   x.FineAmount,
                                   x.SignatureUrl,
                                   x.Remarks,
                                   x.Status,
                                   x.CreatedBy,
                                   x.CreatedAt,
                                   x.LastUpdatedAt,
                                   x.LastUpdatedBy,
                                   x.TheatreId,
                                   x.NepaliMonth,
                               })
                               .FirstOrDefaultAsync();
            var receiptVM = new ReceiptVM()
            {
                Id = record.Id,
                BankName = record.BankName,
                BankBranchName = record.BankBranchName,
                VoucherNo = record.VoucherNo,
                AccountNo = record.AccountNo,
                AccountOperatingOffice = record.AccountOperatingOffice,
                ChequeDraftNo = record.ChequeDraftNo,
                Amount = record.Amount,
                AmountInWord = record.AmountInWord,
                Vapata = record.Vapata,
                DepositoryOfficeName = record.DepositoryOfficeName,
                DepositoryOfficeCode = record.DepositoryOfficeCode,
                DepositorName = record.DepositorName,
                DepositorPhone = record.DepositorPhone,
                TransactionDate = record.TransactionDate,
                TransactionDateNep = record.TransactionDateNep,
                PanNo = record.PanNo,
                VoucherImgFrontUrl = record.VoucherImgFront,
                VoucherImgBackUrl = record.VoucherImgBack,
                FineVoucherReceiptUrl = record.FineVoucherReceipt,
                MonthlySalesReportUrl = record.MonthlySalesReport,
                GrossCollection = record.GrossCollection,
                FineAmount = record.FineAmount,
                SignatureUrl = record.SignatureUrl,
                Remarks = record.Remarks,
                Status = record.Status,
                CreatedBy = record.CreatedBy,
                CreatedAt = record.CreatedAt,
                LastUpdatedAt = record.LastUpdatedAt,
                LastUpdatedBy = record.LastUpdatedBy,
                TheatreId = record.TheatreId,
                NepaliMonth = record.NepaliMonth?.Split(", ").ToList(),
            };

            return receiptVM;
        }

        public async Task<string> UpdateReceiptAsync(int Id, ReceiptVM ItemModel)
        {
            try
            {
                var result = await _context.ReceiptUploads.FindAsync(Id);
                if (result == null)
                {
                    return "Receipt Not Found";
                }
                string commaSeparatedMonths = string.Join(", ", ItemModel.NepaliMonth);

                result.BankName = ItemModel.BankName;
                result.BankBranchName = ItemModel.BankBranchName;
                result.VoucherNo = ItemModel.VoucherNo;
                result.AccountNo = ItemModel.AccountNo;
                result.AccountOperatingOffice = ItemModel.AccountOperatingOffice;
                result.ChequeDraftNo = ItemModel.ChequeDraftNo;
                result.Amount = ItemModel.Amount;
                result.AmountWithTax =ItemModel.Amount * (15m / 100m);
                result.AmountInWord = ItemModel.AmountInWord;
                result.Vapata = ItemModel.Vapata;
                result.DepositoryOfficeName = ItemModel.DepositoryOfficeName;
                result.DepositoryOfficeCode = ItemModel.DepositoryOfficeCode;
                result.DepositorName = ItemModel.DepositorName;
                result.DepositorPhone = ItemModel.DepositorPhone;
                result.TransactionDate = ItemModel.TransactionDate;
                result.TransactionDateNep = ItemModel.TransactionDateNep;
                result.PanNo = ItemModel.PanNo;
                result.Remarks = ItemModel.Remarks;
                result.TheatreId = ItemModel.TheatreId;
                result.Status = ItemModel.Status;
                result.VoucherImgFront = ItemModel.VoucherImgFront != null? UploadImage(ItemModel.VoucherImgFront):result.VoucherImgFront;
                result.FineVoucherReceipt = ItemModel.FineVoucherReceiptFile != null ? UploadImage(ItemModel.FineVoucherReceiptFile) : result.FineVoucherReceipt;
                result.MonthlySalesReport = ItemModel.MonthlySalesReportFile != null ? UploadImage(ItemModel.MonthlySalesReportFile) : result.MonthlySalesReport;
                result.GrossCollection = ItemModel.GrossCollection;
                result.FineAmount = ItemModel.FineAmount;
                result.NepaliMonth = commaSeparatedMonths;
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

        public async Task<List<DashboardTableDataVm>> GetAllReceiptGroupByAsync(int? theaterId)
        {
            try
            {
                IQueryable<ReceiptUpload> query = _context.ReceiptUploads!.Include(x => x.Theatre);

                if (theaterId != null && theaterId != 0)
                {
                    query = query.Where(x => x.TheatreId == theaterId);
                }

                //var groupedReceipts = await query
                //    .GroupBy(x => x.Theatre)
                //    .Select(g => new DashboardTableDataVm
                //    {
                //        dateTime = g.First().TransactionDate,
                //        TheatreName = g.Key.Name,
                //        TotalCollection = g.Sum(x => x.Amount),
                //        Tax =0,
                //        PendingAmount = 0
                //    })
                //    .ToListAsync();
                var groupedReceipts = await query
                    .GroupBy(x => x.Theatre)
                    .Select(g => new DashboardTableDataVm
                    {
                        dateTime = g.First().TransactionDate,
                        TheatreName = g.Key.Name,
                        TotalCollection = g.Sum(x => x.Amount),
                        GrossCollection = g.Sum(x=> x.GrossCollection),
                        PendingAmount = 0
                    })
                    .ToListAsync();

                return groupedReceipts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }

        public async Task<ReceiptSummaryDTO> GetTodaysAsync(int? theaterId)
        {
            try
            {
                IQueryable<ReceiptUpload> query = _context.ReceiptUploads!.Include(x => x.Theatre);

                if (theaterId != null && theaterId != 0)
                {
                    query = query.Where(x => x.TheatreId == theaterId);
                }

                var today = DateTime.Now.Date;

                var result = await query
                    .Where(x => x.CreatedAt.Date == today)
                    .GroupBy(x => true) // Group by a constant value to get a single result
                    .Select(g => new ReceiptSummaryDTO
                    {
                        TotalAmount = g.Sum(x => x.Amount),
                        TotalTaxAmount = g.Sum(x => x.AmountWithTax)
                    })
                    .FirstOrDefaultAsync();

                return result ?? new ReceiptSummaryDTO { TotalAmount = 0, TotalTaxAmount = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }


        public async Task<string> ChangeStatus(List<int> Ids, StatusEnum Status)
        {
            try
            {
                foreach (var id in Ids)
                {
                    var ExistingReceipt = await _context.ReceiptUploads.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (ExistingReceipt == null)       
                    {
                        return "Failed To Update";
                    }    
                    ExistingReceipt.Status = Status; 
                    await _context.SaveChangesAsync();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;           
            }
        }

        public async Task<ChartDataDTO> GetChartData(int? theaterId)
        {
            var ChartData = new ChartDataDTO
            {
                Days = new List<string>(),
                DaysData = new List<decimal>(),
                Months = new List<string>(),
                MonthsData = new List<decimal>()
            };

            IQueryable<ReceiptUpload> query = _context.ReceiptUploads!.Include(x => x.Theatre);

            if (theaterId != null && theaterId != 0)
            {
                query = query.Where(x => x.TheatreId == theaterId);
            }

            // Calculate the start of the current week (Sunday)
            DateTime today = DateTime.Now.Date;
            int daysUntilLastSunday = (int)today.DayOfWeek;
            DateTime startOfWeek = today.AddDays(-daysUntilLastSunday);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            // Query the data for the current week
            var result = await query.Where(x => x.CreatedAt.Date >= startOfWeek && x.CreatedAt.Date < endOfWeek).ToListAsync();

            // Populate the days of the week
            for (DateTime date = startOfWeek; date < endOfWeek; date = date.AddDays(1))
            {
                ChartData.Days.Add(date.ToString("dddd"));
                var dailyAmount = result.Where(x => x.CreatedAt.Date == date).Sum(x => x.Amount);
                ChartData.DaysData.Add(dailyAmount);
            }

            return ChartData;
        }

    }
}
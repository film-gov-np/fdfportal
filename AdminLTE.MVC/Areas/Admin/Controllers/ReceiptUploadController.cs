using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Enums;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.Repository.Services;
using AdminLTE.MVC.Utilites;
using AdminLTE.MVC.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReceiptUploadController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReceiptUploadController> _logger;
        public INotyfService _notification { get; }
        private readonly IReceiptService _receiptService;
        private readonly ITheaterService _theaterService;
        private readonly IIRDOfficeService _iRDOfficeService;

        public ReceiptUploadController(ILogger<ReceiptUploadController> logger, INotyfService notyfService,
            IReceiptService receiptService, UserManager<ApplicationUser> userManager, ITheaterService theaterService,
            IIRDOfficeService iRDOfficeService
)
        {
            _notification = notyfService;
            _logger = logger;
            _receiptService = receiptService;
            _userManager = userManager;
            _theaterService = theaterService;
            _iRDOfficeService = iRDOfficeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var result = await _receiptService.GetAllReceiptAsync(currentUser.TheatreId);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> ExportReceipt()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }

            var result = await _receiptService.GetAllReceiptAsync(currentUser.TheatreId);

            var exportData = result.Select(x => new ReceiptVM
            {
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
              //  PanNo = x.PanNo,
                Remarks = x.Remarks,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                TheatreName = x.TheatreName
            }).ToList();

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Context.RegisterClassMap<ReceiptExportModelMap>();
                csvWriter.WriteRecords(exportData);
            }

            return File(memoryStream.ToArray(), "text/csv", "Receipt.csv");
        }



        [HttpGet]

        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);
            if (loggedInUserRole == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin)
            {
                var theatres = await _theaterService.GetAllTheaterAsync();
                ViewBag.Theatres = theatres.Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.PANNumber,
                    t.IRDOffice.BankName,
                    t.IRDOffice.BankBranchName,
                    t.IRDOffice.AccountNo,
                    t.IRDOffice.AccountOperatingOffice,
                }).ToList();
            }

            else
            {
                if (currentUser.TheatreId.HasValue)
                {
                    var theater = await _theaterService.GetTheaterByIdAsync(currentUser.TheatreId.Value);
                    return View(new ReceiptVM 
                    { PanNo = theater.PANNumber,
                      AccountOperatingOffice = theater.IRDOffice.AccountOperatingOffice,
                      BankBranchName = theater.IRDOffice.BankBranchName,
                      BankName = theater.IRDOffice.BankName,
                      AccountNo = theater.IRDOffice.AccountNo,
                    });
                }
            }
            return View(new ReceiptVM());

        }

        [HttpPost]
        public async Task<IActionResult> Create(ReceiptVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            vm.CreatedBy = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            if (vm.TheatreId == null)
            {
                vm.TheatreId = currentUser.TheatreId;
            }
            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);
            if (loggedInUserRole == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin)
            {
                vm.Status = StatusEnum.Approved;
            }
            else if (loggedInUserRole[0] == ApplicationUserRoles.FDFCollector)
            {
                vm.Status = StatusEnum.Recommend;
            }
            else
            {
                vm.Status = StatusEnum.Pending;
            }

            var result = await _receiptService.AddReceiptAsync(vm);
            if (result != "Success")
            {
                _notification.Error(result);
                return View(vm);
            }
            else
            {
                _notification.Success(result);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
            }

            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);
            if (loggedInUserRole == null)
            {
                _notification.Error("User Role Not Found");
                return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
            }
            if (loggedInUserRole[0] != ApplicationUserRoles.SuperAdmin)
            {
                _notification.Error("Not Authorize");
                return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
            }

            var result = await _receiptService.DeleteReceiptAsync(Id);
            if (result == "Success")
            {
                _notification.Success("Receipt deleted successfully");
                // return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
                return Json(new { status = "Success", message = "Receipt deleted successfully." });
            }
            _notification.Error(result);
            return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);
            if (loggedInUserRole == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin)
            {
                var theatres = await _theaterService.GetAllTheaterAsync();
                ViewBag.Theatres = theatres.Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.PANNumber,
                    t.IRDOffice.BankName,
                    t.IRDOffice.BankBranchName,
                    t.IRDOffice.AccountNo,
                    t.IRDOffice.AccountOperatingOffice,
                }).ToList();
            }
            return View(await _receiptService.GetReceiptByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _receiptService.GetReceiptByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ReceiptVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            vm.LastUpdatedBy = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            var result = await _receiptService.UpdateReceiptAsync(vm.Id, vm);
            if (result == "Success")
            {
                _notification.Success("Receipt updated successfully");
                return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
            }
            _notification.Error(result);
            return RedirectToAction("Index", "ReceiptUpload", new { area = "Admin" });
        }

        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusModel model)
        {
            var result = "";
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return Json("Error");
            }

            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);
            if (loggedInUserRole == null)
            {
                _notification.Error("User Role Not Found");
                return Json("Error");
            }
            if (loggedInUserRole[0] != ApplicationUserRoles.SuperAdmin && loggedInUserRole[0] != ApplicationUserRoles.FDFCollector)
            {
                _notification.Error("Not Authorize");
                return Json("Error");
            }
            if(loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin && model.Approve == true)
            {
                result = await _receiptService.ChangeStatus(model.Ids, StatusEnum.Approved);
            }
            
            else if(loggedInUserRole[0] == ApplicationUserRoles.FDFCollector && model.Approve == true)
            {
                result = await _receiptService.ChangeStatus(model.Ids, StatusEnum.Recommend);
            }
            else 
            {
                result = await _receiptService.ChangeStatus(model.Ids, StatusEnum.Rejected);
            }
            if (result == "Success")
            {
                _notification.Success("Receipt updated successfully");
                return Json("Success");
            }
            _notification.Error(result);
            return Json("Success");
        }
    }
}

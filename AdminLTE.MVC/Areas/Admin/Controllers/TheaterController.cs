using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.Repository.Services;
using AdminLTE.MVC.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Super Admin")]
    public class TheaterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<TheaterController> _logger;
        public INotyfService _notification { get; }

        private readonly ITheaterService _theaterService;
        private readonly IBrandService _brandService;
        private readonly IIRDOfficeService _iRDOfficeService;

        public TheaterController(ILogger<TheaterController> logger, INotyfService notyfService,ITheaterService theaterService, IBrandService brandService,IIRDOfficeService iRDOfficeService, UserManager<ApplicationUser> userManager)
        {
            _notification = notyfService;
            _logger = logger;
            _theaterService = theaterService;
            _userManager = userManager;
            _brandService = brandService;
            _iRDOfficeService = iRDOfficeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _theaterService.GetAllTheaterAsync();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetFBDTheaterData()
        {
            int theaterId = await _theaterService.GetFBDTheaterId();
            ViewBag.TheatreId = theaterId;

            var theaterData = await _theaterService.GetTheaterDataFromApiAsync(theaterId);
            return View(theaterData);
        }

        [HttpGet]

        public async Task<IActionResult> TheaterAdd()
        {
            var brands = await _brandService.GetAllBrandAsync();
            

            ViewBag.BrandMVCs = new SelectList(brands, "BrandCode", "BrandName");
            var IRDOffice = await _iRDOfficeService.GetAllIRDOfficeAsync();
          
            ViewBag.IRDOffice = new SelectList(IRDOffice, "Id", "AccountOperatingOffice");
            
            return View(new TheaterVM());
        }

        [HttpPost]
        public async Task<IActionResult> TheaterAdd(TheaterVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
                vm.CreatedBy = fullName;
            }

            var result = await _theaterService.AddTheaterAsync(vm);
            if (result != "Success")
            {
                _notification.Error(result);
                var brands = await _brandService.GetAllBrandAsync();
                ViewBag.BrandMVCs = new SelectList(brands, "BrandCode", "BrandName");
                var ird = await _iRDOfficeService.GetAllIRDOfficeAsync();
                ViewBag.IRDOffice = new SelectList(ird, "IRDOfficeId", "AccountOperatingOffice");
                return View(vm);
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TheaterFBDAdd(List<TheaterResponseModel> vm)
        {
            var fullName = "";
            var result = "";
            if (!ModelState.IsValid) { return View(vm); }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            }

            foreach (var item in vm)
            {
                var TheaterModel = new TheaterVM
                {
                    TheatreCode = item.TheaterCode,
                    Name = item.Name,
                    Description = "",
                    Location = item.Address,
                    Phone = "",
                    CreatedBy = fullName,
                    TheaterId = item.TheaterId,
                    PANNumber = item.Pannumber,
                    RegNumber = item.RegNumber,
                    Email = item.Email,
                    Vatnumber = item.Vatnumber,
                    BrandCode = item.BrandCode,
                    IRDOfficeId = item.IRDOfficeId,

                };

                result = await _theaterService.AddTheaterAsync(TheaterModel);
            }
            if (result != "Success")
            {
                _notification.Error(result);
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _theaterService.DeleteTheaterAsync(Id);
            if (result == "Success")
            {
                _notification.Success(result);
                return Json(new { status = "Success", message = "Theater deleted successfully." });
            }
            _notification.Error(result);
            return Json(new { status = "Error", message = result });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brands = await _brandService.GetAllBrandAsync();

            ViewBag.BrandMVCs = new SelectList(brands, "BrandCode", "BrandName");
            var IRDOffice = await _iRDOfficeService.GetAllIRDOfficeAsync();

            ViewBag.IRDOffice = new SelectList(IRDOffice, "Id", "AccountOperatingOffice");
            return View( await _theaterService.GetTheaterByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TheaterVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
                vm.LastUpdatedBy = fullName;
            }
            var result = await _theaterService.UpdateTheaterAsync(vm.Id, vm);
            if (result == "Success")
            {
                _notification.Success("Theater updated successfully");
                var brands = await _brandService.GetAllBrandAsync();
                ViewBag.BrandMVCs = new SelectList(brands, "BrandCode", "BrandName");

                var IRDOffice = await _iRDOfficeService.GetAllIRDOfficeAsync();
                ViewBag.IRDOffice = new SelectList(IRDOffice, "Id", "AccountOperatingOffice");
                return View(vm);
            }
            _notification.Error(result);
            return RedirectToAction("Index", "Theater", new { area = "Admin" });

        }
    }
}
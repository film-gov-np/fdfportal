using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.Repository.Services;
using AdminLTE.MVC.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<BrandController> _logger;
        public INotyfService _notification { get; }

        private readonly IBrandService _brandService;

        public BrandController(ILogger<BrandController> logger, INotyfService notyfService, IBrandService brandService, UserManager<ApplicationUser> userManager)
        {
            _notification = notyfService;
            _logger = logger;
            _brandService = brandService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _brandService.GetAllBrandAsync();
            return View(result);
        }
        [HttpGet]
        public IActionResult BrandAdd() 
        {
            
            return View(new BrandVM());
        }
        [HttpPost]
        public async Task<IActionResult> BrandAdd(BrandVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var fullName = $"{currentUser.FirstName} {currentUser.MiddleName} {currentUser.LastName}".Trim();
                vm.AddedBy = fullName;
            }
            var result = await _brandService.AddBrandAsync(vm);
            if (result != "Success")
            {
                _notification.Error(result);
                return View(vm); // Return the view with the current model if there's an error
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetFBDBrandData()
        {
            int BrandId = await _brandService.GetFBDBrandById();
            ViewBag.BrandId = BrandId;

            var theaterData = await _brandService.GetBrandDataFromApiAsync(BrandId);
            return View(theaterData);
        }
        [HttpPost]
        public async Task<IActionResult> BrandFBDAdd(List<BrandResponseModel> vm)
        {
            var fullName = "";
            var result ="";
            if (!ModelState.IsValid) { return View(vm); }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            }

            foreach (var item in vm)
            {
                var Model = new BrandVM
                {
                    BrandCode = item.BrandCode,
                    BrandName = item.BrandName,
                    Email = item.Email,
                    ApiUsername = item.ApiUsername,
                    ApiPassword = item.ApiPassword,
                    StatusValue = item.StatusValue,
                    IsTest = item.IsTest,
                    IsModified = item.IsModified,
                    BrandID   = item.BrandId,                  
                    AddedBy = fullName,
                };

                result = await _brandService.AddBrandAsync(Model);
            }
            if (result != "Success")
            {
                _notification.Error(result);
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
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
                _notification.Error("User Role Not Found");
                return RedirectToAction("Index");
            }
            var result = await _brandService.DeleteBrandAsync(Id);
            if (result == "Success")
            {
                _notification.Success("Brand deleted successfully");
                return Json(new { status = "Success", message = "Brand deleted successfully." });
                //  return RedirectToAction("Index", "Brand", new { area = "Admin" });
            }
            _notification.Error(result);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            return View(await _brandService.GetBrandByIdAsync(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BrandVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            vm.UpdatedBy = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            var result = await _brandService.UpdateBrandAsync(vm.Id, vm);
            if (result == "Success")
            {
                _notification.Success("Brand updated successfully");
                return RedirectToAction("Index", "Brand", new { area = "Admin" });
            }
            _notification.Error(result);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

    }
}

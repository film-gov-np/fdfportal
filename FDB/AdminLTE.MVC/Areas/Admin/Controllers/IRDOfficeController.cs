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
    public class IRDOfficeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<IRDOfficeController> _logger;
        public INotyfService _notification { get; }

        private readonly IIRDOfficeService _iRDOfficeService;

        public IRDOfficeController(ILogger<IRDOfficeController> logger, INotyfService notyfService,IIRDOfficeService iRDOfficeService, UserManager<ApplicationUser> userManager)
        {
            _notification = notyfService;
            _logger = logger;
            _iRDOfficeService = iRDOfficeService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _iRDOfficeService.GetAllIRDOfficeAsync();
          
            return View(result);
        }

        [HttpGet]

        public IActionResult IRDOfficeAdd()
        {

            return View(new IRDOfficeVM());
        }

        [HttpPost]
        public async Task<IActionResult> IRDOfficeAdd(IRDOfficeVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
                vm.CreatedBy = fullName;
            }

            var result = await _iRDOfficeService.AddIRDOfficeAsync(vm);
            if (result != "Success")
            {
                _notification.Error(result);
                return View(vm);
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _iRDOfficeService.DeleteIRDOfficeAsync(Id);
            if (result == "Success")
            {
                _notification.Success(result);
                return Json(new { status = "Success", message = "IRD Office deleted successfully." });
            }
            _notification.Error(result);
            return Json(new { status = "Error", message = result });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View( await _iRDOfficeService.GetIRDOfficeByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IRDOfficeVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser =await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var fullName = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
                vm.LastUpdatedBy = fullName;
            }
            var result = await _iRDOfficeService.UpdateIRDOfficeAsync(vm.Id,vm);
            if (result == "Success")
            {
                _notification.Success("IRD Office updated successfully");
                return View(vm);
            }
            _notification.Error(result);
            return RedirectToAction("Index", "IRDOffice", new { area = "Admin" });

        }
    }
}
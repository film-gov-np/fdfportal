using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DashboardController> _logger;
        public INotyfService _notification { get; }
        private readonly IReceiptService _receiptService;
        private readonly ITheaterService _theaterService;

        public DashboardController(ILogger<DashboardController> logger, INotyfService notyfService,IReceiptService receiptService,UserManager<ApplicationUser> userManager, ITheaterService theaterService
)
        {
            _notification = notyfService;
            _logger = logger;
            _receiptService = receiptService;
            _userManager = userManager;
            _theaterService = theaterService;
        }

        public async Task< IActionResult >Index()
        {
            var currentUser =await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var result = await _receiptService.GetAllReceiptGroupByAsync(currentUser.TheatreId);
            var DailyCollect = await _receiptService.GetTodaysAsync(currentUser.TheatreId);
            ViewBag.DailyCollection = DailyCollect.TotalAmount;
            ViewBag.DailyTaxCollection = DailyCollect.TotalTaxAmount;
            return View(result);
        
        }
        public IActionResult GetTableData()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var currentUser =await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var Result = await _receiptService.GetChartData(currentUser.TheatreId);
            return Json( Result);
        }
    }
}
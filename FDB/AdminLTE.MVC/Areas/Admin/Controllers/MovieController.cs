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
    public class MovieController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<MovieController> _logger;
        public INotyfService _notification { get; }

        private readonly IMovieService _movieService;

        public MovieController(ILogger<MovieController> logger, INotyfService notyfService, IMovieService movieService, UserManager<ApplicationUser> userManager)
        {
            _notification = notyfService;
            _logger = logger;
            _movieService = movieService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var result = await _movieService.GetAllMovieAsync();
            return View(result);
        }

        [HttpGet]
        public IActionResult MovieAdd()
        {
            return View(new MovieVM());
        }


        [HttpPost]
        public async Task<IActionResult> MovieAdd(MovieVM vm)
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
            var result = await _movieService.AddMovieAsync(vm);
            if (result != "Success")
            {
                _notification.Error(result);
                return View(vm); // Return the view with the current model if there's an error
            }
            _notification.Success("Success");
            return RedirectToAction("Index");
        }
         
        [HttpGet]
        public async Task<IActionResult> GetFBDMovieData()
        {
            int MovieId = await _movieService.GetFBDMovieById();
            ViewBag.MovieId = MovieId;

            var theaterData = await _movieService.GetMovieDataFromApiAsync(MovieId);
            return View(theaterData);
        }
        [HttpPost]
        public async Task<IActionResult> MovieFBDAdd(List<MovieResponseModel> vm)
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
                var Model = new MovieVM
                {
                    MovieID = item.MovieId,
                    MovieCode = item.MovieCode,
                    Name = item.Name,
                    GradeCode = item.GradeCode,
                    ProductionHouseCode = item.ProductionHouseCode,
                    LanguageCode = item.LanguageCode,
                    ProductionType = item.ProductionType,
                    ReleaseDate = item.ReleaseDate,
                    StatusValue = item.StatusValue,
                    Slug = item.Slug,
                    IsModified = item.IsModified,
                    IsDeleted = item.IsDeleted,
                    CanSpecifyMovieName = item.CanSpecifyMovieName,
                    SendMovieCodesToExhibitorEmail = item.SendMovieCodesToExhibitorEmail,
                    SendNotificationToProducers = item.SendNotificationToDistributors,
                    SendNotificationToDistributors = item.SendNotificationToProducers,
                    AddedBy = fullName,
                };

                result = await _movieService.AddMovieAsync(Model);
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
            var result = await _movieService.DeleteMovieAsync(Id);
            if (result == "Success")
            {
                _notification.Success(result);
                return Json(result);
                // return RedirectToAction("Index", "Movie", new { area = "Admin" });
            }
            _notification.Error(result);
            return RedirectToAction("Index", "Movie", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            return View(await _movieService.GetMovieByIdAsync(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MovieVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            vm.UpdatedBy = $"{currentUser.FirstName} {(string.IsNullOrWhiteSpace(currentUser.MiddleName) ? "" : currentUser.MiddleName + " ")}{currentUser.LastName}";
            var result = await _movieService.UpdateMovieAsync(vm.Id, vm);
            if (result == "Success")
            {
                _notification.Success("Movie updated successfully");
                return RedirectToAction("Index", "Movie", new { area = "Admin" });
               
            }
            _notification.Error(result);
            return RedirectToAction("Index", "Movie", new { area = "Admin" });
        }



    }




}

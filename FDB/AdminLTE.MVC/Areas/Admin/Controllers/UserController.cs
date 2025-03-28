using AdminLTE.MVC.Models;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.Utilites;
using AdminLTE.MVC.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITheaterService _theaterService;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager, INotyfService notyfService ,ITheaterService theaterService,IWebHostEnvironment webHostEnvironment,ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notyfService;
            _theaterService = theaterService;   
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [Authorize(Roles = "Super Admin, Theatre Admin, FDF Collector")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);
            var users = new List<ApplicationUser>();
            if (loggedInUser == null)
            {
                _notification.Error("No User Login");
            }
            if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin || loggedInUserRole[0] == ApplicationUserRoles.FDFCollector)
            {
                users = await _userManager.Users.Include(x => x.Theatre).ToListAsync();
            }
            else
            {
                users = await _userManager.Users.Include(x => x.Theatre).Where(s => s.TheatreId == loggedInUser.TheatreId).ToListAsync();
            }
            var vm = users. Select(x => new UserVM()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                TheaterName = x.Theatre != null ? x.Theatre.Name:"",
            }).ToList();
            //assinging role
            foreach (var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Role = role.FirstOrDefault();
            }

            return View(vm);
        }

        [Authorize(Roles = "Super Admin, Theatre Admin,FDF Collector")]
        [HttpGet]
        public async Task<IActionResult> ExportUser()
        {
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            if (loggedInUser == null)
            {
                _notification.Error("No User Login");
                return RedirectToAction("Index", "Home"); // Redirect to a suitable action
            }

            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser);
            var users = new List<ApplicationUser>();

            if (loggedInUserRole.Contains(ApplicationUserRoles.SuperAdmin))
            {
                users = await _userManager.Users.Include(x => x.Theatre).ToListAsync();
            }
            else
            {
                users = await _userManager.Users.Include(x => x.Theatre).Where(s => s.TheatreId == loggedInUser.TheatreId).ToListAsync();
            }

            var userViewModels = users.Select(x => new ExportUser()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address,
                TheaterName = x.Theatre != null ? x.Theatre.Name : "",
            }).ToList();

            // Assigning roles
            foreach (var userVm in userViewModels)
            {
                var singleUser = await _userManager.FindByIdAsync(userVm.Id);
                var roles = await _userManager.GetRolesAsync(singleUser);
                userVm.Role = roles.FirstOrDefault();
            }


            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(userViewModels);
            }

            return File(memoryStream.ToArray(), "text/csv", "userExport.csv");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var users = await _userManager.Users.Include(x => x.Theatre).Where(x => x.Id == id).ToListAsync();

            var vm = users. Select(x => new UserVM()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                Address = x.Address,
                TheaterName = x.Theatre != null ? x.Theatre.Name:"",
                PictureUrl = x.PictureUrl,
                Phone = x.Phone,
            }).FirstOrDefault();
          
            var singleUser = await _userManager.FindByIdAsync(vm.Id);
            var role = await _userManager.GetRolesAsync(singleUser);
            vm.Role = role.FirstOrDefault();
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exsits");
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName
            };
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exsits");
                return View();
            }
            var vm = new UserVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                UserId = existingUser.UserId,
                FirstName=existingUser.FirstName,
                MiddleName=existingUser.MiddleName,
                LastName=existingUser.LastName,
                Email=existingUser.Email,
                TheaterId=existingUser.TheatreId,
                PictureUrl=existingUser.PictureUrl,
                Address=existingUser.Address,
                Phone=existingUser.Phone,
            };
            var role = await _userManager.GetRolesAsync(existingUser);
            vm.IsAdmin = role.FirstOrDefault() == ApplicationUserRoles.Admin ? true:false;
            var Theatre = await _theaterService.GetAllTheaterAsync();
            ViewBag.Theatres = new SelectList(Theatre,"Id","Name");
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser == null)
                {
                    _notification.Error("User Not Found");
                    return RedirectToAction("Index");
                }

                var user = await _userManager.Users.Include(x => x.Theatre).Where(x => x.Id == vm.Id).FirstOrDefaultAsync();

                if (_userManager.Users.Any(x => x.Email == vm.Email && x.Id != vm.Id))
                {
                    _notification.Error("Email Already Exists");
                    return View(vm);
                }
                if (user == null)
                {
                    _notification.Error("User Not Found");
                    return View(vm);
                }

                user.Email = vm.Email;
                user.UserId = vm.UserId;
                user.UserName = vm.UserName;
                user.FirstName = vm.FirstName;
                user.MiddleName = vm.MiddleName;
                user.LastName = vm.LastName;
                user.TheatreId = vm.TheaterId == null ? currentUser.TheatreId : vm.TheaterId;
                user.Phone = vm.Phone;
                user.Address = vm.Address;
                user.UserName = vm.Email;
                if (vm.UserImg != null)
                {
                    user.PictureUrl = UploadImage(vm.UserImg);
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Remove the user from all roles first
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);

                    // Add the user to the new role
                    if (vm.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationUserRoles.User);
                    }

                    _notification.Success("User Updated successfully");
                    return RedirectToAction("Index", "User", new { area = "Admin" });
                }

                _notification.Error("Cannot Update User at this Time");
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exist");
                return View(vm);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password reset succuful");
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }


        [Authorize(Roles = "Super Admin, Theatre Admin,FDF Collector")]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
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
            if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin)
            {
                ViewBag.Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = ApplicationUserRoles.SuperAdmin, Text = ApplicationUserRoles.SuperAdmin },
                    new SelectListItem { Value = ApplicationUserRoles.Admin, Text = ApplicationUserRoles.Admin },
                    new SelectListItem { Value = ApplicationUserRoles.User, Text = ApplicationUserRoles.User },
                    new SelectListItem { Value = ApplicationUserRoles.FDFCollector, Text = ApplicationUserRoles.FDFCollector }
                }; 
            }
            else
            {
                ViewBag.Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = ApplicationUserRoles.Admin, Text = ApplicationUserRoles.Admin },
                    new SelectListItem { Value = ApplicationUserRoles.User, Text = ApplicationUserRoles.User },
                };
            }
            var Theatre = await _theaterService.GetAllTheaterAsync();
            ViewBag.Theatres = new SelectList(Theatre,"Id","Name");
            return View(new RegisterVM());
        }

        [Authorize(Roles = "Super Admin, Theatre Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var currentUser =await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUserRole = await _userManager.GetRolesAsync(currentUser!);

            if (currentUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index");
            }
            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkUserByEmail != null)
            {
                _notification.Error("Email already exists");
                if (loggedInUserRole[0] == ApplicationUserRoles.SuperAdmin)
                {
                    ViewBag.Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = ApplicationUserRoles.SuperAdmin, Text = ApplicationUserRoles.SuperAdmin },
                    new SelectListItem { Value = ApplicationUserRoles.Admin, Text = ApplicationUserRoles.Admin },
                    new SelectListItem { Value = ApplicationUserRoles.User, Text = ApplicationUserRoles.User },
                    new SelectListItem { Value = ApplicationUserRoles.FDFCollector, Text = ApplicationUserRoles.FDFCollector }
                };
                }
                else
                {
                    ViewBag.Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = ApplicationUserRoles.Admin, Text = ApplicationUserRoles.Admin },
                    new SelectListItem { Value = ApplicationUserRoles.User, Text = ApplicationUserRoles.User },
                };
                }
                var Theatre = await _theaterService.GetAllTheaterAsync();
                ViewBag.Theatres = new SelectList(Theatre, "Id", "Name");
                return View(vm);
            }
            // var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            // if (checkUserByUsername != null)
            // {
            //     _notification.Error("Username already exists");
            //     return View(vm);
            // }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserId = vm.UserId,
                UserName = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                TheatreId = vm.TheaterId == null ? currentUser.TheatreId:vm.TheaterId,
                Phone = vm.Phone,
                IsActive = true,
                Address = vm.Address,
            };
            if(vm.Roles == "FDF Collector")
            {
                applicationUser.TheatreId = null;
            }
            if (vm.UserImg != null)
            {
                applicationUser.PictureUrl = UploadImage(vm.UserImg);
            }
            else
            {
                applicationUser.PictureUrl = "Default_user.jpg";
            }

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                //if (vm.IsAdmin)
                //{
                //    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.Admin);
                //}
                //else
                //{
                //    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.User);
                //}
                if(vm.Roles == "FDF Collector")
                {
                    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.FDFCollector);
                }
                else if(vm.Roles == "Theatre User")
                {
                    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.User);
                }
                else if(vm.Roles == "Theatre Admin")
                {
                    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.SuperAdmin);

                }
                _notification.Success("User registered successfully");
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            return View(vm);
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "UserPic");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using(FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }


        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);
            if (existingUser == null)
            {
                _notification.Error("Username does not exist");
                return View(vm);
            }
            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if (!verifyPassword)
            {
                _notification.Error("Password does not match");
                return View(vm);
            }
            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);
            _notification.Success("Login Successful");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("You are logged out successfully");
            HttpContext.Response.Headers["Refresh"] = "0;url=/Admin/User/Login";
            return RedirectToAction("Login", "User", new { area = "Admin" });
        }

        [HttpGet("AccessDenied")]

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

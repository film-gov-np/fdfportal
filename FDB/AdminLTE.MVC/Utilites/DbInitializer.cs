using AdminLTE.MVC.Data;
using AdminLTE.MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace AdminLTE.MVC.Utilites
{
    public class DbInitializer: IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(ApplicationUserRoles.SuperAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.SuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.FDFCollector)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "AsterAdmin@gmail.com",
                    Email = "AsterAdmin@gmail.com",
                    FirstName = "Aster",
                    LastName = "Admin",
                    IsActive = true,
                }, "AsterAdmin@0011").Wait();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "FDBAdmin@gmail.com",
                    Email = "FDBAdmin@gmail.com",
                    FirstName = "FDB",
                    LastName = "Admin",
                    IsActive = true,
                }, "FDBAdmin@0011").Wait();

                var appUserAster = _context.ApplicationUsers!.FirstOrDefault(x => x.Email == "AsterAdmin@gmail.com");
                var appUserFDB = _context.ApplicationUsers!.FirstOrDefault(x => x.Email == "FDBAdmin@gmail.com");
                if (appUserAster != null && appUserFDB != null)
                {
                    _userManager.AddToRoleAsync(appUserAster, ApplicationUserRoles.SuperAdmin).GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(appUserFDB, ApplicationUserRoles.SuperAdmin).GetAwaiter().GetResult();
                }
                _context.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AdminLTE.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AdminLTE.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<ReceiptUpload> ReceiptUploads { get; set; }
        public DbSet<StatusMaster> StatusMasters { get; set; }
        public DbSet<BrandMVC> BrandMVCs { get; set; }
        public DbSet<MovieMVC> MovieMVCs { get; set; }
        public DbSet<NotificationAlert> NotificationAlerts { get; set; }
        public DbSet<IRDOffice> IRDOffices { get; set; }
    }

}

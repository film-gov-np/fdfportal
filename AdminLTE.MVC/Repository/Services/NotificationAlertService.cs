using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Data;
using AdminLTE.MVC.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Repository.Services
{
    public class NotificationAlertService : INotificationAlertService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationAlertService> _logger;


        public NotificationAlertService(ApplicationDbContext context,ILogger<NotificationAlertService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<string> MarkAsRead(int Id)
        {
            try
            {
                var notificationResult  = await _context.NotificationAlerts.Where(x=>x.Id == Id).FirstOrDefaultAsync();
                if(notificationResult == null)
                {
                    return "No Notification Found";
                }
                notificationResult.IsRead = true;   
                await _context.SaveChangesAsync(); 
                return "Success"; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminLTE.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotificationAlertController : Controller
    {
        private readonly ILogger<NotificationAlertController> _logger;
        private readonly INotificationAlertService _notificationAlertService;

        public NotificationAlertController(ILogger<NotificationAlertController> logger,INotificationAlertService notificationAlertService)
        {
            _logger = logger;
            _notificationAlertService = notificationAlertService;
        }

        public async Task<string> MarkAsRead(int id)
        {
            return await _notificationAlertService.MarkAsRead(id);
        }
    }
}
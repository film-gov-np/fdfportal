using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Enums;

namespace AdminLTE.MVC.ViewModels
{
    public class NotificationAlertVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public UserRoles ToShow { get; set; }    
        public DateTime DateTime { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
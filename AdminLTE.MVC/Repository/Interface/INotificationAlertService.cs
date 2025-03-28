using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface INotificationAlertService
    {
        Task<string> MarkAsRead(int Id);

    }
}
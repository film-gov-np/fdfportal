using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.ViewModels
{
    public class DashboardTableDataVm
    {
        public DateTime dateTime{ get; set; }

        public string TheatreName { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal GrossCollection { get; set; }
        public decimal PendingAmount { get; set; }

        public decimal TodaysCollection { get; set; }   
    }
}
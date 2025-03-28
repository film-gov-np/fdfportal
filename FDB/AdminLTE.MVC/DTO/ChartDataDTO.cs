using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.DTO
{
    public class ChartDataDTO
    {
        public IList<string> Days { get; set; }
        public IList<decimal> DaysData { get;set; }
        public IList<decimal> MonthsData { get;set;}
        public IList<string> Months { get;set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.DTO
{
    public class ReceiptSummaryDTO
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
    }
}
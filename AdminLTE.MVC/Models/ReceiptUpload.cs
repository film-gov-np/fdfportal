  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Enums;

namespace AdminLTE.MVC.Models
{
    public class ReceiptUpload
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public long VoucherNo { get; set; }
        public string AccountNo { get; set; }
        public string AccountOperatingOffice { get; set; }
        public string NepaliMonth { get; set; }
        public string ChequeDraftNo { get; set; }
        public decimal Amount { get; set; }
        public decimal GrossCollection { get; set; }
        public decimal FineAmount { get; set; }
        public decimal AmountWithTax { get; set; }
        public string AmountInWord { get; set; }
        public string Vapata { get; set; }
        public string DepositoryOfficeName { get; set; }
        public long DepositoryOfficeCode { get; set; }
        public string DepositorName { get; set; }
        public string DepositorPhone { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime TransactionDateNep { get; set; }
        public string PanNo { get; set; }
        public string VoucherImgFront { get; set; }
        public string VoucherImgBack { get; set; }
        public string FineVoucherReceipt { get; set; }
        public string MonthlySalesReport { get; set; }
        public string SignatureUrl { get; set; }
        public string Remarks { get; set; }
        public StatusEnum? Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
         public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
        public int? TheatreId { get; set; }
        public Theatre Theatre { get; set; }
    }
}
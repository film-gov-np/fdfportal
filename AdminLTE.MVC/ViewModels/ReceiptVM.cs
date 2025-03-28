using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Enums;
using Microsoft.AspNetCore.Http;

namespace AdminLTE.MVC.ViewModels
{
    public class NoZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is int number)
        {
            return number != 0;
        }
        return false;
    }

}
    public class ReceiptVM
    {

        public int Id { get; set; }///
       // [Required(ErrorMessage = "Please enter the bank name.")]
        public string BankName { get; set; }//
        public string BankBranchName { get; set; }//
      //  [Required(ErrorMessage = "Please enter the Voucher Number.")]

        public long VoucherNo { get; set; }
      //  [Required(ErrorMessage = "Please enter Your Account Number.")]

        public string AccountNo { get; set; }
     //   [Required(ErrorMessage = "Please enter Account Operating Office.")]
        public string AccountOperatingOffice { get; set; } //
     //   [Required(ErrorMessage = "Please enter the Cheque Draft no.")]
        public string ChequeDraftNo { get; set; }
      //  [Required (ErrorMessage = "Please enter the Amount.")]
        public decimal Amount { get; set; }
        public string AmountInWord { get; set; }
      //  [Required]
        public string Vapata { get; set; }
     //   [Required(ErrorMessage = "Please enter the Depository Office Name.")]
        public string DepositoryOfficeName { get; set; }
     //   [Required(ErrorMessage = "Please enter the Depository Office Code.")]
        public long DepositoryOfficeCode { get; set; }
     //   [Required(ErrorMessage = "Please enter the Depositor Name.")]
        public string DepositorName { get; set; }
     //   [Required(ErrorMessage = "Please enter the Depositor Phone.")]
        public string DepositorPhone { get; set; }
     //   [Required(ErrorMessage = "Please enter the Transaction Date.")]
        public DateTime TransactionDate { get; set; }//
        public DateTime TransactionDateNep { get; set; } ///
        [Required(ErrorMessage = "Please enter the Pan no.")]
        public string PanNo { get; set; }
        public IFormFile VoucherImgFront { get; set; }//
        public string VoucherImgFrontUrl { get; set; }///
        public IFormFile VoucherImgBack { get; set; }///
        public string VoucherImgBackUrl { get; set; }///
        public string SignatureUrl { get; set; }///
        public string Remarks { get; set; }///
        public StatusEnum? Status { get; set; }///
        public string CreatedBy { get; set; }///
        public DateTime CreatedAt { get; set; }///
         public DateTime LastUpdatedAt { get; set; }///
        public string LastUpdatedBy { get; set; }///
        public int? TheatreId { get; set; }///
        public string TheatreName { get; set; }///
        public decimal GrossCollection { get; set; }
        public decimal FineAmount { get; set; }
        public IFormFile FineVoucherReceiptFile { get; set; }///
        public IFormFile MonthlySalesReportFile { get; set; }///

        public string FineVoucherReceiptUrl { get; set; }
        public string MonthlySalesReportUrl { get; set; }
        public List<string> NepaliMonth { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace AdminLTE.MVC.ViewModels
{
    public sealed class ReceiptExportModelMap : ClassMap<ReceiptVM>
    {
        public ReceiptExportModelMap()
        {
            Map(m => m.BankName).Name("Bank Name");
            Map(m => m.BankBranchName).Name("Bank Branch Name");
            Map(m => m.VoucherNo).Name("Voucher No");
            Map(m => m.AccountNo).Name("Account No");
            Map(m => m.AccountOperatingOffice).Name("Account Operating Office");
            Map(m => m.ChequeDraftNo).Name("Cheque/Draft No");
            Map(m => m.Amount).Name("Amount");
            Map(m => m.AmountInWord).Name("Amount In Words");
            Map(m => m.Vapata).Name("Vapata");
            Map(m => m.DepositoryOfficeName).Name("Depository Office Name");
            Map(m => m.DepositoryOfficeCode).Name("Depository Office Code");
            Map(m => m.DepositorName).Name("Depositor Name");
            Map(m => m.DepositorPhone).Name("Depositor Phone");
            Map(m => m.TransactionDate).Name("Transaction Date");
            Map(m => m.TransactionDateNep).Name("Transaction Date (Nepali)");
            Map(m => m.PanNo).Name("PAN No");
            Map(m => m.Remarks).Name("Remarks");
            Map(m => m.CreatedBy).Name("Created By");
            Map(m => m.CreatedAt).Name("Created At");
            Map(m => m.TheatreName).Name("Theatre Name");
            Map(m => m.Status).Name("Status");
        }
    }
}
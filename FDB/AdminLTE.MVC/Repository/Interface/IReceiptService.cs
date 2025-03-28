using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.DTO;
using AdminLTE.MVC.Enums;
using AdminLTE.MVC.ViewModels;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface IReceiptService
    {
        Task<List<ReceiptVM>> GetAllReceiptAsync(int? theaterId);
        Task<List<DashboardTableDataVm>> GetAllReceiptGroupByAsync(int? theaterId);
        Task<ReceiptVM> GetReceiptByIdAsync(int Id);
        Task<string> AddReceiptAsync(ReceiptVM ItemModel);
        Task<string> UpdateReceiptAsync(int Id, ReceiptVM ItemModel);
        Task<string> DeleteReceiptAsync(int Id);
        Task<string> ChangeStatus(List<int> Ids ,StatusEnum Status);
        Task<ReceiptSummaryDTO> GetTodaysAsync(int? theaterId);
        Task<ChartDataDTO> GetChartData(int? theaterId);
        
    }
}
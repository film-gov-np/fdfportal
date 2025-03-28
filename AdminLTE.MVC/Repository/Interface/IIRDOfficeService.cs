using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.ViewModels;
using Microsoft.AspNetCore.JsonPatch;

namespace AdminLTE.MVC.Repository.Interface
{
    public interface IIRDOfficeService
    {
        Task<List<IRDOfficeVM>> GetAllIRDOfficeAsync();
        Task<IRDOfficeVM> GetIRDOfficeByIdAsync(int Id);
        Task<string> AddIRDOfficeAsync(IRDOfficeVM ItemModel);
        Task<string> UpdateIRDOfficeAsync(int Id, IRDOfficeVM ItemModel);
        Task<string> DeleteIRDOfficeAsync(int Id);
       
    }
}
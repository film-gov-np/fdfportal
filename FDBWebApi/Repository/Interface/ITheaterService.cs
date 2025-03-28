using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDBWebApi.ResponseModel;

namespace FDBWebApi.Repository.Interface
{
    public interface ITheaterService
    {
        Task<List<TheaterResponseModel>> GetAllTheaterAsync(int TheatreId);
    }
}
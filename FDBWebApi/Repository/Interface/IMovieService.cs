using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDBWebApi.ResponseModel;

namespace FDBWebApi.Repository.Interface
{
    public interface IMovieService
    {
        Task<List<MovieResponseModel>> GetAllMovieAsync(int MovieId);
    }
}
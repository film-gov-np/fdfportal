using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FDBWebApi.Models;
using FDBWebApi.Repository.Interface;
using FDBWebApi.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace FDBWebApi.Repository.Services
{
    public class TheaterService : ITheaterService
    {
        private readonly FBDDbContext _context;
        private readonly IMapper _mapper;

        public TheaterService(FBDDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TheaterResponseModel>> GetAllTheaterAsync(int TheatreId)
        {
            var result = await _context.Theaters.Where(x => x.TheaterId > TheatreId).ToListAsync();
            return _mapper.Map<List<TheaterResponseModel>>(result);
        }


    }
}
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
    public class BrandService : IBrandService
    {
        private readonly FBDDbContext _context;
        private readonly IMapper _mapper;

        public BrandService(FBDDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BrandResponseModel>> GetAllBrandAsync(int BrandId)
        {
            var result = await _context.Brands.Where(x => x.BrandId > BrandId).ToListAsync();
            return _mapper.Map<List<BrandResponseModel>>(result);
        }


    }
}
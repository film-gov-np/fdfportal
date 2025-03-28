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
    public class MovieService : IMovieService
    {
        private readonly FBDDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(FBDDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MovieResponseModel>> GetAllMovieAsync(int MovieId)
        {
            var result = await _context.Movies.Where(x => x.MovieId > MovieId).ToListAsync();
            return _mapper.Map<List<MovieResponseModel>>(result);
        }


    }
}
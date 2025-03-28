using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FDBWebApi.Models;
using FDBWebApi.ResponseModel;

namespace FDBWebApi.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Theater, TheaterResponseModel>().ReverseMap();
            CreateMap<Movie, MovieResponseModel>().ReverseMap();
            CreateMap<Brand, BrandResponseModel>().ReverseMap();
        }
    }
}
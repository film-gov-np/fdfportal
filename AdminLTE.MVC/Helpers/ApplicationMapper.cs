using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.ViewModels;
using AutoMapper;


namespace AdminLTE.MVC.Helpers
{
    public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<Theatre, TheaterVM>().ReverseMap();
        
        CreateMap<ReceiptUpload, ReceiptVM>()
            .ForMember(dest => dest.VoucherImgFrontUrl, opt => opt.MapFrom(src => src.VoucherImgFront))
            .ForMember(dest => dest.VoucherImgBackUrl, opt => opt.MapFrom(src => src.VoucherImgBack))
            .ForMember(dest => dest.VoucherImgFront, opt => opt.Ignore())
            .ForMember(dest => dest.VoucherImgBack, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.VoucherImgFront, opt => opt.MapFrom(src => src.VoucherImgFrontUrl))
            .ForMember(dest => dest.VoucherImgBack, opt => opt.MapFrom(src => src.VoucherImgBackUrl));

            CreateMap<MovieMVC, MovieVM>().ReverseMap();
            CreateMap<BrandMVC, BrandVM>().ReverseMap();
            CreateMap<IRDOffice, IRDOfficeVM>().ReverseMap();

        }
    }

}
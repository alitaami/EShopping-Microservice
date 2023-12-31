﻿using AutoMapper;
using Discount.Core.Entities;
using Discount.Core.ViewModels;
using Discount.Grpc.Protos;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.MappingProfile
{
    public class DiscountMappingProfile : Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
            CreateMap<Coupon, CouponViewModel>().ReverseMap();
            CreateMap<Coupon, CouponUpdateViewModel>().ReverseMap();
            CreateMap<CouponModel, ServiceResult>().ReverseMap();
            CreateMap<DeleteDiscountResponse, ServiceResult>().ReverseMap();
            CreateMap<CreateDiscountRequest, CouponViewModel>().ReverseMap();
            CreateMap<UpdateDiscountRequest, CouponUpdateViewModel>().ReverseMap();
            CreateMap<CreateDiscountRequest, CouponViewModel>().ReverseMap();

        }
    }
}

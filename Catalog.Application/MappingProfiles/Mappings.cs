﻿using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.Models;
using Catalog.Core.Entities.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.MappingProfiles
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<BrandsDto, ProductBrand>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<TypesDto, ProductType>().ReverseMap();
            CreateMap<Product, ProductCreateViewModel>().ReverseMap();
            CreateMap<Product, ProductUpdateViewModel>().ReverseMap();
            CreateMap<Pagination<Product>,Pagination<ProductDto>>().ReverseMap();
        }
    }
}

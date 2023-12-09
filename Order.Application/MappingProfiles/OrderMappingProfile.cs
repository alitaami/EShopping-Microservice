using AutoMapper;
using Order.Application.Responses;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.MappingProfiles
{
    public class OrderMappingProfile  : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order.Core.Entities.Order, OrderDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using Entities.Base;
using EventBus.Message.Events;
using Order.Application.Responses;
using Order.Application.ViewModels;
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
            CreateMap<Order.Core.Entities.Order, CheckoutOrderViewModel>().ReverseMap();
            CreateMap<Order.Core.Entities.Order, UpdateOrderViewModel>().ReverseMap();
            CreateMap<int, ServiceResult>().ReverseMap();
            CreateMap<CheckoutOrderViewModel, BasketChekoutEvent>().ReverseMap();
        }
    }
}

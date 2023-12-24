using AutoMapper;
using Basket.Core.Dtos;
using Basket.Core.Entities;
using Entities.Base;
using EventBus.Message.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.MappingProfiles
{
    public class BasketMappingProfile  : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();
            CreateMap<BasketCheckout, BasketChekoutEvent>().ReverseMap();
            CreateMap<ServiceResult, ShoppingCartDto>().ReverseMap();

        }
    }
}

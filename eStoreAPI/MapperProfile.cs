using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTOs.Member;
using eStoreAPI.DTOs.Order;
using eStoreAPI.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Member, MemberResponseDTO>();
            CreateMap<MemberCreateRequestDTO, Member>();
            CreateMap<MemberUpdateRequestDTO, Member>();

            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductCreateRequestDTO, Product>();
            CreateMap<ProductUpdateRequestDTO, Product>();

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(des => des.MemberName, act => act.MapFrom(src => (src.Member != null) ? src.Member.Email : ""))
                .ForMember(des => des.orderDetailDTOs, act => act.MapFrom(src => (src.OrderDetails)));
            CreateMap<OrderCreateRequestDTO, Order>();  
            CreateMap<OrderUpdateRequestDTO, Order>();

            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(des => des.ProductName, act => act.MapFrom(src => (src.Product != null) ? src.Product.ProductName : ""));
        }
    }
}

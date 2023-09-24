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

            CreateMap<Product, ProductResponseDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => (src.Category != null) ? src.Category.CategoryName : ""));    
            CreateMap<ProductCreateRequestDTO, Product>();
            CreateMap<ProductUpdateRequestDTO, Product>();
            CreateMap<Product, ProductUpdateRequestDTO>();

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(des => des.MemberName, act => act.MapFrom(src => (src.Member != null) ? src.Member.Email : ""))
                .ForMember(des => des.orderDetailDTOs, act => act.MapFrom(src => (src.OrderDetails)));
            CreateMap<OrderCreateRequestDTO, Order>();  
            CreateMap<OrderUpdateRequestDTO, Order>();
            CreateMap<Order, OrderReportDTO>()
                  .ForMember(des => des.MemberName, act => act.MapFrom(src => (src.Member != null) ? src.Member.Email : ""))
                  .ForMember(des => des.orderDetailDTOs, act => act.MapFrom(src => (src.OrderDetails)))
                  .ForMember(des => des.TotalAmount, act => act.MapFrom(src => src.OrderDetails.Sum(x => x.Quantity * x.UnitPrice)));

            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(des => des.ProductName, act => act.MapFrom(src => (src.Product != null) ? src.Product.ProductName : ""));
        }
    }
}

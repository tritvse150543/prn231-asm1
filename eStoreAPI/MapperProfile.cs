using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTOs.Member;
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
        }
    }
}

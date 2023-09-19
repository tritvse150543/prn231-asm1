using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Repositories;
using eStoreAPI.DTOs.Member;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberRepository memberRepository;
        private readonly IMapper mapper;
        public MemberController(MemberRepository memberRepository, IMapper mapper)
        {
            this.memberRepository = memberRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var members = memberRepository.FindAll();
            var dtos = members.Select(x => mapper.Map<MemberResponseDTO>(x)).ToList();
            return new ObjectResult(dtos);
        }
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var member = memberRepository.FindById(id);
            return new ObjectResult(mapper.Map<MemberResponseDTO>(member));
        }


        [HttpPost]
        public IActionResult Create([FromBody] MemberCreateRequestDTO request)
        {
            var member = mapper.Map<Member>(request);
            member.MemberId = memberRepository.FindAll().Max(x => x.MemberId) + 1;
            memberRepository.Add(member);
            return new ObjectResult("Created")
            {
                StatusCode = 201,
            };
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] MemberUpdateRequestDTO request)
        {
            var member = memberRepository.FindById(id);
            if (member == null)
            {
                return BadRequest($"Member Id: {id} not found");
            }
            mapper.Map(request,member);
            memberRepository.Update(member);
            return Ok();
        }
        
    }
}

using AutoMapper;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository orderRepository;
        private readonly OrderDetailRepository detailRepository;
        private readonly IMapper mapper;
        public OrderController(OrderRepository orderRepository, OrderDetailRepository detailRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.detailRepository = detailRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var orders = orderRepository.FindAll();
        }
    }
}

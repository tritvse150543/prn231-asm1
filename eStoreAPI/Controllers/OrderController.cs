using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Repositories;
using eStoreAPI.DTOs.Order;
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
        private readonly ProductRepository productRepository;
        MemberRepository memberRepository;
        private readonly IMapper mapper;
        public OrderController(MemberRepository memberRepository, OrderRepository orderRepository, OrderDetailRepository detailRepository, ProductRepository productRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.detailRepository = detailRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = orderRepository.FindAll(null, x => x.Member);
            var dtos = orders.Select(x => mapper.Map<OrderResponseDTO>(x)).ToList();
            return Ok(dtos);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = orderRepository.FindAll(null, x => x.Member).FirstOrDefault(x => x.OrderId == id);
            if (order == null)
            {
                return BadRequest("Not found");
            }
            var dto = mapper.Map<OrderResponseDTO>(order);
            return Ok(dto);
        }
        [HttpGet("details")]
        public IActionResult GetDetails(int id)
        {
            try
            {
                var order = orderRepository.FindAll(null, x => x.OrderDetails).FirstOrDefault(x => x.OrderId == id);
                foreach (var detail in order.OrderDetails)
                {
                    var product = productRepository.FindById(detail.ProductId);
                    detail.Product = product;
                }
                return Ok(mapper.Map<OrderResponseDTO>(order));
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("report")]
        public IActionResult CreateReport(DateTime startDate, DateTime endDate)
        {
            var orders = orderRepository
                .FindAll(x => x.OrderDate >= startDate && x.OrderDate <= endDate, x => x.Member, x => x.OrderDetails)
                .OrderBy(x => x.OrderDetails.Sum(y => y.Quantity * y.UnitPrice))
                .Select(x => mapper.Map<OrderResponseDTO>(x));
            return Ok(orders);
        }
        [HttpPost]
        public IActionResult Create([FromBody] OrderCreateRequestDTO dto)
        {
            var order = mapper.Map<Order>(dto);
            var member = memberRepository.FindById((int)dto.MemberId);
            if (member == null)
            {
                return NotFound();
            }
            order.OrderId = orderRepository.FindAll().Max(x => x.OrderId) + 1;
            var orderDetails = new List<OrderDetail>(); 
            foreach(var request in dto.ProductIds)
            {
                var a = dto.ProductIds.FindAll(x => x.Id == request.Id);
                if (a.Count > 1)
                {
                    return BadRequest("Duplicate Product Id");
                }
            }
            foreach (var request in dto.ProductIds)
            {
                try
                {                
                    Product product = productRepository.FindById(request.Id);
                    if (request.Quantity > product.UnitsInStock)
                    {
                        return BadRequest($"Product : {product.ProductName} dont have enough quantity!");
                    }             
                    orderDetails.Add(new OrderDetail
                    {
                        ProductId = request.Id,
                        Discount = 0,
                        OrderId = order.OrderId,
                        Quantity = request.Quantity,
                        UnitPrice = product.UnitPrice
                    });

                }catch (Exception ex)
                {
                    return BadRequest($"Product Id: {request.Id} is not exist! ");
                }
            }
            orderRepository.Add(order);
            foreach (var detail in orderDetails)
            {
                detailRepository.Add(detail);
                var product = productRepository.FindById(detail.ProductId);
                product.UnitsInStock -= detail.Quantity;
                productRepository.Update(product);
            }
            
            return Ok(order.OrderId);
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id,[FromBody] OrderUpdateRequestDTO dto)
        {
            var order = orderRepository.FindById(id);
            mapper.Map(dto,order);
            orderRepository.Update(order);
            return Ok();
        }

    }
}

using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Repositories;
using eStoreAPI.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace eStoreAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository productRepository;
        private readonly OrderDetailRepository detailRepository;
        private readonly IMapper mapper;
        public ProductController(ProductRepository productRepository, OrderDetailRepository detailRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.detailRepository = detailRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = productRepository.FindAll(null,x => x.Category);
            var dtos = products.Select(x => mapper.Map<ProductResponseDTO>(x));
            return Ok(dtos);
        }
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var products = productRepository.FindAll(x => x.ProductId == id, x => x.Category).FirstOrDefault();
            var dto = mapper.Map<ProductResponseDTO>(products);
            return Ok(dto);
        }
        [HttpGet("search")]
        public IActionResult Get([FromQuery] decimal? UnitPrice, [FromQuery] string? ProductName) 
        {
            Expression<Func<Product, bool>> predicate = (x => true);
            if (ProductName != null && UnitPrice != null) 
            {
                predicate = (x => x.ProductName.ToLower().Contains(ProductName.ToLower()) && x.UnitPrice <= UnitPrice);
            }
            else if (ProductName != null)
            {
                predicate = (x => x.ProductName.ToLower().Contains(ProductName.ToLower()));
            }else if (UnitPrice != null)
            {
                predicate = (x => x.UnitPrice <= UnitPrice);
            }
            var products = productRepository.FindAll(predicate,x => x.Category);
            var dtos = products.Select(x => mapper.Map<ProductResponseDTO>(x));
            return Ok(dtos);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ProductCreateRequestDTO request)
        {
            var product = mapper.Map<Product>(request);
            int id = productRepository.FindAll().Max(x => x.ProductId) + 1;
            product.ProductId = id;
            try
            {
                productRepository.Add(product);
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Some error occur") ;
            }
            return new ObjectResult(id)
            {
                StatusCode = 201
            };
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id,[FromBody] ProductUpdateRequestDTO request)
        {
            var product = productRepository.FindById(id);
            if (product == null)
            {
                return BadRequest("Not Found");
            }
            mapper.Map(request,product);
            try
            {
                productRepository.Update(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Some error occur");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id) 
        { 
            var details = detailRepository.FindAll(x => x.ProductId == id);
            foreach(var detail in details)
            {
                detailRepository.Delete(detail);
            }
            productRepository.Delete(id);
            return Ok();
        }

    }
}

using AutoMapper;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository categoryRepository;
        private readonly IMapper mapper;
        public CategoryController(CategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
        [HttpGet]        
        public IActionResult Get()
        {
            var categories = categoryRepository.FindAll();
            return Ok(categories);
        }
    }
    
}

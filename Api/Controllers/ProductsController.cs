using Microsoft.AspNetCore.Mvc;
using ProductsApplication.DTOs;
using ProductsApplication.Services;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(new { message = "Product not found." });

            return Ok(product);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto dto)
        {
            var (isSuccess, message, id) = _productService.Add(dto);

            if (!isSuccess)
                return BadRequest(new { message });

            return Ok(new { message, productId = id });
        }
    }
}
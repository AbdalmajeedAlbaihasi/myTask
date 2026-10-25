using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsApplication.DTOs;
using ProductsApplication.Services;


namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        
        

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation( "Getting employee with ID {EmployeeId}", id);
            if (id <= 0)
            {
                _logger.LogWarning(
                "Invalid employee ID: {EmployeeId}", id);
                return BadRequest();
            }


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
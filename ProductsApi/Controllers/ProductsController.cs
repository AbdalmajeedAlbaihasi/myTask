using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsApplication.DTOs;
using ProductsApplication.Services;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting product with ID {ProductId}", id);
            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}", id);
                return BadRequest(new { message = "Invalid product ID." });
            }

            var product = await _productService.GetByIdAsync(id, cancellationToken);
            if (product == null)
                return NotFound(new { message = "Product not found." });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            var (isSuccess, message, id) = await _productService.AddAsync(dto, cancellationToken);

            if (!isSuccess)
                return BadRequest(new { message });

            return Ok(new { message, productId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            var (isSuccess, message) = await _productService.UpdateAsync(id, dto, cancellationToken);

            if (!isSuccess)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var (isSuccess, message) = await _productService.DeleteAsync(id, cancellationToken);

            if (!isSuccess)
                return NotFound(new { message });

            return Ok(new { message });
        }


        [HttpGet("performance-test")]
        public async Task<IActionResult> PerformanceTest(CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();


            stopwatch.Start();
            var task1Result = await _productService.GetAllAsync(cancellationToken);
            var task2Result = await _productService.GetAllAsync(cancellationToken);
            stopwatch.Stop();
            long sequentialTime = stopwatch.ElapsedMilliseconds;


            stopwatch.Restart();
            var fetchTask1 = _productService.GetAllAsync(cancellationToken);
            var fetchTask2 = _productService.GetAllAsync(cancellationToken);
            await Task.WhenAll(fetchTask1, fetchTask2);
            stopwatch.Stop();
            long parallelTime = stopwatch.ElapsedMilliseconds;


            var performanceReport = new
            {
                Title = "Performance Measurement Report",
                SequentialExecutionTimeMs = sequentialTime,
                ParallelExecutionTimeMs = parallelTime,
                Analysis = "When executing independent I/O-bound operations (like database or network calls) concurrently using Task.WhenAll, the parallel approach significantly reduces total waiting time compared to executing them sequentially one after another. However, for extremely fast in-memory operations, thread-dispatching overhead might occasionally make the difference minimal, but for database queries, parallelism improves throughput and performance."
            };

            return Ok(performanceReport);
        }
    }
}
using Microsoft.Extensions.Logging;
using ProductsApplication.DTOs;
using ProductsApplication.Interfaces;
using ProductsDomain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsApplication.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = await _repository.GetAllAsync(cancellationToken);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public async Task<(bool IsSuccess, string Message, int Id)> AddAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Product name is required.", 0);

            if (dto.Price <= 0)
                return (false, "Product price must be greater than zero.", 0);

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            await _repository.AddAsync(product, cancellationToken);
            _logger.LogInformation("Product {ProductName} created", dto.Name);

            return (true, "Product added successfully.", product.Id);
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAsync(int id, CreateProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
                return (false, "Product not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Product name is required.");

            if (dto.Price <= 0)
                return (false, "Product price must be greater than zero.");

            product.Name = dto.Name;
            product.Price = dto.Price;

            await _repository.UpdateAsync(product, cancellationToken);
            _logger.LogInformation("Product {ProductId} updated", id);

            return (true, "Product updated successfully.");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
                return (false, "Product not found.");

            await _repository.DeleteAsync(product, cancellationToken);
            _logger.LogInformation("Product {ProductId} deleted", id);

            return (true, "Product deleted successfully.");
        }
    }
}
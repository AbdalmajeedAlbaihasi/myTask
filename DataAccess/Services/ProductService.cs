using ProductsDomain;
using ProductsApplication.Interfaces;
using ProductsApplication.DTOs;

namespace ProductsApplication.Services
{
    public class ProductService 
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }


        public List<ProductDto> GetAll()
        {
            return _repository.GetAll().Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
        }

        public ProductDto? GetById(int id)
        {
            var product = _repository.GetById(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }


        public (bool IsSuccess, string Message, int Id) Add(CreateProductDto dto)
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

            _repository.Add(product);

            return (true, "Product added successfully.", product.Id);
        }
    }
}
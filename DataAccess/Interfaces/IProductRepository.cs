using ProductsDomain;

namespace ProductsApplication.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Add(Product product);
    }
}

using ProductsApplication.Interfaces;
using ProductsDomain;
using Products.Infrastructure.Persistence;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }


        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }


        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges(); 
        }
    }
}
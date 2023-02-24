using Microsoft.EntityFrameworkCore;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Persistence.Contexts;

namespace Supermarket.API.Persistence.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<QueryResult<Product>> ListAsync(ProductsQuery query)
        {
            IQueryable<Product> queryable = _context.Products
                                                    .Include(p => p.Category)
                                                    .AsNoTracking();

            if (query.CategoryId.HasValue && query.CategoryId > 0)
            {
                queryable = queryable.Where(p => p.CategoryId == query.CategoryId);
            }

            int totalItems = await queryable.CountAsync();

            List<Product> products = await queryable.Skip((query.Page - 1) * query.ItemsPerPage)
                                                    .Take(query.ItemsPerPage)
                                                    .ToListAsync();

            return new QueryResult<Product>
            {
                Items = products,
                TotalItems = totalItems,
            };
        }
    }
}

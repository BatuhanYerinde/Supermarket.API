using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;

namespace Supermarket.API.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<QueryResult<Product>> ListAsync(ProductsQuery query);
    }
}

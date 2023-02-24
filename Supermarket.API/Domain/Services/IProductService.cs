using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;

namespace Supermarket.API.Domain.Services
{
    public interface IProductService
    {
        Task<QueryResult<Product>> ListAsync(ProductsQuery productsQuery);
    }
}

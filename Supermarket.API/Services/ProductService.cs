using Microsoft.Extensions.Caching.Memory;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Domain.Services;
using Supermarket.API.Infrastructure;

namespace Supermarket.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;

        public ProductService(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<QueryResult<Product>> ListAsync(ProductsQuery query)
        {
            string cacheKey = GetCacheKeyForProductsQuery(query);
            var products = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productRepository.ListAsync(query);
            });

            return products;
        }

        private string GetCacheKeyForProductsQuery(ProductsQuery query)
        {
            string key = CacheKeys.ProductsList.ToString();

            if (query.CategoryId.HasValue && query.CategoryId > 0)
            {
                key = string.Concat(key, "_", query.CategoryId.Value);
            }

            key = string.Concat(key, "_", query.Page, "_", query.ItemsPerPage);
            return key;
        }
    }
}

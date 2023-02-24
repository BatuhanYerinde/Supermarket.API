using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;
using Supermarket.API.Domain.Services;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

     
        [HttpGet]
        [ProducesResponseType(typeof(QueryResultResource<ProductResource>), 200)]
        public async Task<QueryResultResource<ProductResource>> ListAsync([FromQuery] ProductsQueryResource query)
        {
            var productsQuery = _mapper.Map<ProductsQueryResource, ProductsQuery>(query);
            var queryResult = await _productService.ListAsync(productsQuery);

            var resource = _mapper.Map<QueryResult<Product>, QueryResultResource<ProductResource>>(queryResult);
            return resource;
        }


    }
}

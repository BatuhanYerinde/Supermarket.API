using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using Supermarket.API.Domain.Services.Cache;
using Supermarket.API.Extensions;
using Supermarket.API.Infrastructure;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper, ICacheService cacheService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryResource>> GetAllAsync()
        {
            string key = CacheKeys.CategoriesList.ToString();
            var cacheData = await _cacheService.GetDataAsync<IEnumerable<CategoryResource>>(key);
            if (cacheData != null)
            {
                return cacheData;
            }
            var categories = await _categoryService.ListAsync();
            var resources = _mapper.Map<IEnumerable<CategoryResource>>(categories);

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            await _cacheService.SetDataAsync<IEnumerable<CategoryResource>>(key, resources, expirationTime);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryResource resource)
        {
            string key = CacheKeys.CategoriesList.ToString();
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryResource, Category>(resource);

            var result = await _categoryService.SaveAsync(category);
            if (!result.Success)
                return BadRequest(result.Message);
            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            await _cacheService.RemoveDataAsync(key);
            return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryResource resource)
        {
            string key = CacheKeys.CategoriesList.ToString();
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            await _cacheService.RemoveDataAsync(key);
            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            string key = CacheKeys.CategoriesList.ToString();
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            await _cacheService.RemoveDataAsync(key);
            return Ok(categoryResource);
        }
    }
}

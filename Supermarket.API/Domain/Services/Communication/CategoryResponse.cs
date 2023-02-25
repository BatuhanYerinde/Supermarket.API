using Supermarket.API.Domain.Models;

namespace Supermarket.API.Domain.Services.Communication
{
    public class CategoryResponse : BaseResponse<Category>
    {
        public CategoryResponse(Category category) : base(category)
        { }
        public CategoryResponse(string message) : base(message)
        { }
    }
}

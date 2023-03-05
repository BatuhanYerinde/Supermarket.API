using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using Supermarket.API.Domain.Services.Communication;
using Supermarket.API.Handler;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    public class UserController : BaseApiController
    {
        readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(UserResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SaveAsync([FromBody] SaveUserResource resource)
        {
            var user = _mapper.Map<SaveUserResource, User>(resource);
            var result = await _userService.SaveAsync(user);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var userResource = _mapper.Map<User, UserResource>(result.Resource);
            return Ok(userResource);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Token), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin userLogin)
        {
            var result = await _userService.CheckUserExistAndCreateAccessTokenAsync(userLogin);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            return Ok(result.Resource);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(Token), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> RefreshTokenLogin([FromQuery] string refreshToken)
        {
            var result = await _userService.CreateAccessTokenByRefreshTokenAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            return Ok(result.Resource);
        }
    }
}

using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<SaveUserResource> _saveUserResourceValidator;
        private readonly IValidator<UserLogin> _userLoginValidator;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration configuration, IMapper mapper, IUserService userService,
            IValidator<SaveUserResource> saveUserResourceValidator, IValidator<UserLogin> userLoginValidator,
            ILogger<UserController> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
            _saveUserResourceValidator = saveUserResourceValidator;
            _userLoginValidator = userLoginValidator;
            _logger = logger;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(UserResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SaveAsync([FromBody] SaveUserResource resource)
        {
            var validationResult = _saveUserResourceValidator.Validate(resource);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(err => err.ErrorMessage)?.ToList();
                return BadRequest(new ErrorResource(errors));
            }

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
            var validationResult = _userLoginValidator.Validate(userLogin);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(err => err.ErrorMessage)?.ToList();
                return BadRequest(new ErrorResource(errors));
            }
            var result = await _userService.CheckUserExistAndCreateAccessTokenAsync(userLogin);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            _logger.LogInformation("Login is successful!");
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

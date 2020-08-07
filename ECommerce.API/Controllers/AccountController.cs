using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.API.Extensions;
using ECommerce.Core.Entities.Identity;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IHttpContextAccessor accessor, IMapper mapper)
        {
            _mapper = mapper;
            _accessor = accessor;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == default(AppUser)) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (CheckUserNameIsExist(register.UserName).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationResponse()
                {
                    Errors = new[] { "Username is in use" }
                });
            }

            var appUser = new AppUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                DisplayName = register.DisplayName
            };

            var result = await _userManager.CreateAsync(appUser, register.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                UserName = appUser.UserName,
                DisplayName = appUser.DisplayName,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        [Authorize()]
        [HttpGet()]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(HttpContext.User);

            return new UserDto
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("usernameexist")]
        public async Task<ActionResult<bool>> CheckUserNameIsExist([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != default(AppUser);
        }

        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailIsExistAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != default(AppUser);
        }

        [Authorize()]
        [HttpGet("adress")]
        public async Task<ActionResult<AdressDto>> GetUserAdress()
        {
            AppUser appUser = await _userManager.FindUserByCalimsPrincipleWithAdressAsync(HttpContext.User);

            return _mapper.Map<Adress, AdressDto>(appUser.Adress);
        }

        [HttpPost("updateadress")]
        [Authorize]
        public async Task<ActionResult<AdressDto>> UpdateUserAdress(AdressDto adress)
        {
            var user = await _userManager.FindUserByCalimsPrincipleWithAdressAsync(HttpContext.User);

            user.Adress = _mapper.Map<AdressDto, Adress>(adress);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Adress, AdressDto>(user.Adress));

            return Problem(result.Errors.First().Description, string.Empty, 500);
        }
    }
}
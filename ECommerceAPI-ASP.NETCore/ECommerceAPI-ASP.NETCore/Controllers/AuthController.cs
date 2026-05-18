using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.Auth;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(identityUser, "Customer");
                if (identityResult.Succeeded)
                {
                    return Created("", "User Registered Successfully, pls Login");
                }
            }
            return BadRequest("Registration failed. Please check your input and try again.");
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [EnableRateLimiting("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.email);
            if (user is not null)
            {
                var PasswordResult = await userManager.CheckPasswordAsync(user, request.Password);
                if (PasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }

                }
            }
            return Unauthorized("Invalid email or password.");
        }
        [HttpDelete("DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles ="Admin,Vendor,Customer")]
        public async Task<IActionResult> Delete()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized();
            var identityUser=await userManager.FindByIdAsync(userID);
            if(identityUser == null)
                return BadRequest("Something Went Wrong!");
            await userManager.DeleteAsync(identityUser);
            return Ok("User Deleted Successfully");
            
        }
        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequestDto request)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
                return Unauthorized();
            var identityUser = await userManager.FindByIdAsync(userID);
            if (identityUser == null)
                return BadRequest("Something Went Wrong!");
            var response=await userManager.ChangePasswordAsync(identityUser,request.CurrentPassword,request.NewPassword);
            if (!response.Succeeded)
                return BadRequest(response.Errors);
            return Ok("Password Changed Successfully");
        }
    }
}

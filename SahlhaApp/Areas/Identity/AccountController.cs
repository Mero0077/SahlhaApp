using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SahlhaApp.Models.Models;
using SahlhaApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using SahlhaApp.Models.DTOs.Request;
using Mapster;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using SahlhaApp.Models.DTOs.Request.PasswordRequests;
using SahlhaApp.Models.DTOs.Request.RegisterRequest;
using SahlhaApp.Models.DTOs.Request.Profile;
using System.Text.Json;
using SahlhaApp.Models.DTOs.Response.Location;
using Microsoft.AspNetCore.Authorization;

namespace SahlhaApp.Areas.Identity.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jwtOptions;
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            JwtOptions options,
            HttpClient httpClient, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtOptions = options;
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        private string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigninKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequesrDto registerRequesrDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(registerRequesrDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "A user with this email already exists." });
            }


            var user = new ApplicationUser()
            {
                Email = registerRequesrDto.Email,
                FirstName = registerRequesrDto.FirstName,
                LastName = registerRequesrDto.LastName,
                LocationLatitude = registerRequesrDto.LocationLatitude,
                LocationLongitude = registerRequesrDto.LocationLongitude,
                UserName = registerRequesrDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequesrDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "User");
            var accessToken = GenerateToken(user);

            return Ok(new
            {
                token = accessToken,
                message = "Registration successful!"
            });

        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded) return Ok(new { Message = "Email confirmed successfully" });

            return NotFound();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null) return NotFound(new { Message = "User not found" });

            if (user.Email != loginRequestDto.Email || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                return Unauthorized(new { Message = "Invalid email or password" });

            var accessToken= GenerateToken(user);
            return Ok(accessToken);
        }

        //[HttpPost("Logout")]
        //public async Task<IActionResult> LogoutAsync()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok(new { Message = "User logged out successfully." });
        //}

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto forgetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequestDto.Email);
            if (user == null) return NotFound();

            var otpCode = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("OTP", otpCode);
            HttpContext.Session.SetString("Email", forgetPasswordRequestDto.Email);

            // await _emailSender.SendEmailAsync(user.Email, "Your OTP Code",
            //     $"Your OTP code is: <strong>{otpCode}</strong>");

            return Ok();
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequestDto verifyOtpRequestDto)
        {
            var otpCode = HttpContext.Session.GetString("OTP");
            var email = HttpContext.Session.GetString("Email");

            if (otpCode == null || email == null) return NotFound();

            if (otpCode == verifyOtpRequestDto.Otp.ToString() && email == verifyOtpRequestDto.Email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return NotFound();

                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("Email");

                return Ok(new { Message = "OTP verified successfully" });
            }

            return BadRequest(new { Message = "Invalid OTP" });
        }

        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            //if (resetPasswordRequestDto.NewPassword != resetPasswordRequestDto.ConfirmPassword)
            //    return BadRequest(new { Message = "Passwords do not match" });

            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordRequestDto.NewPassword);

            if (result.Succeeded)
                return Ok(new { Message = "Password reset successfully" });

            return BadRequest(new { Message = "Failed to reset password" });
        }

        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequestDto updateAddressRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(updateAddressRequestDto.ApplicationUserId);
            if (user == null) return NotFound(new { Message = "User not found" });

            var updatedUser = updateAddressRequestDto.Adapt<ApplicationUser>();
            await _userManager.UpdateAsync(updatedUser);

            return Ok(new { Message = "Address updated successfully" });
        }
    }
}

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

namespace SahlhaApp.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jwtOptions;
        private readonly HttpClient _httpClient;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            JwtOptions options,
            HttpClient httpClient)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtOptions = options;
            _httpClient = httpClient;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequesrDto registerRequesrDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = registerRequesrDto.Adapt<ApplicationUser>();
            var userIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            var location = await GetLocationFromIpAsync(userIP);

            if (location != null)
            {
                user.LocationLatitude = location.Latitude;
                user.LocationLongitude = location.Longitude;
            }

            var result = await _userManager.CreateAsync(user, registerRequesrDto.Password);

            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded)
            {
                Console.WriteLine($"User IP: {userIP}");
                Console.WriteLine($"Location info: {location?.Latitude}, {location?.Longitude}");

                return Ok(new { Message = "User registered successfully", IP = userIP, Location = location });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }

        private async Task<IpLocationResponse?> GetLocationFromIpAsync(string ip)
        {
            var response = await _httpClient.GetAsync($"http://ip-api.com/json/{ip}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var location = JsonSerializer.Deserialize<IpLocationResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (location?.Status != "success") return null;

            return location;
        }



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

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigninKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginRequestDto.Email),
                    new Claim(ClaimTypes.Email, loginRequestDto.Email)
                }),
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "User logged out successfully." });
        }

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
            if (resetPasswordRequestDto.NewPassword != resetPasswordRequestDto.ConfirmPassword)
                return BadRequest(new { Message = "Passwords do not match" });

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

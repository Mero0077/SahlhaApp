using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using SahlhaApp.Models.Models;
//using SahlhaApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using SahlhaApp.Models.DTOs.Request;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SahlhaApp.Models.DTOs.Request.RegisterRequest;
using SahlhaApp.Models.DTOs.Request.PasswordRequests;

namespace SahlhaApp.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly JwtOptions _jwtOptions;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender,
            JwtOptions options)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._emailSender = emailSender;
            this._jwtOptions = options;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequesrDto registerRequesrDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser()
            {
                UserName = registerRequesrDto.UserName,
                Email = registerRequesrDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerRequesrDto.Password);
            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded)
            {
                // Corrected userId assignment
                var userId = user.Id;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var returnUrl = Url.Content("~/");

                // Fixed URL generation with correct controller name
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account", // Ensure your ConfirmEmail action is in "AccountController"
                    new { userId = userId, code = token, returnUrl = returnUrl },
                    protocol: Request.Scheme
                );

                await _emailSender.SendEmailAsync(registerRequesrDto.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok(new { Message = "User registered successfully" });
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
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

            if (user.Email != loginRequestDto.Email || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password)) return Unauthorized(new { Message = "Invalid email or password" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigninKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, loginRequestDto.Email),
                    new (ClaimTypes.Email, loginRequestDto.Email)
                }),
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesstoken = tokenHandler.WriteToken(securityToken);

            return Ok(accesstoken);
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "User loged out successfully." });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto forgetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequestDto.Email);
            if (user == null) return NotFound();

            // create random OTP from 6 digits
            var otpCode = new Random().Next(100000, 999999).ToString();

            // save the OTP code and the user email in the sever sessioni
            HttpContext.Session.SetString("OTP", otpCode);
            HttpContext.Session.SetString("Email", forgetPasswordRequestDto.Email);

            // send the OTP code to the user email using the email adderss
            await _emailSender.SendEmailAsync(user.Email, "Your OTP Code",
               $"Your OTP code is: <strong>{otpCode}</strong>");

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
            else
            {
                return BadRequest(new { Message = "Invalid OTP" });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (resetPasswordRequestDto.NewPassword != resetPasswordRequestDto.ConfirmPassword) return BadRequest(new { Message = "Passwords do not match" });

            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user is null) return NotFound(new { Message = "User not found" });

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordRequestDto.NewPassword);

            if (result.Succeeded) return Ok(new { Message = "Password reset successfully" });

            return BadRequest(new { Message = "Failed to reset password" });
        }
    }
}

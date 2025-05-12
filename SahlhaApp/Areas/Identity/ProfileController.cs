
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Request;


//using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs.Request.PasswordRequests;
using SahlhaApp.Models.DTOs.Request.Profile;
using SahlhaApp.Models.DTOs.Response.ProfileResponse;
using System.Security.Claims;


namespace SahlhaApp.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string FilePath = "D:\\.net diplom\\Angular\\SahetyAPI\\assets\\img";

        public ProfileController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this._userManager = userManager;
           _unitOfWork = unitOfWork;
        }



        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            var userInfo = user.Adapt<ProfileInfoResponseDto>();
            var userInProviderRequestes = await _unitOfWork.PendingProviderVerification.GetOne(e => e.ApplicationUserId == userId);
            if(userInProviderRequestes is not null) userInfo.IsRequested = true;

            return Ok(userInfo);
        }

        [HttpPut("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureRequestDto updateProfilePictureRequestDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (updateProfilePictureRequestDto.ImgUrl is null || updateProfilePictureRequestDto.ImgUrl.Length == 0) return BadRequest("No image file was uploaded.");

            var oldPath = user.ImgUrl;

            var fileName = await DocumentHelper.HandleSingleFile(updateProfilePictureRequestDto.ImgUrl, oldPath);
            if (string.IsNullOrEmpty(fileName)) return StatusCode(500, "Error saving image.");

            user.ImgUrl = fileName;
            await _userManager.UpdateAsync(user);

            var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
            return Ok(new { Message = "Profile picture updated successfully", ImageUrl = imageUrl });
        }


        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileRequestDto profileRequestDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            profileRequestDto.Adapt(user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Profile updated successfully" });
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto updatePasswordRequestDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, updatePasswordRequestDto.OldPassword, updatePasswordRequestDto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Password updated successfully" });
        }
    }
}


using Microsoft.AspNetCore.Identity;

//using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs.Request.PasswordRequests;
using SahlhaApp.Models.DTOs.Request.Profile;


namespace SahlhaApp.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("Profile")]
        public async Task<IActionResult> Profile([FromBody] ProfileRequestDto profileRequestDto)
        {
            var user = await _userManager.FindByNameAsync(profileRequestDto.UserName);
            if (user is null) return Unauthorized();

            var userAccount = await _unitOfWork.User.GetUserWithRolesByIdAsync(user.UserName);
            return Ok(userAccount);
        }

        [HttpPost("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureRequestDto updateProfilePictureRequestDto, CancellationToken cancellationToken)
        {
            if (updateProfilePictureRequestDto.ImgUrl == null || updateProfilePictureRequestDto.ImgUrl.Length == 0) return BadRequest();

            var user = await _userManager.FindByEmailAsync(updateProfilePictureRequestDto.Email);
            if (user == null) return NotFound();

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateProfilePictureRequestDto.ImgUrl.FileName);
            var filePath = Path.Combine(this.FilePath, fileName);

            if (!string.IsNullOrEmpty(user.ImgUrl))
            {
                var oldPath = Path.Combine(this.FilePath, user.ImgUrl);
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);

            }

            using (var stream = System.IO.File.Create(filePath))
            {
                await updateProfilePictureRequestDto.ImgUrl.CopyToAsync(stream, cancellationToken);
            }

            user.ImgUrl = fileName;
            await _userManager.UpdateAsync(user);

            return Ok(new { Message = "Profile picture updated successfully", ImageUrl = fileName });
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileRequestDto profileRequestDto)
        {
            var user = await _userManager.FindByNameAsync(profileRequestDto.UserName);
            if (user is null) return NotFound();

            user.UserName = profileRequestDto.UserName;
            await _userManager.UpdateAsync(user);

            return Ok(new { Message = "Profile updated successfully" });
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto updatePasswordRequestDto)
        {
            var user = await _userManager.FindByNameAsync(updatePasswordRequestDto.UserName);
            if (user is null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, updatePasswordRequestDto.OldPassword, updatePasswordRequestDto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Password updated successfully" });
        }
    }
}

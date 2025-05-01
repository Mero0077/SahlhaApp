using SahlhaApp.Models.DTOs.Response.ProfileResponse;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ProfileResponseDto?> GetUserWithRolesByIdAsync(string userName);
    }
}

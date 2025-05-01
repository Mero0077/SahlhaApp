using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs.Response.ProfileResponse;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<ProfileResponseDto?> GetUserWithRolesByIdAsync(string userName)
        {
            var param = new SqlParameter("@UserName", userName);

            var rawResult = await _context
                .Set<ProfileResponseRaw>()
                .FromSqlRaw("EXEC GetUserWithRolesById @UserName", param)
                .AsNoTracking()
                .ToListAsync();

            if (!rawResult.Any())
                return null;

            var grouped = rawResult
                .GroupBy(x => new { x.UserName, x.Email, x.PhoneNumber })
                .Select(g => new ProfileResponseDto
                {
                    UserName = g.Key.UserName,
                    Email = g.Key.Email,
                    PhoneNumber = g.Key.PhoneNumber,
                    Roles = g
                        .Where(x => x.RoleName != null)
                        .Select(x => x.RoleName!)
                        .Distinct()
                        .ToList()
                })
                .FirstOrDefault();

            return grouped;
        }
    }
}

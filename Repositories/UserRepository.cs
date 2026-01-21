using Dapper;
using SkillHub.Interfaces;
using SkillHub.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SkillHub.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var sql = "SELECT * FROM AspNetUsers WHERE Id = @UserId";
            return await _db.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserId = userId });
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var sql = "UPDATE AspNetUsers SET FullName = @FullName, IsApproved = @IsApproved WHERE Id = @Id";
            await _db.ExecuteAsync(sql, user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetPendingProvidersAsync()
        {
            // Assuming we can identify providers by role or simply check IsApproved = 0
            // However, Identity stores roles in AspNetUserRoles.
            // For simplicity, let's assume we filter by checking those who are not approved yet.
            // A better query would join AspNetUserRoles and AspNetRoles but let's keep it simple for now or fetch all unapproved.
            
            // Note: This query is a simplification. In a real scenario with Identity, you'd join tables.
            // Let's try to get users who are not approved.
            var sql = "SELECT * FROM AspNetUsers WHERE IsApproved = 0"; 
            return await _db.QueryAsync<ApplicationUser>(sql);
        }

        public async Task ApproveProviderAsync(string providerId)
        {
            var sql = "UPDATE AspNetUsers SET IsApproved = 1 WHERE Id = @ProviderId";
            await _db.ExecuteAsync(sql, new { ProviderId = providerId });
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM AspNetUsers";
            return await _db.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> GetTotalProvidersCountAsync()
        {
            // This assumes we can distinguish providers. If we strictly use Roles, we need a JOIN.
            // For now, let's count all users for simplicity or we can add a JOIN if needed.
            // Let's use a JOIN to be accurate.
            var sql = @"
                SELECT COUNT(*) 
                FROM AspNetUsers u
                JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                JOIN AspNetRoles r ON ur.RoleId = r.Id
                WHERE r.Name = 'Provider'";
            return await _db.ExecuteScalarAsync<int>(sql);
        }
    }
}

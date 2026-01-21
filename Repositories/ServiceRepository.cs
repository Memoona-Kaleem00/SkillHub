using Dapper;
using Microsoft.Extensions.Configuration;
using SkillHub.Interfaces;
using SkillHub.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillHub.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IDbConnection _db;

        public ServiceRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SkillHub.Models.Service>> GetAllAsync()
        {
            var sql = "SELECT * FROM Services";
            return await _db.QueryAsync<SkillHub.Models.Service>(sql);
        }

        public async Task<SkillHub.Models.Service> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Services WHERE ServiceId = @Id";
            return await _db.QuerySingleOrDefaultAsync<SkillHub.Models.Service>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(SkillHub.Models.Service entity)
        {
            var sql = "INSERT INTO Services (ProviderId, CategoryId, Title, Description, Price, ImageUrl) VALUES (@ProviderId, @CategoryId, @Title, @Description, @Price, @ImageUrl); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.QuerySingleAsync<int>(sql, entity);
        }

        public async Task<int> UpdateAsync(SkillHub.Models.Service entity)
        {
            var sql = "UPDATE Services SET CategoryId = @CategoryId, Title = @Title, Description = @Description, Price = @Price, ImageUrl = @ImageUrl WHERE ServiceId = @ServiceId";
            return await _db.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = @"
                DELETE FROM Bookings WHERE ServiceId = @Id;
                DELETE FROM Services WHERE ServiceId = @Id;";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<SkillHub.Models.Service>> GetServicesByProviderAsync(string providerId)
        {
            var sql = "SELECT * FROM Services WHERE ProviderId = @ProviderId";
            return await _db.QueryAsync<SkillHub.Models.Service>(sql, new { ProviderId = providerId });
        }

        public async Task<IEnumerable<SkillHub.Models.Service>> SearchServicesAsync(string query, int? categoryId, string sort)
        {
            var sql = "SELECT * FROM Services WHERE (@Query IS NULL OR Title LIKE '%' + @Query + '%') AND (@CategoryId IS NULL OR CategoryId = @CategoryId)";
            
            switch (sort)
            {
                case "price_asc":
                    sql += " ORDER BY Price ASC";
                    break;
                case "price_desc":
                    sql += " ORDER BY Price DESC";
                    break;
                case "newest":
                default:
                    sql += " ORDER BY ServiceId DESC";
                    break;
            }

            return await _db.QueryAsync<SkillHub.Models.Service>(sql, new { Query = query, CategoryId = categoryId });
        }
    }
}

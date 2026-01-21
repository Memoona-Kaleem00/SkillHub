using Dapper;
using SkillHub.Interfaces;
using SkillHub.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SkillHub.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnection _db;

        public BookingRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            var sql = "SELECT * FROM Bookings";
            return await _db.QueryAsync<Booking>(sql);
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Bookings WHERE BookingId = @Id";
            return await _db.QuerySingleOrDefaultAsync<Booking>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Booking entity)
        {
            var sql = "INSERT INTO Bookings (ServiceId, CustomerId, ProviderId, BookingDate, Status) VALUES (@ServiceId, @CustomerId, @ProviderId, @BookingDate, @Status); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.QuerySingleAsync<int>(sql, entity);
        }

        public async Task<int> UpdateAsync(Booking entity)
        {
            var sql = "UPDATE Bookings SET BookingDate = @BookingDate, Status = @Status WHERE BookingId = @BookingId";
            return await _db.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Bookings WHERE BookingId = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(string customerId)
        {
            var sql = "SELECT * FROM Bookings WHERE CustomerId = @CustomerId";
            return await _db.QueryAsync<Booking>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Booking>> GetBookingsByProviderAsync(string providerId)
        {
            var sql = "SELECT * FROM Bookings WHERE ProviderId = @ProviderId";
            return await _db.QueryAsync<Booking>(sql, new { ProviderId = providerId });
        }

        public async Task UpdateStatusAsync(int bookingId, string status)
        {
            var sql = "UPDATE Bookings SET Status = @Status WHERE BookingId = @BookingId";
            await _db.ExecuteAsync(sql, new { Status = status, BookingId = bookingId });
        }
    }
}

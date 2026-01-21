using SkillHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHub.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(string customerId);
        Task<IEnumerable<Booking>> GetBookingsByProviderAsync(string providerId);
        Task UpdateStatusAsync(int bookingId, string status);
    }
}

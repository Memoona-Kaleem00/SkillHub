using SkillHub.Models;
using System.Threading.Tasks;

namespace SkillHub.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetPendingProvidersAsync();
        Task ApproveProviderAsync(string providerId);
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetTotalProvidersCountAsync();
    }
}

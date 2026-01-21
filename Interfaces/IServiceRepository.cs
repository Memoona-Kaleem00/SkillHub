using SkillHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHub.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByProviderAsync(string providerId);
        Task<IEnumerable<Service>> SearchServicesAsync(string query, int? categoryId, string sort);
    }
}

using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAscending = true, int pageNumber=1, int pageSize=1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk updateWalk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}

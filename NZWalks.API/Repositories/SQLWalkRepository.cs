using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            var walksDomainList = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            return walksDomainList;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var singleWalk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x=>x.Id == id);
            
            return singleWalk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk updateWalk)
        {
            var existingWalk = await dbContext.Walks.FindAsync(id);
            if (existingWalk == null) {
                return null;
            }

            existingWalk.Name = updateWalk.Name;
            existingWalk.Description = updateWalk.Description;
            existingWalk.LengthInKm = updateWalk.LengthInKm;
            existingWalk.WalkImageUrl = updateWalk.WalkImageUrl;
            existingWalk.DifficultyId = updateWalk.DifficultyId;
            existingWalk.RegionId = updateWalk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}

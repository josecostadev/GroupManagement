using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mapping;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly ILogger<GroupsService> _logger;
        private readonly IGroupManagementDbContext _dbContext;

        public GroupsService(ILogger<GroupsService> logger, IGroupManagementDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogTrace("### Hello from {method} ###", nameof(GetAllAsync));

            var groups = await _dbContext.Groups.AsNoTracking().OrderByDescending(o => o.Id).ToListAsync(ct);
            return groups.ToService();
        }

        public async Task<Group> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger.LogWarning("### Hello from {method} ###", nameof(GetByIdAsync));

            var group = await _dbContext.Groups.FirstOrDefaultAsync(o => o.Id == id, ct);
            return group.ToService();
        }

        public async Task<Group> UpdateAsync(Group group, CancellationToken ct)
        {
            _logger.LogWarning("### Hello from {method} ###", nameof(UpdateAsync));

            var toUpdate = _dbContext.Groups.SingleOrDefault(o => o.Id == group.Id);
            toUpdate.Name = group.Name;

            if (group.RowVersion != null)
            {
                toUpdate.RowVersion = uint.Parse(group.RowVersion);
            }

            //var updatedGroupEntry = _dbContext.Groups.Update(toUpdate);
            //await _dbContext.DataContext.SaveChangesAsync(ct);

            _dbContext.DataContext.Entry(toUpdate).OriginalValues.SetValues(new Dictionary<string, object> { { "RowVersion", toUpdate.RowVersion } });
            await _dbContext.DataContext.SaveChangesAsync(ct);

            return toUpdate.ToService();
        }

        public async Task<Group> AddAsync(Group group, CancellationToken ct)
        {
            _logger.LogWarning("### Hello from {method} ###", nameof(AddAsync));
            
            var entry = await _dbContext.Groups.AddAsync(group.ToEntity(), ct);
            await _dbContext.DataContext.SaveChangesAsync();
            return entry.Entity.ToService();
        }

        public async Task<Group> DeleteAsync(long id, CancellationToken ct)
        {
            var toDelete = _dbContext.Groups.SingleOrDefault(o => o.Id == id);

            if (toDelete == null)
                return default;

            _dbContext.Groups.Remove(toDelete);
            await _dbContext.DataContext.SaveChangesAsync(ct);
            return toDelete.ToService();
        }
    }
}

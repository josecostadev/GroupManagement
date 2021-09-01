using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class InMemoryGroupsService : IGroupsService
    {
        private readonly List<Group> _groups = new List<Group>();
        private readonly ILogger<InMemoryGroupsService> _logger;
        private long currentId = 0;

        public InMemoryGroupsService(ILogger<InMemoryGroupsService> logger)
        {
            _logger = logger;
        }

        public Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogTrace("### Hello from {method}", nameof(GetAllAsync));
            return Task.FromResult<IReadOnlyCollection<Group>>(_groups.AsReadOnly());
        }

        public async Task<Group> GetByIdAsync(long id, CancellationToken ct)
        {
            _logger.LogWarning("### Hello from {method}", nameof(GetByIdAsync));
            await Task.Delay(5000, ct);

            return _groups.SingleOrDefault(o => o.Id == id);
        }

        public Task<Group> UpdateAsync(Group group, CancellationToken ct)
        {
            var toUpdate = _groups.SingleOrDefault(o => o.Id == group.Id);

            if (toUpdate == null)
            {
                return null;
            }

            toUpdate.Name = group.Name;
            return Task.FromResult(toUpdate);
        }

        public Task<Group> AddAsync(Group group, CancellationToken ct)
        {
            group.Id = ++currentId;
            _groups.Add(group);
            return Task.FromResult(group);
        }
    }
}

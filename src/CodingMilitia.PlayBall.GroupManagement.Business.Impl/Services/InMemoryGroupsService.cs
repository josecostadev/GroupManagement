using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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

        public IReadOnlyCollection<Group> GetAll()
        {
            _logger.LogTrace("### Hello from {method}", nameof(GetAll));
            return _groups.AsReadOnly();
        }

        public Group GetById(long id)
        {
            _logger.LogWarning("### Hello from {method}", nameof(GetById));
            return _groups.SingleOrDefault(o => o.Id == id);
        }

        public Group Update(Group group)
        {
            var toUpdate = _groups.SingleOrDefault(o => o.Id == group.Id);

            if (toUpdate == null)
            {
                return null;
            }

            toUpdate.Name = group.Name;
            return toUpdate;
        }

        public Group Add(Group group)
        {
            group.Id = ++currentId;
            _groups.Add(group);
            return group;
        }
    }
}

using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class InMemoryGroupsService : IGroupsService
    {
        private readonly List<Group> _groups = new List<Group>();
        private long currentId = 0;

        public IReadOnlyCollection<Group> GetAll()
        {
            return _groups.AsReadOnly();
        }

        public Group GetById(long id)
        {
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

using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CodingMilitia.PlayBall.GroupManagement.Web.IoC
{
    public class GroupsServiceDecorator : IGroupsService
    {
        private readonly IGroupsService _inner;
        private readonly ILogger<GroupsServiceDecorator> _logger;

        public GroupsServiceDecorator(IGroupsService inner, ILogger<GroupsServiceDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public Group Add(Group group)
        {
            _logger.LogTrace("##### {decoratedMethod}", nameof(Add));
            return _inner.Add(group);
        }

        public IReadOnlyCollection<Group> GetAll()
        {
            using (var scope = _logger.BeginScope("Created scope: {decorator}", nameof(GroupsServiceDecorator)))
            {
                _logger.LogTrace("##### Hello {decoratedMethod}", nameof(GetAll));
                var result = _inner.GetAll();
                _logger.LogTrace("##### Bye {decoratedMethod}", nameof(GetAll));
                return result;
            }
        }

        public Group GetById(long id)
        {
            _logger.LogWarning("##### {decoratedMethod}", nameof(GetById));
            return _inner.GetById(id);
        }

        public Group Update(Group group)
        {
            _logger.LogTrace("##### {decoratedMethod}", nameof(Update));
            return _inner.Update(group);
        }
    }
}

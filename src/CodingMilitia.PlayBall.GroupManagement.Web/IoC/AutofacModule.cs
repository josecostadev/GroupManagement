using System;
using System.Collections.Generic;
using Autofac;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<InMemoryGroupsService>().As<IGroupsService>().SingleInstance();

            builder.RegisterDecorator<IGroupsService>((context, service) => new GroupsServiceDecorator(service), "groupsService");
            builder.RegisterType<InMemoryGroupsService>().Named<IGroupsService>("groupsService").SingleInstance();
        }

        private class GroupsServiceDecorator : IGroupsService
        {
            private readonly IGroupsService _inner;

            public GroupsServiceDecorator(IGroupsService inner)
            {
                _inner = inner;
            }

            public Group Add(Group group)
            {
                Console.WriteLine($"##### Add");
                return _inner.Add(group);
            }

            public IReadOnlyCollection<Group> GetAll()
            {
                Console.WriteLine("##### GetAll");
                return _inner.GetAll();
            }

            public Group GetById(long id)
            {
                Console.WriteLine($"##### GetById {id}");
                return _inner.GetById(id);
            }

            public Group Update(Group group)
            {
                Console.WriteLine($"##### Update");
                return _inner.Update(group);
            }
        }
    }
}

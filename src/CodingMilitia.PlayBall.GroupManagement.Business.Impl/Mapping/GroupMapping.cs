using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mapping
{
    public static class GroupMapping
    {
        public static Group ToService(this GroupEntity entity)
        {
            return entity != null ? new Group{ Id = entity.Id, Name = entity.Name, RowVersion = entity.RowVersion.ToString() } : null;
        }

        public static GroupEntity ToEntity(this Group serviceModel)
        {
            return serviceModel != null ? new GroupEntity { Id = serviceModel.Id, Name = serviceModel.Name, RowVersion = uint.Parse(serviceModel.RowVersion) } : null;
        }

        public static IReadOnlyCollection<Group> ToService(this IReadOnlyCollection<GroupEntity> entityCollection)
        {
            return entityCollection.Map<GroupEntity, Group>(ToService);
        }

    }
}

using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Mappings
{
    public static class GroupMappings
    {
        public static GroupViewModel ToViewModel(this Group serviceModel)
        {
            return serviceModel != null ? new GroupViewModel { Id = serviceModel.Id, Name = serviceModel.Name, RowVersion = serviceModel.RowVersion } : null;
        }

        public static Group ToServiceModel(this GroupViewModel viewModel)
        {
            return viewModel != null ? new Group { Id = viewModel.Id, Name = viewModel.Name, RowVersion = viewModel.RowVersion } : null;
        }

        public static IReadOnlyCollection<GroupViewModel> ToViewModel(this IReadOnlyCollection<Group> models)
        {
            if (models?.Count == 0)
            {
                return Array.Empty<GroupViewModel>();
            }

            var groups = new GroupViewModel[models.Count];
            var i = 0;
            foreach(var model in models)
            {
                groups[i] = model.ToViewModel();
                ++i;
            }

            return new ReadOnlyCollection<GroupViewModel>(groups);
        }
    }
}

using CodingMilitia.PlayBall.GroupManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Data
{
    public interface IGroupManagementDbContext
    {
        DbContext DataContext { get; }

        DbSet<GroupEntity> Groups { get; set; }
    }
}
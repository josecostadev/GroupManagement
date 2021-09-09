using CodingMilitia.PlayBall.GroupManagement.Data.Configurations;
using CodingMilitia.PlayBall.GroupManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Data
{
    public class GroupManagementDbContext : DbContext, IGroupManagementDbContext
    {
        private Guid _randomGuid;

        public GroupManagementDbContext(DbContextOptions<GroupManagementDbContext> options) : base(options)
        {
            this._randomGuid = Guid.NewGuid();
        }

        public DbContext DataContext => this;

        public DbSet<GroupEntity> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            // Configure entities

            modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());
        }

        public override string ToString()
        {
            return $"{GetType().Name} - {_randomGuid}";
        }
    }
}

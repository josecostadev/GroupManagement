using CodingMilitia.PlayBall.GroupManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Data.Configurations
{
    public class GroupEntityConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .UseNpgsqlIdentityAlwaysColumn();

            builder
                .Property(o => o.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

        }
    }
}

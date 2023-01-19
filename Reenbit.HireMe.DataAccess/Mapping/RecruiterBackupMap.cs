using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class RecruiterBackupMap : IEntityTypeConfiguration<RecruiterBackup>
    {
        public void Configure(EntityTypeBuilder<RecruiterBackup> builder)
        {
            builder.ToTable("RecruiterBackup");

            builder.HasKey(x => x.Id);
        }
    }
}

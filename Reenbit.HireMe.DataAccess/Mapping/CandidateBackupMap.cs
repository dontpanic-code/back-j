using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class CandidateBackupMap : IEntityTypeConfiguration<CandidateBackup>
    {
        public void Configure(EntityTypeBuilder<CandidateBackup> builder)
        {
            builder.ToTable("CandidatesBackup");

            builder.HasKey(x => x.Id);
        }
    }
}

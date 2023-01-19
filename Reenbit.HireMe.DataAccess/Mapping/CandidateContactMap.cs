using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class CandidateContactMap : IEntityTypeConfiguration<CandidateContact>
    {
        public void Configure(EntityTypeBuilder<CandidateContact> builder)
        {
            builder.ToTable("CandidateContacts");

            builder.HasKey(ur => new { ur.RecruiterId, ur.CandidateId });
        }
    }
}

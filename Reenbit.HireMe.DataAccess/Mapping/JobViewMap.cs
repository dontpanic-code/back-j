using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class JobViewMap : IEntityTypeConfiguration<JobView>
    {
        public void Configure(EntityTypeBuilder<JobView> builder)
        {
            builder.ToTable("JobView");

            builder.HasKey(x => x.Id);
        }
    }
}

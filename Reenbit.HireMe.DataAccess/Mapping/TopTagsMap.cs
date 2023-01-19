using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class TopTagsMap : IEntityTypeConfiguration<TopTags>
    {
        public void Configure(EntityTypeBuilder<TopTags> builder)
        {
            builder.ToTable("TopTags");

            builder.HasKey(x => x.Tags);
        }
    }
}

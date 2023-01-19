using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class BlogMap : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blog");

            builder.HasKey(x => x.Id);
        }
    }
}

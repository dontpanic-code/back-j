using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class MessagesMap : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(x => x.Id);
        }
    }
}

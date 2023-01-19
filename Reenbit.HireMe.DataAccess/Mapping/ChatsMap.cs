using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reenbit.HireMe.Domain.Entities;

namespace Reenbit.HireMe.DataAccess.Mapping
{
    class ChatsMap : IEntityTypeConfiguration<Chats>
    {
        public void Configure(EntityTypeBuilder<Chats> builder)
        {
            builder.ToTable("Chats");

            builder.HasKey(x => x.IdChat);
        }
    }
}

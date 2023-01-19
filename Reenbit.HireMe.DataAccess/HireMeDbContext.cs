using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Mapping;

namespace Reenbit.HireMe.DataAccess
{
    public class HireMeDbContext : DbContext
    {
        public HireMeDbContext(DbContextOptions<HireMeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new CandidateMap());
            modelBuilder.ApplyConfiguration(new CandidateBackupMap());
            modelBuilder.ApplyConfiguration(new CandidateContactMap());

            modelBuilder.ApplyConfiguration(new RecruiterMap());
            modelBuilder.ApplyConfiguration(new RecruiterBackupMap());

            modelBuilder.ApplyConfiguration(new ChatsMap());
            modelBuilder.ApplyConfiguration(new MessagesMap());

            modelBuilder.ApplyConfiguration(new JobMap());
            modelBuilder.ApplyConfiguration(new JobViewMap());

            modelBuilder.ApplyConfiguration(new BlogMap());
            modelBuilder.ApplyConfiguration(new TopTagsMap());
        }
    }
}

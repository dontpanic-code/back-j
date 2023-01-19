using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Infrastructure;
using System;

namespace Reenbit.HireMe.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConfigurationManager configurationManager;

        private readonly IServiceProvider serviceProvider;

        public UnitOfWorkFactory(
            IConfigurationManager configurationManager,
            IServiceProvider serviceProvider)
        {
            this.configurationManager = configurationManager;
            this.serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var dbContext = this.CreateDbContext();
            var unitOfWork = new UnitOfWork(dbContext, this.serviceProvider);

            return unitOfWork;
        }

        private DbContext CreateDbContext()
        {
            var dbConnetionString = this.configurationManager.DatabaseConnectionString;
            var dbContextOptions = new DbContextOptionsBuilder<HireMeDbContext>()
                .UseSqlServer(dbConnetionString)
                .Options;
            DbContext dbContext = new HireMeDbContext(dbContextOptions);

            return dbContext;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Reenbit.HireMe.DataAccess;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.DataAccess.Repositories;
using Reenbit.HireMe.Infrastructure;
using Reenbit.HireMe.Services;
using Reenbit.HireMe.Services.Abstraction;
using System;

namespace Reenbit.HireMe.API.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            RegisterInfrastructure(services);
            RegisterServices(services);
            RegisterDataAccess(services);

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICandidatesService, CandidatesService>();
            services.AddTransient<IRecruiterService, RecruiterService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IChatsService, ChatsService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IJobService, JobService>();

            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ITopTagsService, TopTagsService>();
        }

        public static void RegisterDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScoped<ICandidatesRepository, CandidatesRepository>();
            services.AddScoped<IRecruiterRepository, RecruiterRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatsRepository, ChatsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobViewRepository, JobViewRepository>();

            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ITopTagsRepository, TopTagsRepository>();

            //services.AddSingleton<SocketManager>();
        }
        public static void RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
        }
    }
}

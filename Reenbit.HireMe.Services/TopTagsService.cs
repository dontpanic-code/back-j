using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Reenbit.HireMe.Services
{
    public class TopTagsService : ITopTagsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public TopTagsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<List<TopTags>> GetAllPosts()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var blogRepository = uow.GetRepository<ITopTagsRepository>();
                return await blogRepository.GetAllPosts();
            }
        }
    }
}

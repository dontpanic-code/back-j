using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Add(UserDTO user)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {               
                var userRepository = uow.GetRepository<IUserRepository>();
                var isuserdb = userRepository.Find(u => u.Email == user.Email).Any();
                if (!isuserdb)
                {
                    userRepository.Add(new User
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        FullName = user.FullName,
                        LastName = user.LastName,
                        TypeUser = user.TypeUser
                    });
                    uow.SaveChanges();
                }
            }
        }
        public async Task<User> GetUser(UserDTO user)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                return await userRepository.GetUserByEmail(user.Email);
            }
        }

        public async Task DeleteUser(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                var result = await userRepository.FindAsync(c => c.Email == email);

                if (result.Count > 0)
                {
                    userRepository.Remove(result.First());
                }

                await uow.SaveChangesAsync();
            }
        }
    }
}

using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IUserService
    {
        void Add(UserDTO user);
        Task<User> GetUser(UserDTO user);
        Task DeleteUser(string email);
    }
}

using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IMessagesService
    {
        Task<List<MessagesDTO>> GetCandidatesWithPrivateInfo(string email);

        Task AddCandidate(MessagesDTO createCandidate);
    }
}

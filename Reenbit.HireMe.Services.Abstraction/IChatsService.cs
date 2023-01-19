using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IChatsService
    {
        //Task<List<CandidatePublicDTO>> GetApprovedCandidates();

        Task<List<ChatsDTO>> GetCandidatesWithPrivateInfo(string email);

        //Task<List<CandidatePublicDTO>> GetNotProcessedCandidates();

        //Task<CandidatePublicDTO> GetCandidateByUserId(string email);

        Task AddCandidate(CreateChatsDTO createCandidate);

        List<ChatsDTO> GetChatsByUserId(int id, bool isRecruter);

        //Task DeleteCandidate(string email);

        //Task ProcessCandidate(int candidateId, ProcessCandidateDTO processCandidateDTO);

        //Task<bool> RequestCandidateContacts(RequestContactsDTO requestContactsDTO, string recruiterEmail);
    }
}

using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface ICandidatesService
    {
        Task<List<CandidatePublicDTO>> GetApprovedCandidates();
        Task<List<CandidatePublicDTO>> GetApprovedCandidatesGuest();

        Task<List<CandidatePublicDTO>> GetCandidatesWithPrivateInfo();

        Task<List<CandidatePublicDTO>> GetNotProcessedCandidates();

        Task<CandidatePublicDTO> GetCandidateByUserId(string email);

        Task AddCandidate(CreateCandidateDTO createCandidate, string email);

        Task DeleteCandidate(string email);

        Task ProcessCandidate(int candidateId, ProcessCandidateDTO processCandidateDTO);

        Task<bool> RequestCandidateContacts(RequestContactsDTO requestContactsDTO, string recruiterEmail);
    }
}

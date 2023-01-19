using Reenbit.HireMe.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IRecruiterService
    {
        //Task<List<RecruiterPublicDTO>> GetApprovedRecruiter();

        Task<List<RecruiterPublicDTO>> GetRecruiterWithPrivateInfo();

        //Task<List<RecruiterPublicDTO>> GetNotProcessedRecruiter();

        Task<RecruiterPublicDTO> GetRecruiterByUserId(string email);

        Task AddRecruiter(CreateRecruiterDTO createRecruiter, string email);

        Task DeleteRecruiter(string email);

        //Task ProcessRecruiter(int candidateId, ProcessRecruiterDTO processRecruiterDTO);

        //Task<bool> RequestRecruiterContacts(RequestRecruiterDTO requestContactsDTO, string recruiterEmail);
    }
}

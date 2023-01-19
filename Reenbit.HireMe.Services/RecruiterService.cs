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
    public class RecruiterService : IRecruiterService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public RecruiterService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task AddRecruiter(CreateRecruiterDTO createRecruiter, string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);

                if (userId != 0)
                {
                    var recruiterRepository = uow.GetRepository<IRecruiterRepository>();

                    var newCandidate = new Recruiter
                    {
                        UserId = userId,
                        Position = createRecruiter.Position,
                        PositionOther = createRecruiter.PositionOther,
                        IsAnonymous = createRecruiter.IsAnonymous,
                        DateCreated = DateTime.UtcNow,
                        Company = createRecruiter.Company,
                        CompanyOther = createRecruiter.CompanyOther
                    };

                    var newCandidateBuckup = new RecruiterBackup
                    {
                        UserId = userId,
                        Position = createRecruiter.Position,
                        PositionOther = createRecruiter.PositionOther,                    
                        DateCreated = DateTime.UtcNow,
                        Company = createRecruiter.Company,
                        CompanyOther = createRecruiter.CompanyOther
                    };


                    Recruiter r = await recruiterRepository.ForUpdate(userId);
                    if (r != null)
                    {
                        r.Position = createRecruiter.Position;
                        r.PositionOther = createRecruiter.PositionOther;
                        r.Company = createRecruiter.Company;
                        r.CompanyOther = createRecruiter.CompanyOther;

                        recruiterRepository.Update(r);
                    }
                    else
                    {
                        recruiterRepository.Add(newCandidate);
                        recruiterRepository.AddRecruiterBackup(newCandidateBuckup);
                    }
                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteRecruiter(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);

                if (userId != 0)
                {
                    var candidatesRepository = uow.GetRepository<IRecruiterRepository>();

                    var result = await candidatesRepository.FindAsync(c => c.UserId == userId);
                    if (result.Count > 0)
                    {
                        candidatesRepository.Remove(result.First());
                    }

                    await uow.SaveChangesAsync();
                }
            }
        }

        //public async Task<List<RecruiterPublicDTO>> GetApprovedCandidates()
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var userRepository = uow.GetRepository<ICandidatesRepository>();
        //        var result = await userRepository.GetApproved();

        //        return result.Select(MapCandidateToGridDTO)
        //                     .OrderByDescending(u => u.Id)
        //                     .ToList();
        //    }
        //}

        //public async Task<List<CandidatePublicDTO>> GetNotProcessedCandidates()
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var userRepository = uow.GetRepository<ICandidatesRepository>();
        //        var result = await userRepository.GetNotProcessed();

        //        return result.Select(MapCandidateToGridDTO)
        //                     .OrderByDescending(u => u.Id)
        //                     .ToList();
        //    }
        //}


        public async Task<List<RecruiterPublicDTO>> GetRecruiterWithPrivateInfo()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IRecruiterRepository>();
                var result = await userRepository.GetAllAsync();

                return result.Select(MapRecruiterToAdminDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }

        public async Task<RecruiterPublicDTO> GetRecruiterByUserId(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);

                var candidatesRepository = uow.GetRepository<IRecruiterRepository>();
                var result = await candidatesRepository.FindAsync(c => c.UserId == userId);

                return result.Select(u =>
                        new RecruiterPublicDTO
                        {
                            Id = u.Id,
                            Position = u.Position,
                            PositionOther = u.PositionOther,
                            IsAnonymous = u.IsAnonymous,
                            Company = u.Company,
                            CompanyOther = u.CompanyOther

                        })
                    .FirstOrDefault();
            }
        }

        //public async Task ProcessRecruiter(int candidateId, ProcessRecruiterDTO processCandidateDTO)
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var candidatesRepository = uow.GetRepository<ICandidatesRepository>();
        //        var candidate = await candidatesRepository.GetByIdAsync(candidateId);

        //        if (candidate != null)
        //        {
        //            candidate.IsApproved = processCandidateDTO.IsApproved;

        //            if (!processCandidateDTO.IsApproved)
        //            {
        //                candidate.RejectionReason = processCandidateDTO.RejectionReason;
        //            }

        //            await uow.SaveChangesAsync();
        //        }
        //    }
        //}

        //public async Task<bool> RequestCandidateContacts(RequestContactsDTO requestContactsDTO, string recruiterEmail)
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var userRepository = uow.GetRepository<IUserRepository>();
        //        var candidatesRepository = uow.GetRepository<ICandidatesRepository>();

        //        var recruiterId = await userRepository.GetUserIdByEmail(recruiterEmail);
        //        var candidateId = await candidatesRepository.FindByKeyAndMapAsync(c => c.Id == requestContactsDTO.CandidateId, c => c.UserId);

        //        var isContactRequestExists = await candidatesRepository.IsContactRequestExists(candidateId, recruiterId);

        //        if (!isContactRequestExists)
        //        {
        //            // send email

        //            candidatesRepository.AddCandidateContactRequest(
        //                new CandidateContact
        //                {
        //                    CandidateId = candidateId,
        //                    RecruiterId = recruiterId,
        //                    Message = requestContactsDTO.Message,
        //                    RequestDate = DateTime.UtcNow
        //                });
        //            await uow.SaveChangesAsync();

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        private RecruiterPublicDTO MapRecruiterToGridDTO(Recruiter candidate)
        {
            return new RecruiterPublicDTO
            {
                Id = candidate.Id,
                Position = candidate.Position,
                PositionOther = candidate.PositionOther,
                IsAnonymous = candidate.IsAnonymous,
                Company = candidate.Company,
                CompanyOther = candidate.CompanyOther
            };
        }

        private RecruiterPublicDTO MapRecruiterToAdminDTO(Recruiter candidate)
        {
            return new RecruiterPublicDTO
            {
                Id = candidate.Id,
                Position = candidate.Position,
                PositionOther = candidate.PositionOther,
                IsAnonymous = candidate.IsAnonymous,
                Company = candidate.Company,
                CompanyOther = candidate.CompanyOther
            };
        }
    }
}

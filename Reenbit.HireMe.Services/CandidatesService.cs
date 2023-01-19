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
    public class CandidatesService : ICandidatesService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public CandidatesService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task AddCandidate(CreateCandidateDTO createCandidate, string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);
                //int userId = 1185;
                if (userId != 0)
                {
                    var candidatesRepository = uow.GetRepository<ICandidatesRepository>();

                    var newCandidate = new Candidate
                    {
                        UserId = userId,
                        EnglishLevel = createCandidate.EnglishLevel,
                        EnglishSpeaking = createCandidate.EnglishSpeaking,
                        Position = createCandidate.Position,
                        Country = createCandidate.Country,
                        City = createCandidate.City,
                        ExperienceInYears = createCandidate.ExperienceInYears,
                        ConsiderRelocation = createCandidate.ConsiderRelocation,
                        IsRemote = createCandidate.IsRemote,
                        LeadershipExperience = createCandidate.LeadershipExperience,
                        CurrentLocation = createCandidate.CurrentLocation,
                        CvUrl = createCandidate.CvUrl,
                        LinkedinUrl = createCandidate.LinkedinUrl,
                        ShowPersonalInfo = true,
                        AllSelectedCompanies = createCandidate.AllSelectedCompanies,
                        OwnNameCompany = createCandidate.OwnNameCompany,
                        IsAnonymous = createCandidate.IsAnonymous,
                        IsApproved = true,
                        DateCreated = DateTime.UtcNow,
                        Courses = createCandidate.Courses,
                        Education = createCandidate.Education,
                        
                    };

                    var newCandidateBuckup = new CandidateBackup
                    {
                        UserId = userId,
                        EnglishLevel = createCandidate.EnglishLevel,
                        EnglishSpeaking = createCandidate.EnglishSpeaking,
                        Position = createCandidate.Position,
                        ExperienceInYears = createCandidate.ExperienceInYears,
                        ConsiderRelocation = createCandidate.ConsiderRelocation,
                        IsRemote = createCandidate.IsRemote,
                        LeadershipExperience = createCandidate.LeadershipExperience,
                        CurrentLocation = createCandidate.CurrentLocation,
                        Country = createCandidate.Country,
                        City = createCandidate.City,
                        CvUrl = createCandidate.CvUrl,
                        LinkedinUrl = createCandidate.LinkedinUrl,
                        ShowPersonalInfo = true,
                        AllSelectedCompanies = createCandidate.AllSelectedCompanies,
                        OwnNameCompany = createCandidate.OwnNameCompany,
                        IsAnonymous = createCandidate.IsAnonymous,
                        IsApproved = true,                        
                        DateCreated = DateTime.UtcNow,
                        //Education = createCandidate.Education,
                        //Courses = createCandidate.Courses
                    };

                    Candidate c = await candidatesRepository.ForUpdate(userId);
                    if (c != null)
                    {
                        c.UserId = userId;
                        c.EnglishLevel = createCandidate.EnglishLevel;
                        c.EnglishSpeaking = createCandidate.EnglishSpeaking;
                        c.Position = createCandidate.Position;
                        c.Country = createCandidate.Country;
                        c.City = createCandidate.City;
                        c.ExperienceInYears = createCandidate.ExperienceInYears;
                        c.ConsiderRelocation = createCandidate.ConsiderRelocation;
                        c.IsRemote = createCandidate.IsRemote;
                        c.LeadershipExperience = createCandidate.LeadershipExperience;
                        c.CurrentLocation = createCandidate.CurrentLocation;
                        c.CvUrl = createCandidate.CvUrl;
                        c.LinkedinUrl = createCandidate.LinkedinUrl;
                        c.ShowPersonalInfo = true;
                        c.AllSelectedCompanies = createCandidate.AllSelectedCompanies;
                        c.OwnNameCompany = createCandidate.OwnNameCompany;
                        c.IsAnonymous = createCandidate.IsAnonymous;
                        c.Courses = createCandidate.Courses;
                        c.Education = createCandidate.Education;
                        c.IsApproved = true;

                        candidatesRepository.Update(c);
                    }
                    else
                    {
                        candidatesRepository.Add(newCandidate);
                        //candidatesRepository.AddCandidatesBackup(newCandidateBuckup);
                    }

                    

                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteCandidate(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);

                if (userId != 0)
                {
                    var candidatesRepository = uow.GetRepository<ICandidatesRepository>();

                    var result = await candidatesRepository.FindAsync(c => c.UserId == userId);
                    if (result.Count > 0)
                    {
                        candidatesRepository.Remove(result.First());
                    }

                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CandidatePublicDTO>> GetApprovedCandidates()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<ICandidatesRepository>();
                var result = await userRepository.GetApproved();

                return result.Select(MapCandidateToGridDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }

        public async Task<List<CandidatePublicDTO>> GetApprovedCandidatesGuest()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<ICandidatesRepository>();
                var result = await userRepository.GetApprovedGuest();

                return result.Select(MapCandidateGuestDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }

        public async Task<List<CandidatePublicDTO>> GetNotProcessedCandidates()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<ICandidatesRepository>();
                var result = await userRepository.GetNotProcessed();

                return result.Select(MapCandidateToGridDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }


        public async Task<List<CandidatePublicDTO>> GetCandidatesWithPrivateInfo()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<ICandidatesRepository>();
                var result = await userRepository.GetAllAsync();

                return result.Select(MapCandidateToAdminDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }

        public async Task<CandidatePublicDTO> GetCandidateByUserId(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                int userId = await userRepository.GetUserIdByEmail(email);

                var candidatesRepository = uow.GetRepository<ICandidatesRepository>();
                var result = await candidatesRepository.FindAsync(c => c.UserId == userId);

                return result.Select(u =>
                        new CandidatePublicDTO
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            Position = u.Position,
                            EnglishLevel = u.EnglishLevel,
                            EnglishSpeaking = u.EnglishSpeaking,
                            CurrentLocation = u.CurrentLocation,
                            Country = u.Country,
                            City = u.City,
                            LeadershipExperience = u.LeadershipExperience,
                            ConsiderRelocation = u.ConsiderRelocation,
                            IsRemote = u.IsRemote,
                            ExperienceInYears = u.ExperienceInYears,
                            LinkedinUrl = u.LinkedinUrl,
                            CvUrl = u.CvUrl,
                            AllSelectedCompanies = u.AllSelectedCompanies,
                            OwnNameCompany = u.OwnNameCompany,
                            ShowPersonalInfo = u.ShowPersonalInfo,
                            IsAnonymous = u.IsAnonymous,
                            Education = u.Education,
                            Courses = u.Courses

                        })
                    .FirstOrDefault();
            }
        }

        public async Task ProcessCandidate(int candidateId, ProcessCandidateDTO processCandidateDTO)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var candidatesRepository = uow.GetRepository<ICandidatesRepository>();
                var candidate = await candidatesRepository.GetByIdAsync(candidateId);

                if (candidate != null)
                {
                    candidate.IsApproved = processCandidateDTO.IsApproved;

                    if (!processCandidateDTO.IsApproved)
                    {
                        candidate.RejectionReason = processCandidateDTO.RejectionReason;
                    }

                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> RequestCandidateContacts(RequestContactsDTO requestContactsDTO, string recruiterEmail)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                var candidatesRepository = uow.GetRepository<ICandidatesRepository>();

                var recruiterId = await userRepository.GetUserIdByEmail(recruiterEmail);
                var candidateId = await candidatesRepository.FindByKeyAndMapAsync(c => c.Id == requestContactsDTO.CandidateId, c => c.UserId);

                var isContactRequestExists = await candidatesRepository.IsContactRequestExists(candidateId, recruiterId);

                if (!isContactRequestExists)
                {
                    // send email

                    candidatesRepository.AddCandidateContactRequest(
                        new CandidateContact
                        {
                            CandidateId = candidateId,
                            RecruiterId = recruiterId,
                            Message = requestContactsDTO.Message,
                            RequestDate = DateTime.UtcNow
                        });
                    await uow.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private CandidatePublicDTO MapCandidateToGridDTO(Candidate candidate)
        {
            return new CandidatePublicDTO
            {
                Id = candidate.Id,
                UserId = candidate.UserId,
                Position = candidate.Position,
                EnglishLevel = candidate.EnglishLevel,
                EnglishSpeaking = candidate.EnglishSpeaking,
                CurrentLocation = candidate.CurrentLocation,
                LeadershipExperience = candidate.LeadershipExperience,
                ConsiderRelocation = candidate.ConsiderRelocation,
                IsRemote = candidate.IsRemote,
                ExperienceInYears = candidate.ExperienceInYears,
                LinkedinUrl = candidate.ShowPersonalInfo ? candidate.LinkedinUrl : null,
                CvUrl = candidate.ShowPersonalInfo ? candidate.CvUrl : null,
                ShowPersonalInfo = candidate.ShowPersonalInfo,
                Country = candidate.Country,
                City = candidate.City,
                AllSelectedCompanies = candidate.AllSelectedCompanies,
                IsAnonymous = candidate.IsAnonymous,
                OwnNameCompany = candidate.OwnNameCompany,
                Education = candidate.Education,
                Courses = candidate.Courses,
            };
        }

        private CandidatePublicDTO MapCandidateGuestDTO(Candidate candidate)
        {
            return new CandidatePublicDTO
            {
                Id = candidate.Id,
                Position = candidate.Position,
                EnglishLevel = candidate.EnglishLevel,
                EnglishSpeaking = candidate.EnglishSpeaking,
                CurrentLocation = candidate.CurrentLocation,
                LeadershipExperience = candidate.LeadershipExperience,
                ConsiderRelocation = candidate.ConsiderRelocation,
                IsRemote = candidate.IsRemote,
                ExperienceInYears = candidate.ExperienceInYears,  
                Country = candidate.Country,
                City = candidate.City,
                Education = candidate.Education,
                Courses = candidate.Courses,                
            };
        }

        private CandidatePublicDTO MapCandidateToAdminDTO(Candidate candidate)
        {
            return new CandidatePublicDTO
            {
                Id = candidate.Id,
                UserId = candidate.UserId,
                Position = candidate.Position,
                EnglishLevel = candidate.EnglishLevel,
                EnglishSpeaking = candidate.EnglishSpeaking,
                CurrentLocation = candidate.CurrentLocation,                
                LeadershipExperience = candidate.LeadershipExperience,
                ConsiderRelocation = candidate.ConsiderRelocation,
                IsRemote = candidate.IsRemote,
                ExperienceInYears = candidate.ExperienceInYears,
                LinkedinUrl = candidate.LinkedinUrl,
                CvUrl = candidate.CvUrl,
                ShowPersonalInfo = candidate.ShowPersonalInfo,
                Country = candidate.Country,
                City = candidate.City,
                AllSelectedCompanies = candidate.AllSelectedCompanies,
                IsAnonymous = candidate.IsAnonymous,
                OwnNameCompany = candidate.OwnNameCompany,
                Education = candidate.Education,
                Courses = candidate.Courses,
            };
        }
    }
}

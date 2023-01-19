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
    public class ChatsService : IChatsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ChatsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task AddCandidate(CreateChatsDTO createCandidate)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IChatsRepository>();
                var isuserdb = await userRepository.GetChatByEmailId(createCandidate.CurrentEmail, createCandidate.Id);

                //int userId = isuserdb;

                if (isuserdb==0)
                {
                    var candidatesRepository = uow.GetRepository<IChatsRepository>();

                    var newCandidate = new Chats
                    {
                        DisplayName = createCandidate.DisplayName,
                        Id = createCandidate.Id,
                        TotalUnreadMessages = createCandidate.TotalUnreadMessages,
                        CurrentUserId = createCandidate.CurrentUserId,
                        CurrentName = createCandidate.CurrentName,
                        CurrentUnread = createCandidate.CurrentUnread,
                        CurrentEmail = createCandidate.CurrentEmail,
                    };

                    candidatesRepository.Add(newCandidate);
                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateUnreadCount(int fromId, int toId, bool isRecruiter)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IChatsRepository>();

                if (isRecruiter)
                {
                    var result = await userRepository.ForUpdateUnread(fromId, toId);
                    result.TotalUnreadMessages += 1;
                    userRepository.Update(result);
                    await uow.SaveChangesAsync();
                }
                else
                {
                    var result = await userRepository.ForUpdateUnread(toId, fromId);
                    result.CurrentUnread += 1;
                    userRepository.Update(result);
                    await uow.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateReadCount(int fromId, int toId, bool isRecruiter)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IChatsRepository>();

                if (isRecruiter)
                {
                    var result = await userRepository.ForUpdateUnread(fromId, toId);
                    result.CurrentUnread = (int)0;
                    userRepository.Update(result);
                    await uow.SaveChangesAsync();
                }
                else
                {
                    var result = await userRepository.ForUpdateUnread(toId, fromId);
                    result.TotalUnreadMessages = (int)0;
                    userRepository.Update(result);
                    await uow.SaveChangesAsync();
                }
            }
        }

        //public async Task DeleteCandidate(string email)
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var userRepository = uow.GetRepository<IUserRepository>();
        //        int userId = await userRepository.GetUserIdByEmail(email);

        //        if (userId != 0)
        //        {
        //            var candidatesRepository = uow.GetRepository<ICandidatesRepository>();

        //            var result = await candidatesRepository.FindAsync(c => c.UserId == userId);
        //            if (result.Count > 0)
        //            {
        //                candidatesRepository.Remove(result.First());
        //            }

        //            await uow.SaveChangesAsync();
        //        }
        //    }
        //}


        public async Task<List<ChatsDTO>> GetCandidatesWithPrivateInfo(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository1 = uow.GetRepository<IUserRepository>();
                int userId = await userRepository1.GetUserIdByEmail(email);

                var userRepository = uow.GetRepository<IChatsRepository>();
                var result = await userRepository.GetChatsById(userId);

                return result.Select(MapChatsToAdminDTO)
                             .OrderByDescending(u => u.IdChat)
                             .ToList();
                             //.Where(c => c.Id == userId)
            }
        }

        public List<ChatsDTO> GetChatsByUserId(int id, bool isRecruter)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = uow.GetRepository<IChatsRepository>();
                var result = userRepository.GetChatsByIdCopy(id);

                //return result;
                if (isRecruter)
                {
                    return result.Select(MapChatsToAdminDTO)
                             .OrderByDescending(u => u.CurrentUnread)
                             .ToList();
                }
                else
                {
                    return result.Select(MapChatsToAdminDTO)
                             .OrderByDescending(u => u.TotalUnreadMessages)
                             .ToList();
                }
            }
        }

        //public async Task<CandidatePublicDTO> GetCandidateByUserId(string email)
        //{
        //    using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
        //    {
        //        var userRepository = uow.GetRepository<IUserRepository>();
        //        int userId = await userRepository.GetUserIdByEmail(email);

        //        var candidatesRepository = uow.GetRepository<ICandidatesRepository>();
        //        var result = await candidatesRepository.FindAsync(c => c.UserId == userId);

        //        return result.Select(u =>
        //                new CandidatePublicDTO
        //                {
        //                    Id = u.Id,
        //                    Position = u.Position,
        //                    EnglishLevel = u.EnglishLevel,
        //                    CurrentLocation = u.CurrentLocation,
        //                    Country = u.Country,
        //                    City = u.City,
        //                    LeadershipExperience = u.LeadershipExperience,
        //                    ConsiderRelocation = u.ConsiderRelocation,
        //                    IsRemote = u.IsRemote,
        //                    ExperienceInYears = u.ExperienceInYears,
        //                    LinkedinUrl = u.LinkedinUrl,
        //                    CvUrl = u.CvUrl,
        //                    AllSelectedCompanies = u.AllSelectedCompanies,
        //                    OwnNameCompany = u.OwnNameCompany,
        //                    ShowPersonalInfo = u.ShowPersonalInfo,
        //                    IsAnonymous = u.IsAnonymous

        //                })
        //            .FirstOrDefault();
        //    }
        //}

        //private CandidatePublicDTO MapCandidateToGridDTO(Candidate candidate)
        //{
        //    return new CandidatePublicDTO
        //    {
        //        Id = candidate.Id,
        //        Position = candidate.Position,
        //        EnglishLevel = candidate.EnglishLevel,
        //        CurrentLocation = candidate.CurrentLocation,
        //        LeadershipExperience = candidate.LeadershipExperience,
        //        ConsiderRelocation = candidate.ConsiderRelocation,
        //        IsRemote = candidate.IsRemote,
        //        ExperienceInYears = candidate.ExperienceInYears,
        //        LinkedinUrl = candidate.ShowPersonalInfo ? candidate.LinkedinUrl : null,
        //        CvUrl = candidate.ShowPersonalInfo ? candidate.CvUrl : null,
        //        ShowPersonalInfo = candidate.ShowPersonalInfo,
        //        Country = candidate.Country,
        //        City = candidate.City,
        //        AllSelectedCompanies = candidate.AllSelectedCompanies,
        //        IsAnonymous = candidate.IsAnonymous,
        //        OwnNameCompany = candidate.OwnNameCompany
        //    };
        //}

        private ChatsDTO MapChatsToAdminDTO(Chats candidate)
        {
            return new ChatsDTO
            {
                Id = candidate.Id,
                IdChat = candidate.IdChat,
                DisplayName = candidate.DisplayName,
                TotalUnreadMessages = candidate.TotalUnreadMessages,
                CurrentUserId = candidate.CurrentUserId,
                CurrentName = candidate.CurrentName,
                CurrentUnread = candidate.CurrentUnread,
                CurrentEmail = candidate.CurrentEmail,
            };
        }
    }
}

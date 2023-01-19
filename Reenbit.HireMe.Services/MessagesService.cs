using MimeKit;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MailKit.Security;

//using System.Net;
//using System.Net.Mail;

namespace Reenbit.HireMe.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public MessagesService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task AddCandidate(MessagesDTO createCandidate)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var candidatesRepository = uow.GetRepository<IMessagesRepository>();

                var chats = uow.GetRepository<IChatsRepository>();
                var users = uow.GetRepository<IUserRepository>();
                Chats c = await chats.ForUpdateUnread(createCandidate.FromId, createCandidate.ToId);
                Chats c2 = await chats.ForUpdateUnread(createCandidate.ToId, createCandidate.FromId);
                User u = await users.GetUserBId(createCandidate.ToId); 

                var newCandidate = new Messages
                {
                    Type = createCandidate.Type,
                    FromId = createCandidate.FromId,
                    ToId = createCandidate.ToId,
                    Message = createCandidate.Message,
                    DateSent = DateTime.UtcNow,
                    DateSeen = DateTime.UtcNow
                };

                candidatesRepository.Add(newCandidate);

                await uow.SaveChangesAsync();

                try
                {
                    if (createCandidate.FromId == c.CurrentUserId)
                    {
                        //from recruiter 
                        MailNotification(u.FirstName, u.Email, c.CurrentName, createCandidate.Message);
                    }
                }
                catch
                {
                    if (createCandidate.FromId == c2.Id)
                    {
                        //from seeker
                        MailNotification(u.FirstName, u.Email, c2.DisplayName, createCandidate.Message);
                    }

                }

                //MailNotification(c.CurrentName, createCandidate.FromId, createCandidate.ToId);
            }
        }


        public async Task<List<MessagesDTO>> GetCandidatesWithPrivateInfo(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository1 = uow.GetRepository<IUserRepository>();
                int userId = await userRepository1.GetUserIdByEmail(email);

                var userRepository = uow.GetRepository<IMessagesRepository>();
                var result = await userRepository.GetChatsById(userId);

                return result.Select(MapChatsToAdminDTO)
                             .OrderBy(u => u.DateSent)
                             .ToList();
            }
        }

        private MessagesDTO MapChatsToAdminDTO(Messages candidate)
        {
            return new MessagesDTO
            {
                Type = candidate.Type,
                Message = candidate.Message,
                DateSeen = candidate.DateSeen,
                DateSent = candidate.DateSent,
                FromId = candidate.FromId,
                ToId = candidate.ToId
            };
        }

        private void MailNotification(string name, string email, string nameFrom, string text)
        {
            //string name = "Anastasiia", mail = "nastia.iskandarova@gmail.com"; 
           
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rozrobnyk", "rozrobnyk@devorld.studio"));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "You have a new message";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey " + name + @",
You have received a new message from "+ nameFrom + @":

"+ text+ @"

---Go to the website to reply: https://rozrobnyk.com/"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("mail.adm.tools", 465, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("rozrobnyk@devorld.studio", "-g;9y4CeCZ^6");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}

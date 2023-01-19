using Microsoft.AspNetCore.SignalR;
using Reenbit.HireMe.API.Models;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Services;
using Reenbit.HireMe.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API
{
    public class GroupChatHub : Hub
    {
        private IUnitOfWorkFactory unitOfWorkFactory;

        private static List<ParticipantResponseViewModel> AllConnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
        private static List<ParticipantResponseViewModel> DisconnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
        private static List<GroupChatParticipantViewModel> AllGroupParticipants { get; set; } = new List<GroupChatParticipantViewModel>();
        private object ParticipantsConnectionLock = new object();
        private List<ParticipantResponseViewModel> Friends { get; set; } = new List<ParticipantResponseViewModel>();
        private static List<FriendsViewModel> AllChatsFriends { get; set; } = new List<FriendsViewModel>();

        public GroupChatHub(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        private static IEnumerable<ParticipantResponseViewModel> FilteredGroupParticipants(string currentUserId)
        {
            //return AllConnectedParticipants
            //    .Where(p => p.Participant.ParticipantType == ChatParticipantTypeEnum.User
            //           || AllGroupParticipants.Any(g => g.Id == p.Participant.Id && g.ChattingTo.Any(u => u.Id == currentUserId))
            //    );

            var chatsIndex = AllChatsFriends.FindIndex(x => x.Id == currentUserId);
            var participant = AllChatsFriends.ElementAt(chatsIndex);
            return participant.Friends;
        }

        public static IEnumerable<ParticipantResponseViewModel> ConnectedParticipants(string currentUserId)
        {
            return FilteredGroupParticipants(currentUserId).Where(x => x.Participant.Id != currentUserId);
        }

        public void ChangeStatus(string userId)
        {
            (from p in AllChatsFriends
             from item in p.Friends
             where item.Participant.UserId == userId
             select item.Participant
            ).ToList().ForEach(s => { s.Id = Context.ConnectionId; s.Status = 0; }); 
        }

        public void DisconnectStatus(string userId)
        {
            (from p in AllChatsFriends
             from item in p.Friends
             where item.Participant.UserId == userId
             select item.Participant
            ).ToList().ForEach(s => s.Id = userId);
        }

        public string GetConnectedFriendStatus(string userId)
        {
            var connectionIndex = AllConnectedParticipants.FindIndex(x => x.Participant.UserId == userId);
            if (connectionIndex >= 0)
            { 
                var participant = AllConnectedParticipants.ElementAt(connectionIndex);
                Clients.All.SendAsync("updateListFriends", participant.Participant.Id);
                return participant.Participant.Id;
            }
            else
            {
                return userId;
            }
        }

        public void GetChatsFromDB(string id, bool isRecruiter)
        {
            //this.Friends.Clear();
            //*** get user chats from DB
            ChatsService chatsService = new ChatsService(this.unitOfWorkFactory);
            int tmpId = Convert.ToInt32(id);
            var result = chatsService.GetChatsByUserId(tmpId, isRecruiter);
            result.ForEach(elem =>
            {
                if (isRecruiter == true)
                {
                    if (elem.CurrentUserId == tmpId)
                    {
                        this.Friends.Add(new ParticipantResponseViewModel()
                        {
                            Metadata = new ParticipantMetadataViewModel()
                            {
                                TotalUnreadMessages = elem.CurrentUnread,
                            },
                            Participant = new ChatParticipantViewModel()
                            {
                                DisplayName = elem.DisplayName,
                                Id = GetConnectedFriendStatus(elem.Id.ToString()),
                                UserId = elem.Id.ToString(),
                            }
                        });
                    }
                }
                else
                {
                    if (elem.Id == tmpId)
                    {
                        this.Friends.Add(new ParticipantResponseViewModel()
                        {
                            Metadata = new ParticipantMetadataViewModel()
                            {
                                TotalUnreadMessages = elem.TotalUnreadMessages,
                            },
                            Participant = new ChatParticipantViewModel()
                            {
                                DisplayName = elem.CurrentName,
                                Id = GetConnectedFriendStatus(elem.CurrentUserId.ToString()),
                                UserId = elem.CurrentUserId.ToString(),
                            }
                        });
                    }
                }

                var r = new FriendsViewModel
                {
                    Friends = this.Friends,
                    Id = Context.ConnectionId,
                    IdUser = id
                };
                var chatsIndex = AllChatsFriends.FindIndex(x => x.Id == Context.ConnectionId);
                //var participant = AllChatsFriends.ElementAt(chatsIndex);
                AllChatsFriends[chatsIndex] = r; 

                //AllChatsFriends.Add(r);
                //ChangeStatus(id);
                //Clients.All.SendAsync("messageReceived", this.Friends);
                Clients.Caller.SendAsync("friendsListChanged", this.Friends);
            });
            //*** end get user chats from DB
        }

        public void Join(string userName, string id, bool isRecruiter)
        {
            lock (ParticipantsConnectionLock)
            {
                AllConnectedParticipants.Add(new ParticipantResponseViewModel()
                {
                    Metadata = new ParticipantMetadataViewModel()
                    {
                        TotalUnreadMessages = 0,
                    },
                    Participant = new ChatParticipantViewModel()
                    {
                        DisplayName = userName,
                        Id = Context.ConnectionId,
                        UserId = id,
                        Status = 0
                    }
                });
                Clients.Caller.SendAsync("generatedUserId", Context.ConnectionId);

                //Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);

                //*** get user chats from DB
                ChatsService chatsService = new ChatsService(this.unitOfWorkFactory);
                int tmpId = Convert.ToInt32(id);
                var result = chatsService.GetChatsByUserId(tmpId, isRecruiter);
                result.ForEach(elem =>
                {
                    if (isRecruiter == true)
                    {
                        if (elem.CurrentUserId == tmpId)
                        {
                            string tmpUserId = GetConnectedFriendStatus(elem.Id.ToString()); 
                            this.Friends.Add(new ParticipantResponseViewModel()
                            {
                                Metadata = new ParticipantMetadataViewModel()
                                {
                                    TotalUnreadMessages = elem.CurrentUnread,
                                },
                                Participant = new ChatParticipantViewModel()
                                {
                                    DisplayName = elem.DisplayName,
                                    Id = tmpUserId,
                                    UserId = elem.Id.ToString(),
                                    Status = tmpUserId != elem.Id.ToString() ? 0 : 3
                                }
                            });
                        }
                    }
                    else
                    {
                        if (elem.Id == tmpId)
                        {
                            string tmpUserId = GetConnectedFriendStatus(elem.Id.ToString());
                            this.Friends.Add(new ParticipantResponseViewModel()
                            {
                                Metadata = new ParticipantMetadataViewModel()
                                {
                                    TotalUnreadMessages = elem.TotalUnreadMessages,
                                },
                                Participant = new ChatParticipantViewModel()
                                {
                                    DisplayName = elem.CurrentName,
                                    Id = GetConnectedFriendStatus(elem.CurrentUserId.ToString()),
                                    UserId = elem.CurrentUserId.ToString(),
                                    Status = tmpUserId != elem.Id.ToString() ? 0 : 3
                                }
                            });
                        }
                    }
                        
                    var r = new FriendsViewModel
                    {
                        Friends = this.Friends,
                        Id = Context.ConnectionId,
                        IdUser = id
                    };
                    AllChatsFriends.Add(r);
                    //ChangeStatus(id);
                    //Clients.All.SendAsync("messageReceived", this.Friends);
                    Clients.Caller.SendAsync("friendsListChanged", this.Friends);

                    
                });
                //*** end get user chats from DB
                ChangeStatus(id);
                Clients.All.SendAsync("updateListFriends", "need update list friends");
            }
        }

        public void SendMessage(MessageViewModel message, bool isRecruiter)
        {

            var sender = AllConnectedParticipants.Find(x => x.Participant.Id == message.FromId);
            var recipient = AllConnectedParticipants.Find(x => x.Participant.Id == message.ToId);

            if (sender != null)
            {
                var m = new MessagesDTO
                {
                    DateSent = message.DateSent,
                    DateSeen = message.DateSeen,
                    FromId = Convert.ToInt32(sender.Participant.UserId),
                    ToId = recipient != null? Convert.ToInt32(recipient.Participant.UserId) : Convert.ToInt32(message.ToId),
                    Message = message.Message,
                    Type = message.Type
                };

                ChatsService chatsService = new ChatsService(unitOfWorkFactory);
                chatsService.UpdateUnreadCount(m.FromId, m.ToId, isRecruiter); 

                MessagesService candidatesService = new MessagesService(unitOfWorkFactory);
                candidatesService.AddCandidate(m);

                //Clients.All.SendAsync("messageReceived", sender.Participant, message);
                Clients.All.SendAsync("updateListFriends", isRecruiter);
                Clients.Client(message.ToId).SendAsync("messageReceived", sender.Participant, message);

            }
        }

        public void SetMessagesRead(string _id, string _toId, bool isRecruiter)
        {
            int id = Convert.ToInt32(_id);
            int toId = Convert.ToInt32(_toId);

            ChatsService chatsService = new ChatsService(unitOfWorkFactory);
            chatsService.UpdateReadCount(id, toId, isRecruiter);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            lock (ParticipantsConnectionLock)
            {
                var connectionIndex = AllConnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

                if (connectionIndex >= 0)
                {
                    var participant = AllConnectedParticipants.ElementAt(connectionIndex);
                   

                    AllConnectedParticipants.Remove(participant);
                    DisconnectedParticipants.Add(participant);

                    DisconnectStatus(participant.Participant.UserId);

                    Clients.All.SendAsync("updateListFriends", "updateListFriends");
                }

                return base.OnDisconnectedAsync(exception);
            }
        }

        public override Task OnConnectedAsync()
        {
            // если userId уже есть в списке AllConnectedParticipants, то нужно удалить его и добавить заново (чтобы обновился Context.ConnectionId) 
            lock (ParticipantsConnectionLock)
            {
                var connectionIndex = DisconnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

                if (connectionIndex >= 0)
                {
                    var participant = DisconnectedParticipants.ElementAt(connectionIndex);

                    DisconnectedParticipants.Remove(participant);
                    AllConnectedParticipants.Add(participant);

                    Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
                }

                return base.OnConnectedAsync();
            }
        }
    }
}

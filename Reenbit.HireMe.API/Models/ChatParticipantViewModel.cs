using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API.Models
{
    public class ChatParticipantViewModel
    {
        public ChatParticipantTypeEnum ParticipantType { get; set; }
        public string Id { get; set; }
        public int Status { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public string UserId { get; set; }
    }
}

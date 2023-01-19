using System;
using System.Collections.Generic;
using System.Text;

namespace Reenbit.HireMe.Domain.DTOs
{
    public class CreateChatsDTO
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int TotalUnreadMessages { get; set; }

        public int CurrentUserId { get; set; }

        public string CurrentName { get; set; }

        public int CurrentUnread { get; set; }

        public string CurrentEmail { get; set; }

    }
}

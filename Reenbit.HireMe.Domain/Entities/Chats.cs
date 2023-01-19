namespace Reenbit.HireMe.Domain.Entities
{
    public class Chats
    {
        public int IdChat { get; set; }

        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int TotalUnreadMessages { get; set; }

        public int CurrentUserId { get; set; }

        public string CurrentName { get; set; }

        public int CurrentUnread { get; set; }

        public string CurrentEmail { get; set; }
    }
}

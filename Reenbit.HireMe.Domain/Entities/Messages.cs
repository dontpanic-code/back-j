using System;
namespace Reenbit.HireMe.Domain.Entities
{
    public class Messages
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int FromId { get; set; }

        public int ToId { get; set; }

        public string Message { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime? DateSeen { get; set; }
    }
}

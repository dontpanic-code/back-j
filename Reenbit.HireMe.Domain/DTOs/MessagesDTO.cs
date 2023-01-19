using System;
using System.Collections.Generic;
using System.Text;

namespace Reenbit.HireMe.Domain.DTOs
{
    public class MessagesDTO
    {
        public int Type { get; set; }

        public int FromId { get; set; }

        public int ToId { get; set; }

        public string Message { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime? DateSeen { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API.Models
{
    public class MessageViewModel
    {
        public int Type { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Message { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateSeen { get; set; }
        public string DownloadUrl { get; set; }
        public int? FileSizeInBytes { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
    }
}

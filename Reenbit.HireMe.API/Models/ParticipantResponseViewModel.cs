using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API.Models
{
    public class ParticipantResponseViewModel
    {
        public ChatParticipantViewModel Participant { get; set; }
        public ParticipantMetadataViewModel Metadata { get; set; }
    }
}

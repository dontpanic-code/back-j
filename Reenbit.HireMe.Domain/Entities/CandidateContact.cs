using System;

namespace Reenbit.HireMe.Domain.Entities
{
    public class CandidateContact
    {
        public int RecruiterId { get; set; }

        public int CandidateId { get; set; }

        public string Message { get; set; }

        public DateTime RequestDate { get; set; }
    }
}

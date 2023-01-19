using System;

namespace Reenbit.HireMe.Domain.Entities
{
    public class Recruiter
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Position { get; set; }

        public string PositionOther { get; set; }

        public bool IsAnonymous { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Company { get; set; }

        public string CompanyOther { get; set; }
    }
}

using System;

namespace Reenbit.HireMe.Domain.Entities
{
    public class Candidate
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Position { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public bool LeadershipExperience { get; set; }

        public string CurrentLocation { get; set; }

        public int ExperienceInYears { get; set; }

        public string EnglishLevel { get; set; }

        public string EnglishSpeaking { get; set; }

        public bool ConsiderRelocation { get; set; }

        public bool IsRemote { get; set; }

        public string LinkedinUrl { get; set; }

        public string CvUrl { get; set; }

        public bool? IsApproved { get; set; }

        public string RejectionReason { get; set; }

        public bool ShowPersonalInfo { get; set; }

        public string AllSelectedCompanies { get; set; }

        public string OwnNameCompany { get; set; }

        public bool IsAnonymous { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool Education { get; set; }

        public bool Courses { get; set; }
    }
}

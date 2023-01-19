using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Reenbit.HireMe.DataAccess.Abstraction
{
    public class CandidatesFilterRequest
    {
        public CandidatesFilterRequest()
        {
            this.Positions = new List<string>();
            this.Locations = new List<string>();
        }

        //[FromQuery]
        public ICollection<string> Positions { get; set; }

        //[FromQuery]
        public bool? LeadershipExperience { get; set; }

        //[FromQuery]
        public ICollection<string> Locations { get; set; }

        //[FromQuery]
        public string ExperienceInYears { get; set; }

        //[FromQuery]
        public string EnglishLevel { get; set; }

        //[FromQuery]
        public bool? ConsiderRelocation { get; set; }
    }
}

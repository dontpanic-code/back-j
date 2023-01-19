using System;

namespace Reenbit.HireMe.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string AboutProject { get; set; }
        public string JobRequirements { get; set; }
        public string Stack { get; set; }
        public string StagesInterview { get; set; }
        public string EnglishLevel { get; set; }
        public string SalaryRange { get; set; }
        public string WorkplaceType { get; set; }
        public string EmploymentType { get; set; }
        public string Benefits { get; set; }
        public string Contacts { get; set; }
        public bool IsApproved { get; set; }
        public string? DateCreated { get; set; }
        public string ContactType { get; set; }
        public string ContactLink { get; set; }
        public string Tags { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Experience { get; set; }
    }
}

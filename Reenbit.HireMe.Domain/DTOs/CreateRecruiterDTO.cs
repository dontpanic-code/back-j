namespace Reenbit.HireMe.Domain.DTOs
{
    public class CreateRecruiterDTO
    {
        public string Position { get; set; }

        public string PositionOther { get; set; }

        public bool IsAnonymous { get; set; }

        public string Company { get; set; }

        public string CompanyOther { get; set; }

    }
}

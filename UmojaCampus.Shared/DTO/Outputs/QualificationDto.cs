

using System;

namespace UmojaCampus.Shared.DTO.Outputs
{
    public class QualificationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Duration { get; set; }
        public int TotalCredit { get; set; }
        public decimal Fees { get; set; }
        public string CoverImage { get; set; }
    }
}

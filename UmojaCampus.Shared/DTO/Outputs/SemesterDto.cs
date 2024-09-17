using System;

namespace UmojaCampus.Shared.DTO.Outputs
{
    public class SemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}

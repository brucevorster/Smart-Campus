using AutoMapper;
using UmojaCampus.Business.Entities;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;

namespace UmojaCampus.API.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Entity to Dto
            CreateMap<Qualification, QualificationDto>().ReverseMap();
            CreateMap<Semester, SemesterDto>().ReverseMap();



            //Dto to Entity
            CreateMap<SaveQualificationDto, Qualification>();
            CreateMap<SaveSemesterDto, Semester>();

            
        }
    }
}

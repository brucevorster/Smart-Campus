using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmojaCampus.Business.Entities;
using UmojaCampus.Business.Persistence.Contexts;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;
using UmojaCampus.Shared.Resources;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Implementations
{
    public class SemesterService(ApplicationDbContext context, IMapper mapper) : ISemesterService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<SemesterDto>> AddAsync(SaveSemesterDto dto, CurrentUser user)
        {   
            if(dto.DateTo <= dto.DateFrom)
            {
                return await Result<SemesterDto>.FailAsync(ErrorResource.InvalidDates);
            }

            var semester = _mapper.Map<Semester>(dto);
            await _context.Semesters.AddAsync(semester);
            await _context.SaveChangesAsync(user);

            var semesterDto = _mapper.Map<SemesterDto>(semester);
            return await Result<SemesterDto>.SuccessAsync(semesterDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var semester = await _context
                .Semesters.FindAsync(id);

            if (semester is null)
            {
                return await Result<string>.FailAsync(ErrorResource.ResourceNotFound);
            }

            semester.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await Result<string>.SuccessAsync();
        }

        public async Task<Result<SemesterDto>> FindByIdAsync(Guid id)
        {
            var semester = await _context
                .Semesters.FindAsync(id);

            if (semester is null)
            {
                return Result<SemesterDto>.Fail(ErrorResource.NoResultFound);
            }

            var semesterDto = _mapper.Map<SemesterDto>(semester);
            return await Result<SemesterDto>.SuccessAsync(semesterDto);
        }

        public async Task<Result<IEnumerable<SemesterDto>>> GetAllAsync()
        {
            var semesters = await _context
                .Semesters.ToListAsync();

            var data = _mapper.Map<IEnumerable<SemesterDto>>(semesters);

            return await Result<IEnumerable<SemesterDto>>.SuccessAsync(data);
        }

        public async Task<Result<SemesterDto>> UpdateAsync(Guid id, SaveSemesterDto dto, CurrentUser user)
        {
           
            var existingSemester = await _context
                .Semesters.FindAsync(id);

            if (existingSemester is null)
            {
                return await Result<SemesterDto>.FailAsync(ErrorResource.ResourceNotFound);
            }
           
            _context.Entry(existingSemester)
                .CurrentValues.SetValues(dto);

            await _context.SaveChangesAsync(user);

            var data = _mapper.Map<SemesterDto>(existingSemester);

            return await Result<SemesterDto>.SuccessAsync(data);

        }


    }
}

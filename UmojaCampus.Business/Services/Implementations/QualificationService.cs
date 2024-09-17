using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UmojaCampus.Business.Entities;
using UmojaCampus.Business.Extensions;
using UmojaCampus.Business.Persistence.Contexts;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;
using UmojaCampus.Shared.Helpers;
using UmojaCampus.Shared.RequestFeatures;
using UmojaCampus.Shared.Resources;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Implementations
{
    public class QualificationService(ApplicationDbContext context,
        IMapper mapper) : IQualificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<Result<QualificationDto>> AddAsync(SaveQualificationDto dto, CurrentUser user)
        {
            if(dto.ToDate <= dto.FromDate)
            {
                return await Result<QualificationDto>.FailAsync(ErrorResource.InvalidDates);
            }

            var qualification = _mapper.Map<Qualification>(dto);

            qualification.Duration = StringHelper.CalculateDuration(dto.FromDate, dto.ToDate);
            
            await _context.Qualifications.AddAsync(qualification);
            await _context.SaveChangesAsync(user);

            var qualificationDto = _mapper.Map<QualificationDto>(qualification);
            return await Result<QualificationDto>.SuccessAsync(qualificationDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var qualification = await _context
                .Qualifications.FindAsync(id);

            if (qualification == null)
            {
                return await Result<string>.FailAsync(ErrorResource.ResourceNotFound);
            }

            qualification.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await Result<string>.SuccessAsync();
        }

        public async Task<Result<QualificationDto>> FindByIdAsync(Guid id)
        {
            var qualification = await _context
                .Qualifications.FindAsync(id);

            if(qualification == null)
            {
                return Result<QualificationDto>.Fail(ErrorResource.NoResultFound);
            }

            var qualificationDto = _mapper.Map<QualificationDto>(qualification);    
            return await Result<QualificationDto>.SuccessAsync(qualificationDto);
        }

        public async Task<Result<IEnumerable<QualificationDto>>> GetAllAsync()
        {
            var qualifications = await _context
                .Qualifications.ToListAsync();

            var data = _mapper.Map<IEnumerable<QualificationDto>>(qualifications);

            return await Result<IEnumerable<QualificationDto>>.SuccessAsync(data);
        }

        public async Task<PaginatedResult<QualificationDto>> GetPaginatedResultAsync(QueryParameters parameters)
        {
            var query = _context.Qualifications.AsQueryable();
            var filter = parameters.Filter;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.Name.Contains(filter) ||
                            x.Description.Contains(filter));
            }

            var result = await query.OrderBy(x => x.Name).ToPaginatedListAsync(parameters.PageNumber, parameters.PageSize);

            var mappedResult = _mapper.Map<List<QualificationDto>>(result.Data);
            return PaginatedResult<QualificationDto>.Success(mappedResult, result.TotalCount, result.CurrentPage, result.PageSize);
        }

        public async Task<Result<QualificationDto>> UpdateAsync(Guid id, SaveQualificationDto dto, CurrentUser user)
        {
            if (dto.ToDate <= dto.FromDate)
            {
                return await Result<QualificationDto>.FailAsync(ErrorResource.InvalidDates);
            }

            var existingQualification = await _context
                .Qualifications.FindAsync(id);

            if(existingQualification == null)
            {
                return await Result<QualificationDto>.FailAsync(ErrorResource.ResourceNotFound);
            }

            existingQualification.Duration = StringHelper.CalculateDuration(dto.FromDate, dto.ToDate);
            _context.Entry(existingQualification)
                .CurrentValues.SetValues(dto);

            await _context.SaveChangesAsync(user);

            var data = _mapper.Map<QualificationDto>(existingQualification);
            return await Result<QualificationDto>.SuccessAsync(data);
        }
    }
}

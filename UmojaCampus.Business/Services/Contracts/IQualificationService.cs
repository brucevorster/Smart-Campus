using UmojaCampus.Business.Entities;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;
using UmojaCampus.Shared.RequestFeatures;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Contracts
{
    public interface IQualificationService
    {
        Task<Result<QualificationDto>> AddAsync(SaveQualificationDto dto, CurrentUser user);
        Task<Result<QualificationDto>> UpdateAsync(Guid id, SaveQualificationDto dto, CurrentUser user);
        Task<Result<QualificationDto>> FindByIdAsync(Guid id);
        Task<Result<IEnumerable<QualificationDto>>> GetAllAsync();
        Task<PaginatedResult<QualificationDto>> GetPaginatedResultAsync(QueryParameters parameters);
        Task<Result> DeleteAsync(Guid id);
    }
}

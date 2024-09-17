using UmojaCampus.Business.Entities;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Contracts
{
    public interface ISemesterService
    {

        Task<Result<SemesterDto>> AddAsync(SaveSemesterDto dto, CurrentUser user);
        Task<Result<SemesterDto>> UpdateAsync(Guid id, SaveSemesterDto dto, CurrentUser user);
        Task<Result<SemesterDto>> FindByIdAsync(Guid id);
        Task<Result<IEnumerable<SemesterDto>>> GetAllAsync();
        Task<Result> DeleteAsync(Guid id);
    }
}

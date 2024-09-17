using UmojaCampus.Shared.DTO;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Shared.Helpers
{
    public static class PaginationHelper
    {
        public static PaginatedResultDto<T> ToPaginatedResultDto<T>(PaginatedResult<T> result)
        {
            return new PaginatedResultDto<T>
            {
                Data = result.Data,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            };
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UmojaCampus.API.Controllers.Base;
using UmojaCampus.Business.Constants;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.Helpers;
using UmojaCampus.Shared.RequestFeatures;

namespace UmojaCampus.API.Controllers
{
    [Route("api/qualifications")]
    [Authorize(Roles = AuthorizationRole.ADMIN)]
    [ApiController]
    public class QualificationsController(IQualificationService qualificationService) : BaseAuthController
    {
        private readonly IQualificationService _qualificationService = qualificationService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = GetUser(User.Identity);
            var result = await _qualificationService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet(nameof(GetPagedList))]
        public async Task<IActionResult> GetPagedList([FromQuery] QueryParameters queryParameters)
        {
            var result = await _qualificationService
                .GetPaginatedResultAsync(queryParameters);

            return Ok(PaginationHelper.ToPaginatedResultDto(result));
        }

        [HttpGet("qualification/{id:guid}")]
        public async Task<IActionResult> GetQualification(Guid id)
        {
            var result = await _qualificationService
                .FindByIdAsync(id);

            if (!result.Succeeded)
                return NotFound(result.Messages);

            return Ok(result.Data);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] SaveQualificationDto dto)
        {
            var result = await _qualificationService
                .AddAsync(dto, GetUser(User.Identity));

            if(!result.Succeeded)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SaveQualificationDto dto)
        {
            var result = await _qualificationService
                .UpdateAsync(id, dto, GetUser(User.Identity));

            if(!result.Succeeded)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _qualificationService.DeleteAsync(id);

            if(!result.Succeeded)
            {
                return NotFound(result.Messages);
            }

            return Ok();
        }
    }
}

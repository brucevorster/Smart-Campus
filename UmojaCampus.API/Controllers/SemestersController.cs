using Microsoft.AspNetCore.Mvc;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.DTO.Inputs;

namespace UmojaCampus.API.Controllers
{
    [Route("api/semesters")]
    [ApiController]
    public class SemestersController(ISemesterService SemesterService) : ControllerBase
    {
        private readonly ISemesterService _semesterService = SemesterService;

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _semesterService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("semester/{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _semesterService
                .FindByIdAsync(id);

            if (!result.Succeeded)
                return NotFound(result.Messages);

            return Ok(result.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Add([FromBody] SaveSemesterDto dto)
        {
            //var user = GetCurrentUser();

            var result = await _semesterService
                .AddAsync(dto, null);

            if (!result.Succeeded)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SaveSemesterDto dto)
        {
           // var user = GetCurrentUser();
            var result = await _semesterService
                .UpdateAsync(id, dto, null);

            if (!result.Succeeded)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpDelete("delete/{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _semesterService.DeleteAsync(id);

            if (!result.Succeeded)
            {
                return NotFound(result.Messages);
            }

            return Ok();
        }

    }
}

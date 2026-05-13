using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Engineers;
using StatusPageServices.ResponseDTO.Engineers;

namespace DistributedApplicationsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EngineersController : ControllerBase
    {
        private readonly IEngineersService _engineersService;

        public EngineersController(IEngineersService engineersService)
        {
            _engineersService = engineersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _engineersService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _engineersService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEngineerDto dto)
        {
            var created = await _engineersService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEngineerDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _engineersService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _engineersService.DeleteAsync(id);
            return NoContent();
        }
    }
}

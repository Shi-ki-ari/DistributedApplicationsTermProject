using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.IncidentUpdates;
using StatusPageServices.ResponseDTO.IncidentUpdates;

namespace DistributedApplicationsProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentUpdatesController : ControllerBase
    {
        private readonly IIncidentUpdatesService _incidentUpdatesService;

        public IncidentUpdatesController(IIncidentUpdatesService incidentUpdatesService)
        {
            _incidentUpdatesService = incidentUpdatesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _incidentUpdatesService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _incidentUpdatesService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncidentUpdateDto dto)
        {
            var created = await _incidentUpdatesService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIncidentUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _incidentUpdatesService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _incidentUpdatesService.DeleteAsync(id);
            return NoContent();
        }
    }
}

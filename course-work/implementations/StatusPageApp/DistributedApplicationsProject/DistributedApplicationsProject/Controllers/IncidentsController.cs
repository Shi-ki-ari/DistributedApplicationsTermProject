using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO.Incidents;

namespace DistributedApplicationsProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentsService _incidentsService;

        public IncidentsController(IIncidentsService incidentsService)
        {
            _incidentsService = incidentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUpdatesForIncident([FromQuery] PaginationQuery query)
        {
            return Ok(await _incidentsService.GetPagedIncidentsAsync(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _incidentsService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncidentDto dto)
        {
            var created = await _incidentsService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIncidentDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _incidentsService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _incidentsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
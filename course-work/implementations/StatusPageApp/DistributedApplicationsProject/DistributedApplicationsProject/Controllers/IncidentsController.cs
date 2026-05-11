using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO.Incidents;

namespace DistributedApplicationsProject.Controllers
{
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _incidentsService.GetAllAsync();
            return Ok(result);
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
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Services;
using StatusPageServices.ResponseDTO.Services;

namespace DistributedApplicationsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _servicesService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _servicesService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
        {
            var created = await _servicesService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _servicesService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _servicesService.DeleteAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.ServiceChecks;
using StatusPageServices.ResponseDTO.ServiceChecks;

namespace DistributedApplicationsProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceChecksController : ControllerBase
    {
        private readonly IServiceChecksService _serviceChecksService;

        public ServiceChecksController(IServiceChecksService serviceChecksService)
        {
            _serviceChecksService = serviceChecksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceChecksService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _serviceChecksService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost("sweep")]
        public async Task<IActionResult> Sweep()
        {
            var created = await _serviceChecksService.ExecuteSweepAsync();
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceChecksService.DeleteAsync(id);
            return NoContent();
        }
    }
}

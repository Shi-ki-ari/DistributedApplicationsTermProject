using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.ServiceCategories;
using StatusPageServices.ResponseDTO.ServiceCategories;

namespace DistributedApplicationsProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly IServiceCategoriesService _serviceCategoriesService;

        public ServiceCategoriesController(IServiceCategoriesService serviceCategoriesService)
        {
            _serviceCategoriesService = serviceCategoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceCategoriesService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _serviceCategoriesService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceCategoryDto dto)
        {
            var created = await _serviceCategoriesService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceCategoryDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _serviceCategoriesService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceCategoriesService.DeleteAsync(id);
            return NoContent();
        }
    }
}

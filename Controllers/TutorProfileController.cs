using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class TutorProfileController : Controller
    {

        private readonly ITutorProfileService _tutorProfileService;
        public TutorProfileController(ITutorProfileService tutorProfileService)
        {
            _tutorProfileService = tutorProfileService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _tutorProfileService.GetAll());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getById(Guid id)
        {
            var tutor = await _tutorProfileService.getById(id);
            
            return tutor != null ? Ok(tutor) : NotFound();
            
        }
        
        [HttpPost("Create")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create([FromBody] TutorProfile newTutorProfile)
        {

            var createdTutorProfile = await _tutorProfileService.Create(newTutorProfile);
            return CreatedAtAction(nameof(getById), new { id = createdTutorProfile.TutorId }, createdTutorProfile);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] TutorProfile editedTutorProfile)
        {

            return await _tutorProfileService.Update(id, editedTutorProfile) ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            return await _tutorProfileService.ChangeStatus(id) ? Ok("Se ha cambiado el estado del evento") : NotFound();
        }
    }
}

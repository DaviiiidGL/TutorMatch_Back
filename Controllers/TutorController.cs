using TutorMatch_Back.Interfaces;
using TutorMatch_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TutorMatch_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // con este DataAnnotation solo se dejará ejecutar los endpoint a los JWT que sea de Admin
    // Si se quiere agregar ás roles se separa por comas Admin,User,Colab
    [Authorize(Roles = "Admin")]
    public class TutorController : Controller
    {
        private readonly ITutorService _tutorService;

        public TutorController (ITutorService tutorService)
        {
            _tutorService = tutorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        // Este DataAnnotation sirve para que este endpoint en específico no requiera de autenticación
        // Por defecto el authorize toma todos los endpoints que no tengan este indicativo
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _tutorService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(Guid id)
        {
            var tutor = await _tutorService.getById(id);
            //Se refactoriza condicion por una operación ternaria o si corto
            return tutor != null ? Ok(tutor) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TutorProfile newTutor)
        {

            var createdTutor = await _eventTutor.Create(newTutor);
            return CreatedAtAction(nameof(getById), new { id = createdTutor.TutorId }, createdTutor);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] TutorProfile editedTutor)
        {

            return await _tutorService.Update(id, editedTutor) ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            return await _tutorService.ChangeStatus(id) ? Ok("Se ha cambiado el estado del tutor") : NotFound();
        }




    }
}
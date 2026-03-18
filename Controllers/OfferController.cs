using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // con este DataAnnotation solo se dejará ejecutar los endpoint a los JWT que sea de Admin
    // Si se quiere agregar ás roles se separa por comas Admin,User,Colab
    [Authorize(Roles = "Admin")]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        // Este DataAnnotation sirve para que este endpoint en específico no requiera de autenticación
        // Por defecto el authorize toma todos los endpoints que no tengan este indicativo
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _offerService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(Guid id)
        {
            var offer = await _offerService.getById(id);
            //Se refactoriza condicion por una operación ternaria o si corto
            return offer != null ? Ok(offer) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Offer newOffer)
        {

            var createdOffer = await _offerService.Create(newOffer);
            return CreatedAtAction(nameof(getById), new { id = createdOffer.OfferId }, createdOffer);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Offer editedOffer)
        {

            return await _offerService.Update(id, editedOffer) ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            return await _offerService.ChangeStatus(id) ? Ok("Se ha cambiado el estado de la oferta") : NotFound();
        }
    }
}

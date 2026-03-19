using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _offerService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(Guid id)
        {
            var offer = await _offerService.getById(id);
            //Se refactoriza condicion por una operación ternaria o si corto
            return offer != null ? Ok(offer) : NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Tutor")]
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

        //[HttpGet("{id}")]
        //[Authorize(Roles = "Tutor")]
        //public async Task<IActionResult> getByTutorId(Guid id)
        //{
            // Capturamos el id del token o del Identity
            // Este Id es necesario para validar si ese usuario o JWT si corresponde al cliente
            // con esto se evita un error de vulnerabildiad
            //string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // se envía como argumento el id del cliente que pasa por parámetro en la URL get
            // Y se envía el id de la identidad para corrobar que si corresponda
            // var ticket = await _Service.getByClientId(id, UserId);
            //Se refactoriza condicion por una operación ternaria o si corto
            // return ticket != null ? Ok(ticket) : NotFound();
            //if (evento == null)
            //{
            //    return NotFound("No existe el evento");
            //}
            //return Ok(evento);
       // }
    }
}
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
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _offerService.GetAll());

        [HttpGet("{id}/getById")]
        public async Task<IActionResult> getById(Guid id)
        {
            var offer = await _offerService.getById(id);
            //Se refactoriza condicion por una operación ternaria o si corto
            return offer != null ? Ok(offer) : NotFound();
        }

        [HttpPut("Create")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create([FromBody] Offer newOffer)
        {
            var createdOffer = await _offerService.Create(newOffer);
            return CreatedAtAction(nameof(getById), new { id = createdOffer.OfferId }, createdOffer);
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Offer editedOffer)
        {

            return await _offerService.Update(id, editedOffer) ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/change-status")]
        [Authorize(Roles = "Tutor, Admin")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            return await _offerService.ChangeStatus(id) ? Ok("Se ha cambiado el estado de la oferta") : NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getByTutorId(Guid id)
        {
      
            var ticket = await _offerService.listByTutor(id);
       
        return ticket != null ? Ok(ticket) : NotFound();

        }
    }
}
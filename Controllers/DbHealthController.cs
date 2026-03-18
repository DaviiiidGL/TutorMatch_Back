using Microsoft.AspNetCore.Mvc;
using TutorMatch.DAO;

namespace TutorMatch.Controllers
{
    // Clase para probar la conexion a base de datos
    //TODO: Eliminar clase despues de probar conexion en mas dispositivos

    [ApiController]
    [Route("api/[controller]")]
    public class DbHealthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DbHealthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            try
            {
                // consulta muy simple contra la DB
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new { canConnect });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}

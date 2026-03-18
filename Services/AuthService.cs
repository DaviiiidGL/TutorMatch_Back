using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TutorMatch.Interfaces;

namespace TutorMatch.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(string email, string pw, string role)
        {
            // 1. Validar rol permitido (pero SIN consultar a la BD)
            if (role != "Student" && role != "Tutor" && role != "Admin")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "El rol debe ser Student, Tutor o Admin."
                });
            }

            // 2. Verificar si el email ya está en uso
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Ya existe un usuario con ese email."
                });
            }

            // 3. Crear usuario
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, pw);

            if (!result.Succeeded)
                return result;

            // 4. Asignar rol DIRECTAMENTE (ya lo creaste a mano en AspNetRoles)
            var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return addToRoleResult;
            }

            return IdentityResult.Success;
        }


        public async Task<string?> Login(string email, string pwd)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, pwd))
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            return GenerarJwtToken(user, userRoles);
        }

        private string GenerarJwtToken(IdentityUser user, IList<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authFirmaKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(5), // ✅ UtcNow en lugar de Now
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authFirmaKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

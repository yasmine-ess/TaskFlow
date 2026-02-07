using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlow.Models;
using TaskFlow.Models.DTOs;

namespace TaskFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //La variable _userManager est de type UserManager<ApplicationUser>
        //C’est l’outil fourni par Identity pour gérer les utilisateurs : créer, vérifier mot de passe, récupérer par email, etc.

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Tu es authentifié !");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,  // on utilise email comme username
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Utilisateur créé avec succès !" });
        }




    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
           
            var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized("Email ou mot de passe invalide");

        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid) return Unauthorized("Email ou mot de passe invalide");

            //  CREER LE TOKEN ICI
            var token = GenerateJwtToken(user);

            // retourner le token
            return Ok(new
            {
                token = token
            });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("PING OK");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }
}

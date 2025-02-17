using DashBe.Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using DashBe.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using DashBe.Domain.Models;

namespace DashBe.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public UserController(IUserService userService, IJwtTokenService jwtTokenService) 
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Application.DTOs.RegisterRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string .IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { Message = "Tutti i campi sono obbligatori" });
                }

            await _userService.RegisterAsync(request.Username, request.Email, request.Password);
            return Ok(new { Message = "Utente registrato con successo" });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Application.DTOs.LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new { Message = "Credenziali non valide" });

            var roles = user.RoleUsers.Select(r => r.Role.Name);

            var token = _jwtTokenService.GenerateToken(user.Id, user.Username, user.Email, roles);

            return Ok(new { Token = token});
        }

        [Authorize(Roles = "Admin")] 
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto request)
        {
            try
            {
                await _userService.AssignRoleAsync(request.UserId, request.RoleId);
                return Ok(new { Message = "Ruolo assegnato correttamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var userDto = await _userService.GetByIdAsync(userId);
            if (userDto == null)
                return NotFound(new { Message = "Utente non trovato" });

            return Ok(userDto);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userService.ConfirmEmailAsync(token, email);

            if (user == null)
            {
                return BadRequest(new { Message = "Link di conferma non valido o utente non trovato." });
            }

            return Ok(new { Message = "Email confermata con successo!" });
        }


    }
}

using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class CadastroUsersController : ControllerBase
    {
        private readonly ICadastroUserApplication _cadastroUserApplication;
        public CadastroUsersController(ICadastroUserApplication cadastroUserApplication)
        {
            _cadastroUserApplication = cadastroUserApplication;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var register = await _cadastroUserApplication.Register(registerDTO);
                if (register.Succeeded)
                {
                    return Created("Conta criada com sucesso!", registerDTO.Nome);
                }
                else
                {
                    return BadRequest(register.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var login = await _cadastroUserApplication.Login(loginDTO);
                if (login.Succeeded)
                {
                    var user = await _cadastroUserApplication.UserExist(loginDTO.CPF);
                    return Ok(new
                    {
                        token = _cadastroUserApplication.JWT(user).Result
                    });
                }
                else
                {
                    return Unauthorized("CPF ou placa incorretos.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

using Application;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroWebApi.Controllers
{
    [Route("api/associados")]
    [ApiController]
    public class AssociadosController : ControllerBase
    {
        private readonly IAssociadoApplication _associadoApplication;
        public AssociadosController(IAssociadoApplication associadoApplication)
        {
            _associadoApplication = associadoApplication;
        }

        [HttpGet("all", Name = "Get-Associados")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var associados = await _associadoApplication.GetAll();
                return this.HATEOASResult(associados, (a => this.Ok(a)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "Get-Associado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var associado = await _associadoApplication.Get(id);
                return this.HATEOASResult(associado, (a) => this.Ok(a));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("self/show", Name = "Own-Show")]
        [Authorize]
        public async Task<IActionResult> Show()
        {
            try
            {
                var associado = User.Identity.Name;
                var show = await _associadoApplication.GetByPlaca(associado);
                if(show != null)
                {
                    return this.HATEOASResult(show, (a) => this.Ok(a));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("self/update", Name = "Own-Update")]
        [Authorize]
        public async Task<IActionResult> Update(EnderecoDTO enderecoDTO)
        {
            try
            {
                var associado = User.Identity.Name;
                var user = await _associadoApplication.GetByPlaca(associado);

                if (user != null)
                {
                    enderecoDTO.Id = user.EnderecoId;
                    user.Endereco = enderecoDTO;
                    await _associadoApplication.Edit(user);
                    await _associadoApplication.Save();
                    return this.HATEOASResult(null, (a) => this.Ok("Dados atualizados com sucesso!"));
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "Edit-Associado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(AssociadoDTO associadoDTO)
        {
            try
            {
                await _associadoApplication.Edit(associadoDTO);
                await _associadoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados atualizados com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create-Associado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AssociadoDTO associadoDTO)
        {
            try
            {
                await _associadoApplication.Add(associadoDTO);
                await _associadoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Created("Associado adicionado!", 
                    new { Associado = associadoDTO.Nome }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "Delete-Associado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _associadoApplication.Delete(id);
                await _associadoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados deletados com sucesso!"));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

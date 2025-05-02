using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroWebApi.Controllers
{
    [Route("api/enderecos")]
    [ApiController]
    public class EnderecosController : ControllerBase
    {
        private readonly IEnderecoApplication _enderecoApplication;
        public EnderecosController(IEnderecoApplication enderecoApplication)
        {
            _enderecoApplication = enderecoApplication;
        }

        [HttpGet("all", Name = "Get-Enderecos")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var enderecos = await _enderecoApplication.GetAll();
                return this.HATEOASResult(enderecos, (a => this.Ok(a)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "Get-Endereco")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var endereco = await _enderecoApplication.Get(id);
                return this.HATEOASResult(endereco, (a) => this.Ok(a));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut(Name = "Edit-Endereco")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EnderecoDTO enderecoDTO)
        {
            try
            {
                await _enderecoApplication.Edit(enderecoDTO);
                await _enderecoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados atualizados com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create-Endereco")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(EnderecoDTO enderecoDTO)
        {
            try
            {
                await _enderecoApplication.Add(enderecoDTO);
                await _enderecoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Created("Endereco adicionado!",
                    new { Endereco = enderecoDTO }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "Delete-Endereco")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _enderecoApplication.Delete(id);
                await _enderecoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados deletados com sucesso!"));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

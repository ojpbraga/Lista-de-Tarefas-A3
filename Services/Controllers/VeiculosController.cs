using Application.DTOs;
using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CadastroWebApi.Controllers
{
    [Route("api/veiculos")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoApplication _veiculoApplication;
        public VeiculosController(IVeiculoApplication veiculoApplication)
        {
            _veiculoApplication = veiculoApplication;
        }

        [HttpGet("all", Name = "Get-Veiculos")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var veiculos = await _veiculoApplication.GetAll();
                return this.HATEOASResult(veiculos, (a => this.Ok(a)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "Get-Veiculo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var veiculo = await _veiculoApplication.Get(id);
                return this.HATEOASResult(veiculo, (a) => this.Ok(a));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut(Name = "Edit-Veiculo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(VeiculoDTO veiculoDTO)
        {
            try
            {
                await _veiculoApplication.Edit(veiculoDTO);
                await _veiculoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados atualizados com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create-Veiculo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(VeiculoDTO veiculoDTO)
        {
            try
            {
                await _veiculoApplication.Add(veiculoDTO);
                await _veiculoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Created("Veiculo adicionado!",
                    new { Veiculo = veiculoDTO.Modelo }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "Delete-Veiculo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _veiculoApplication.Delete(id);
                await _veiculoApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados deletados com sucesso!"));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

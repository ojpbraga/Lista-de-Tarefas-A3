using Application.DTOs;
using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;

namespace CadastroWebApi.Controllers
{
    [Route("api/carros")]
    [ApiController]
    public class CarrosController : ControllerBase
    {
        private readonly ICarroApplication _carroApplication;
        public CarrosController(ICarroApplication carroApplication)
        {
            _carroApplication = carroApplication;
        }

        [HttpGet("all", Name = "Get-Carros")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var carros = await _carroApplication.GetAll();
                return this.HATEOASResult(carros, (a => this.Ok(a)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "Get-Carro")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var carro = await _carroApplication.Get(id);
                return this.HATEOASResult(carro, (a) => this.Ok(a));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut(Name = "Edit-Carro")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CarroDTO carroDTO)
        {
            try
            {
                await _carroApplication.Edit(carroDTO);
                await _carroApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados atualizados com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create-Carro")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(CarroDTO carroDTO)
        {
            try
            {
                await _carroApplication.Add(carroDTO);
                await _carroApplication.Save();
                return this.HATEOASResult(null, (a) => this.Created("Carro adicionado!",
                    new { Carro = carroDTO.Modelo }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "Delete-Carro")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _carroApplication.Delete(id);
                await _carroApplication.Save();
                return this.HATEOASResult(null, (a) => this.Ok("Dados deletados com sucesso!"));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

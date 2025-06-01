using Application.DTOs;
using Application.Interfaces;
using CadastroWebApi.Controllers;
using Domain.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test.ApiControllerTest
{
    public class VeiculosControllerTest
    {
        [Fact]
        public async Task GetVeiculo_NullVeiculo_ReturnNotFound()
        {
            //Arrange
            IVeiculoApplication veiculo = null;
            var controller = new VeiculosController(veiculo);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetVeiculo_VeiculoFound_ReturnOk()
        {
            //Arrange
            VeiculoDTO Veiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();

            repositoryTest.Setup(r => r.Get(It.IsAny<int>()))
                .ReturnsAsync(Veiculo);

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_AllDatabaseVeiculos_ReturnOk()
        {
            //Arrange
            List<VeiculoDTO> listVeiculos = new List<VeiculoDTO>();
            VeiculoDTO Veiculo = new VeiculoDTO();

            listVeiculos.Add(Veiculo);

            var repositoryTest = new Mock<IVeiculoApplication>();

            repositoryTest.Setup(r => r.GetAll())
                .ReturnsAsync(listVeiculos);

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_UnavailableDatabase_ReturnBadRequest()
        {
            //Arrange
            List<VeiculoDTO> listVeiculos = new List<VeiculoDTO>();
            VeiculoDTO Veiculo = new VeiculoDTO();

            listVeiculos.Add(Veiculo);

            var repositoryTest = new Mock<IVeiculoApplication>();
            repositoryTest.Setup(r => r.GetAll())
                .Throws<Exception>();

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateVeiculo_NewVeiculo_ReturnCreated()
        {
            //Arrange
            var newVeiculo = new VeiculoDTO()
            {
                Id = 1,
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();
            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Add(newVeiculo);

            //Assert
            Assert.IsType<HATEOASResult>(resultado);

            newVeiculo.Id.Should().NotBe(null, "Can't be null.");
            newVeiculo.Placa.Should().NotBe(null, "Can't be null.");
            newVeiculo.Modelo.Should().NotBeNull();
            newVeiculo.Id.Should().Be(1);
            newVeiculo.Placa.Should().Be("2541OAX");
            newVeiculo.Should().BeOfType<VeiculoDTO>();
        }

        [Fact]
        public async Task CreateVeiculo_BadObject_ReturnBadrequest()
        {
            //Arrange
            IVeiculoApplication newVeiculo = null;
            var controller = new VeiculosController(newVeiculo);

            //Act
            var result = await controller.Add(It.IsAny<VeiculoDTO>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateVeiculo_DataAnnotationTest_ReturnOk()
        {
            //Assert
            var newVeiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            //Act
            var errors = ValidateObject(newVeiculo);

            //Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void CreateVeiculo_DataAnnotationTest_ErrorObjectVeiculo()
        {
            //Assert
            var newVeiculo = new VeiculoDTO()
            {
                Placa = "b",
                Modelo = ""
            };

            //Act
            var erros = ValidateObject(newVeiculo);

            //Assert
            Assert.True(erros.Count == 2);
        }

        [Fact]
        public async Task EditVeiculo_Veiculo_ReturnOk()
        {
            //Arrange
            var Veiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();
            repositoryTest.Setup(r => r.Edit(It.IsAny<VeiculoDTO>())).Returns(Task.FromResult(Veiculo));

            var editedVeiculo = new VeiculoDTO()
            {
                Id = Veiculo.Id + 3,
                Placa = Veiculo.Placa + "Y",
                Modelo = Veiculo.Modelo + "z"
            };

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(editedVeiculo);

            //Assert
            result.Should().BeOfType<HATEOASResult>();

            editedVeiculo.Id.Should().Be(3);
            editedVeiculo.Placa.Should().Be("2541OAXY");
            editedVeiculo.Modelo.Should().Be("Mercedez Benzz");
        }

        [Fact]
        public async Task EditVeiculo_VeiculoError_ReturBadRequesst()
        {
            //Arrange
            var Veiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();
            repositoryTest.Setup(r => r.Edit(Veiculo)).Throws<Exception>();

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(Veiculo);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteVeiculo_Veiculo_ReturnOk()
        {
            //Arrange
            var Veiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.FromResult(Veiculo));

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<HATEOASResult>();
        }

        [Fact]
        public async Task DeleteVeiculo_VeiculoNotFound_ReturnNotFound()
        {
            //Arrange
            var Veiculo = new VeiculoDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<IVeiculoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Throws<Exception>();

            var controller = new VeiculosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<NotFoundObjectResult>();
        }

        private static IList<ValidationResult> ValidateObject(VeiculoDTO VeiculoDTO)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(VeiculoDTO, null, null);
            Validator.TryValidateObject(VeiculoDTO, context, validate, true);
            return validate;
        }
    }
}

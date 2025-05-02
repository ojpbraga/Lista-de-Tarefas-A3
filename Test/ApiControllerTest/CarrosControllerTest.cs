using Application.DTOs;
using Application.Interfaces;
using CadastroWebApi.Controllers;
using Domain.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.ApiControllerTest
{
    public class CarrosControllerTest
    {
        [Fact]
        public async Task GetCarro_NullCarro_ReturnNotFound()
        {
            //Arrange
            ICarroApplication carro = null;
            var controller = new CarrosController(carro);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCarro_CarroFound_ReturnOk()
        {
            //Arrange
            CarroDTO carro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();

            repositoryTest.Setup(r => r.Get(It.IsAny<int>()))
                .ReturnsAsync(carro);

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_AllDatabaseCarros_ReturnOk()
        {
            //Arrange
            List<CarroDTO> listCarros = new List<CarroDTO>();
            CarroDTO carro = new CarroDTO();

            listCarros.Add(carro);

            var repositoryTest = new Mock<ICarroApplication>();

            repositoryTest.Setup(r => r.GetAll())
                .ReturnsAsync(listCarros);

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_UnavailableDatabase_ReturnBadRequest()
        {
            //Arrange
            List<CarroDTO> listCarros = new List<CarroDTO>();
            CarroDTO carro = new CarroDTO();

            listCarros.Add(carro);

            var repositoryTest = new Mock<ICarroApplication>();
            repositoryTest.Setup(r => r.GetAll())
                .Throws<Exception>();

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateCarro_NewCarro_ReturnCreated()
        {
            //Arrange
            var newCarro = new CarroDTO()
            {
                Id = 1,
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();
            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Add(newCarro);

            //Assert
            Assert.IsType<HATEOASResult>(resultado);

            newCarro.Id.Should().NotBe(null, "Can't be null.");
            newCarro.Placa.Should().NotBe(null, "Can't be null.");
            newCarro.Modelo.Should().NotBeNull();
            newCarro.Id.Should().Be(1);
            newCarro.Placa.Should().Be("2541OAX");
            newCarro.Should().BeOfType<CarroDTO>();
        }

        [Fact]
        public async Task CreateCarro_BadObject_ReturnBadrequest()
        {
            //Arrange
            ICarroApplication newCarro = null;
            var controller = new CarrosController(newCarro);

            //Act
            var result = await controller.Add(It.IsAny<CarroDTO>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateCarro_DataAnnotationTest_ReturnOk()
        {
            //Assert
            var newCarro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            //Act
            var errors = ValidateObject(newCarro);

            //Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void CreateCarro_DataAnnotationTest_ErrorObjectCarro()
        {
            //Assert
            var newCarro = new CarroDTO()
            {
                Placa = "b",
                Modelo = ""
            };

            //Act
            var erros = ValidateObject(newCarro);

            //Assert
            Assert.True(erros.Count == 2);
        }

        [Fact]
        public async Task EditCarro_Carro_ReturnOk()
        {
            //Arrange
            var carro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();
            repositoryTest.Setup(r => r.Edit(It.IsAny<CarroDTO>())).Returns(Task.FromResult(carro));

            var editedCarro = new CarroDTO()
            {
                Id = carro.Id + 3,
                Placa = carro.Placa + "Y",
                Modelo = carro.Modelo + "z"
            };

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(editedCarro);

            //Assert
            result.Should().BeOfType<HATEOASResult>();

            editedCarro.Id.Should().Be(3);
            editedCarro.Placa.Should().Be("2541OAXY");
            editedCarro.Modelo.Should().Be("Mercedez Benzz");
        }

        [Fact]
        public async Task EditCarro_CarroError_ReturBadRequesst()
        {
            //Arrange
            var carro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();
            repositoryTest.Setup(r => r.Edit(carro)).Throws<Exception>();

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(carro);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteCarro_Carro_ReturnOk()
        {
            //Arrange
            var carro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.FromResult(carro));

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<HATEOASResult>();
        }

        [Fact]
        public async Task DeleteCarro_CarroNotFound_ReturnNotFound()
        {
            //Arrange
            var carro = new CarroDTO()
            {
                Placa = "2541OAX",
                Modelo = "Mercedez Benz"
            };

            var repositoryTest = new Mock<ICarroApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Throws<Exception>();

            var controller = new CarrosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<NotFoundObjectResult>();
        }

        private static IList<ValidationResult> ValidateObject(CarroDTO CarroDTO)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(CarroDTO, null, null);
            Validator.TryValidateObject(CarroDTO, context, validate, true);
            return validate;
        }
    }
}

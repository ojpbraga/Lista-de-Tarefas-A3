using Application;
using Application.DTOs;
using Application.Interfaces;
using CadastroWebApi.Controllers;
using Domain.Entities;
using Domain.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Xunit;

namespace Test.ApiControllerTest
{
    public class EnderecosControllerTest
    {
        [Fact]
        public async Task GetEndereco_NullEndereco_ReturnNotFound()
        {
            //Arrange
            IEnderecoApplication endereco = null;
            var controller = new EnderecosController(endereco);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetEndereco_EnderecoFound_ReturnOk()
        {
            //Arrange
            EnderecoDTO endereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade =  "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();

            repositoryTest.Setup(r => r.Get(It.IsAny<int>()))
                .ReturnsAsync(endereco);

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_AllDatabaseEnderecos_ReturnOk()
        {
            //Arrange
            List<EnderecoDTO> listEnderecos = new List<EnderecoDTO>();
            EnderecoDTO endereco = new EnderecoDTO();

            listEnderecos.Add(endereco);

            var repositoryTest = new Mock<IEnderecoApplication>();

            repositoryTest.Setup(r => r.GetAll())
                .ReturnsAsync(listEnderecos);

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_UnavailableDatabase_ReturnBadRequest()
        {
            //Arrange
            List<EnderecoDTO> listEnderecos = new List<EnderecoDTO>();
            EnderecoDTO endereco = new EnderecoDTO();

            listEnderecos.Add(endereco);

            var repositoryTest = new Mock<IEnderecoApplication>();
            repositoryTest.Setup(r => r.GetAll())
                .Throws<Exception>();

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateEndereco_NewEndereco_ReturnCreated()
        {
            //Arrange
            EnderecoDTO newEndereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();
            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Add(newEndereco);

            //Assert
            Assert.IsType<HATEOASResult>(resultado);

            newEndereco.CEP.Should().NotBe(null, "Can't be null.");
            newEndereco.Estado.Should().NotBe(null, "Can't be null.");
            newEndereco.Cidade.Should().NotBeNull();
            newEndereco.Numero.Should().Be(220);
            newEndereco.Pais.Should().Be("Brasil");
            newEndereco.Should().BeOfType<EnderecoDTO>();
        }

        [Fact]
        public async Task CreateEndereco_BadObject_ReturnBadrequest()
        {
            //Arrange
            IEnderecoApplication newEndereco = null;
            var controller = new EnderecosController(newEndereco);

            //Act
            var result = await controller.Add(It.IsAny<EnderecoDTO>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateEndereco_DataAnnotationTest_ReturnOk()
        {
            //Assert
            EnderecoDTO newEndereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            //Act
            var errors = ValidateObject(newEndereco);

            //Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void CreateEndereco_DataAnnotationTest_ErrorObjectEndereco()
        {
            //Assert
            var newEndereco = new EnderecoDTO()
            {
                CEP = "1",
                Rua = "or",
                Numero = -9,
                Bairro = "Centro",
                Cidade = "sp",
                Estado = "s",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            //Act
            var erros = ValidateObject(newEndereco);

            //Assert
            Assert.True(erros.Count == 3);
        }

        [Fact]
        public async Task EditEndereco_Endereco_ReturnOk()
        {
            //Arrange
            var Endereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();
            repositoryTest.Setup(r => r.Edit(It.IsAny<EnderecoDTO>())).Returns(Task.FromResult(Endereco));

            var editedEndereco = new EnderecoDTO()
            {
                Numero = Endereco.Numero + 3,
                Rua = Endereco.Rua + " Abreu",
                Bairro = Endereco.Bairro + " Sul"
            };

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(editedEndereco);

            //Assert
            result.Should().BeOfType<HATEOASResult>();

            editedEndereco.Numero.Should().Be(223);
            editedEndereco.Rua.Should().Be("Tancredo Neves Abreu");
            editedEndereco.Bairro.Should().Be("Centro Sul");
        }

        [Fact]
        public async Task EditEndereco_EnderecoError_ReturBadRequesst()
        {
            //Arrange
            var endereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();
            repositoryTest.Setup(r => r.Edit(endereco)).Throws<Exception>();

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(endereco);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteEndereco_Endereco_ReturnOk()
        {
            //Arrange
            var endereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.FromResult(endereco));

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<HATEOASResult>();
        }

        [Fact]
        public async Task DeleteEndereco_EnderecoNotFound_ReturnNotFound()
        {
            //Arrange
            var endereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var repositoryTest = new Mock<IEnderecoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Throws<Exception>();

            var controller = new EnderecosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<NotFoundObjectResult>();
        }

        private static IList<ValidationResult> ValidateObject(EnderecoDTO EnderecoDTO)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(EnderecoDTO, null, null);
            Validator.TryValidateObject(EnderecoDTO, context, validate, true);
            return validate;
        }
    }
}

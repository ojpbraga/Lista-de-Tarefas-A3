using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Controllers;
using Xunit;

namespace Test.ApiControllerTest
{
    public class CadastroUserControllerTest
    {
        [Fact]
        public async Task Login_LoginDone_ReturnOk()
        {
            //Arrange
            var user = new LoginDTO()
            {
                CPF = "12324345613",
                Placa = "2541OAX"
            };

            var identityTest = new Mock<ICadastroUserApplication>();
            identityTest.Setup(i => i.Login(user))
                .ReturnsAsync(SignInResult.Success);

            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var result = await controller.Login(user);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_LoginError_ReturnUnauthorized()
        {
            //Arrange
            var user = new LoginDTO()
            {
                CPF = "1",
                Placa = "4"
            };

            var identityTest = new Mock<ICadastroUserApplication>();
            identityTest.Setup(i => i.Login(user))
                .ReturnsAsync(SignInResult.Failed);

            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var result = await controller.Login(user);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ErroLoginException_Return500()
        {
            //Arrange
            var user = new LoginDTO()
            {
                CPF = "12324345613",
                Placa = "2541OAX"
            };

            var identityTest = new Mock<ICadastroUserApplication>();
            identityTest.Setup(i => i.Login(user))
                .Throws<Exception>();

            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var result = await controller.Login(user);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(result);
        }

        [Fact]
        public async Task Register_RegisterDone_ReturnCreated()
        {
            //Arrange
            var user = new RegisterDTO()
            {
                Nome = "jajajaja",
                CPF = "12324345613",
                Telefone = "035 984736291",
                Placa = "2541OAX",
                Modelo = "Mercedez Benz",
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var identityTest = new Mock<ICadastroUserApplication>();
            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var resultado = await controller.Register(user);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(resultado);
        }

        [Fact]
        public async Task Register_NullRegister_ReturnBadRequest()
        {
            //Arrange
            var usuario = new RegisterDTO();
            usuario = null;

            var identityTest = new Mock<ICadastroUserApplication>();
            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var resultado = await controller.Register(usuario);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(resultado);
        }

        [Fact]
        public async Task Register_RegisterException_Return500()
        {
            //Arrange
            var user = new RegisterDTO()
            {
                Nome = "jajajaja",
                CPF = "12324345613",
                Telefone = "035 984736291",
                Placa = "2541OAX",
                Modelo = "Mercedez Benz",
                CEP = "34156278",
                Rua = "Tancredo Neves",
                Numero = 220,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var identityTest = new Mock<ICadastroUserApplication>();
            identityTest.Setup(i => i.Register(user))
                .Throws<Exception>();

            var controller = new CadastroUsersController(identityTest.Object);

            //Act
            var resultado = await controller.Register(user);

            //Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(resultado);
        }
    }
}

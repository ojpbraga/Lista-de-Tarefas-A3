using Application.DTOs;
using Application.Interfaces;
using CadastroWebApi.Controllers;
using Domain.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Xunit;

namespace Test.ApiTest
{
    public class AssociadosControllerTest
    {
        [Fact]
        public async Task GetAssociado_NullAssociado_ReturnNotFound()
        {
            //Arrange
            IAssociadoApplication associado = null;
            var controller = new AssociadosController(associado);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAssociado_AssociadoFound_ReturnOk()
        {
            //Arrange
            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();

            repositoryTest.Setup(r => r.Get(It.IsAny<int>()))
                .ReturnsAsync(associado);

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var result = await controller.Get(It.IsAny<int>());

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_AllDatabaseAssociados_ReturnOk()
        {
            //Arrange
            List<AssociadoDTO> listAssociados = new List<AssociadoDTO>();
            AssociadoDTO associado = new AssociadoDTO();

            listAssociados.Add(associado);

            var repositoryTest = new Mock<IAssociadoApplication>();

            repositoryTest.Setup(r => r.GetAll())
                .ReturnsAsync(listAssociados);

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task GetAll_UnavailableDatabase_ReturnBadRequest()
        {
            //Arrange
            List<AssociadoDTO> listAssociados = new List<AssociadoDTO>();
            AssociadoDTO associado = new AssociadoDTO();

            listAssociados.Add(associado);

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetAll())
                .Throws<Exception>();

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateAssociado_NewAssociado_ReturnCreated()
        {
            //Arrange
            var newAssociado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();
            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Add(newAssociado);

            //Assert
            Assert.IsType<HATEOASResult>(resultado);

            newAssociado.Id.Should().NotBe(null, "Can't be null.");
            newAssociado.CPF.Should().NotBe(null, "Can't be null.");
            newAssociado.Nome.Should().NotBeNull();
            newAssociado.Id.Should().Be(1);
            newAssociado.CarroId.Should().Be(22);
            newAssociado.Should().BeOfType<AssociadoDTO>();
        }

        [Fact]
        public async Task CreateAssociado_BadObject_ReturnBadrequest()
        {
            //Arrange
            IAssociadoApplication newAssociado = null;
            var controller = new AssociadosController(newAssociado);

            //Act
            var result = await controller.Add(It.IsAny<AssociadoDTO>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateAssociado_DataAnnotationTest_ReturnOk()
        {
            //Assert
            var newAssociado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            //Act
            var errors = ValidateObject(newAssociado);

            //Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void CreateAssociado_DataAnnotationTest_ErrorObjectAssociado()
        {
            //Assert
            var newAssociado = new AssociadoDTO()
            {
                Id = -1,
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "1",
                Telefone = "5",
                CarroId = -1,
                EnderecoId = -1
            };

            //Act
            var erros = ValidateObject(newAssociado);

            //Assert
            Assert.True(erros.Count == 5);
        }

        [Fact]
        public async Task EditAssociado_Associado_ReturnOk()
        {
            //Arrange
            var associado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.Edit(It.IsAny<AssociadoDTO>())).Returns(Task.FromResult(associado));

            var editedAssociado = new AssociadoDTO()
            {
                Id = associado.Id + 3,
                Nome = associado.Nome + " Vinícius Souza Ribeiro",
                CarroId = associado.CarroId + 4
            };

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(editedAssociado);

            //Assert
            result.Should().BeOfType<HATEOASResult>();

            editedAssociado.Id.Should().Be(4);
            editedAssociado.Nome.Should().Be("Arthur Vinícius Souza Ribeiro");
            editedAssociado.CarroId.Should().Be(26);
        }

        [Fact]
        public async Task EditAssociado_AssociadoError_ReturBadRequesst()
        {
            //Arrange
            var associado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.Edit(associado)).Throws<Exception>();

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var result = await controller.Edit(associado);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteAssociado_Associado_ReturnOk()
        {
            //Arrange
            var associado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Returns(Task.FromResult(associado));

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<HATEOASResult>();
        }

        [Fact]
        public async Task DeleteAssociado_AssociadoNotFound_ReturnNotFound()
        {
            //Arrange
            var associado = new AssociadoDTO()
            {
                Id = 1,
                Nome = "Arthur",
                CPF = "12324345613",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.Delete(It.IsAny<int>())).Throws<Exception>();

            var controller = new AssociadosController(repositoryTest.Object);

            //Act
            var resultado = await controller.Remove(It.IsAny<int>());

            //Assert
            resultado.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Show_AssociadoSession_ReturnOk()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .ReturnsAsync(associado);

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await controller.Show();

            //Assert
            Assert.IsType<HATEOASResult>(result);
        }

        [Fact]
        public async Task Show_AssociadoNull_ReturnNotFound()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = null }
            };

            //Act
            var result = await controller.Show();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Show_AssociadoException_ReturnBadRequest()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                EnderecoId = 22
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .Throws<Exception>();

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await controller.Show();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_AssociadoEndereco_ReturnOk()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                Carro = new CarroDTO
                {
                    Placa = "0XAB40",
                    Modelo = "Mercedes Benz",
                },
                EnderecoId = 22,
                Endereco = new EnderecoDTO
                {
                    CEP = "34156278",
                    Rua = "Tancredo Neves",
                    Numero = 220,
                    Bairro = "Centro",
                    Cidade =  "São Paulo",
                    Estado = "São Paulo",
                    Pais = "Brasil",
                    Observacao = "Esquina com Bandeirantes 320"
                }
            };

            EnderecoDTO newEndereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = associado.Endereco.Rua + " Branco",
                Numero = associado.Endereco.Numero + 4,
                Bairro = "Sul",
                Cidade = "Belo Horizonte",
                Estado = "Minas Gerais",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .ReturnsAsync(associado);

            repositoryTest.Setup(r => r.Edit(It.IsAny<AssociadoDTO>())).Returns(Task.FromResult(associado));

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await controller.Update(newEndereco);

            //Assert
            Assert.IsType<HATEOASResult>(result);

            newEndereco.Id.Should().Be(associado.Endereco.Id);
            newEndereco.Rua.Should().Be("Tancredo Neves Branco");
            newEndereco.Numero.Should().Be(224);
            newEndereco.Bairro.Should().Be("Sul");
            newEndereco.Cidade.Should().Be("Belo Horizonte");
            newEndereco.Estado.Should().Be("Minas Gerais");
        }

        [Fact]
        public async Task Update_AssociadoNotFound_ReturnNotFound()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                Carro = new CarroDTO
                {
                    Placa = "0XAB40",
                    Modelo = "Mercedes Benz",
                },
                EnderecoId = 22,
                Endereco = new EnderecoDTO
                {
                    CEP = "34156278",
                    Rua = "Tancredo Neves",
                    Numero = 220,
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "São Paulo",
                    Pais = "Brasil",
                    Observacao = "Esquina com Bandeirantes 320"
                }
            };

            EnderecoDTO newEndereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = associado.Endereco.Rua + " Branco",
                Numero = associado.Endereco.Numero + 4,
                Bairro = "Sul",
                Cidade = "Belo Horizonte",
                Estado = "Minas Gerais",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            repositoryTest.Setup(r => r.Edit(It.IsAny<AssociadoDTO>())).Returns(Task.FromResult(associado));

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = null }
            };

            //Act
            var result = await controller.Update(newEndereco);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_AssociadoEditError_ReturnBadRequest()
        {
            //Arrange
            CadastroUser cadastroUser = new CadastroUser()
            {
                Id = 0,
                CPF = "123434532342",
                Placa = "2541OAX"
            };

            AssociadoDTO associado = new AssociadoDTO()
            {
                Nome = "Arthur Vinícius Souza Ribeiro",
                CPF = "123434532342",
                Telefone = "031 995331227",
                CarroId = 22,
                Carro = new CarroDTO
                {
                    Placa = "0XAB40",
                    Modelo = "Mercedes Benz",
                },
                EnderecoId = 22,
                Endereco = new EnderecoDTO
                {
                    CEP = "34156278",
                    Rua = "Tancredo Neves",
                    Numero = 220,
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "São Paulo",
                    Pais = "Brasil",
                    Observacao = "Esquina com Bandeirantes 320"
                }
            };

            EnderecoDTO newEndereco = new EnderecoDTO()
            {
                CEP = "34156278",
                Rua = associado.Endereco.Rua + " Branco",
                Numero = associado.Endereco.Numero + 4,
                Bairro = "Sul",
                Cidade = "Belo Horizonte",
                Estado = "Minas Gerais",
                Pais = "Brasil",
                Observacao = "Esquina com Bandeirantes 320"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                new Claim(ClaimTypes.Name, cadastroUser.Placa)
            }));

            var repositoryTest = new Mock<IAssociadoApplication>();
            repositoryTest.Setup(r => r.GetByPlaca(It.IsAny<string>()))
                .ReturnsAsync(associado);

            repositoryTest.Setup(r => r.Edit(It.IsAny<AssociadoDTO>())).Throws<Exception>();

            var controller = new AssociadosController(repositoryTest.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await controller.Update(newEndereco);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private static IList<ValidationResult> ValidateObject(AssociadoDTO associadoDTO)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(associadoDTO, null, null);
            Validator.TryValidateObject(associadoDTO, context, validate, true);
            return validate;
        }
    }
}

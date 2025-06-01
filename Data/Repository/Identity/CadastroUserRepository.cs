using Data.Context;
using Domain.Entities;
using Domain.Identity;
using Domain.Intefaces;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Data.Repository.Identity
{
    public class CadastroUserRepository : ICadastroUserRepository
    {
        protected readonly UserManager<CadastroUser> _userManager;
        protected readonly SignInManager<CadastroUser> _signInManager;
        protected readonly IPasswordHasher<CadastroUser> _passwordHasher;
        protected readonly CadastroContext _cadastroContext;
        protected readonly IConfiguration _configuration;

        public CadastroUserRepository(UserManager<CadastroUser> userManager,
                                  SignInManager<CadastroUser> signInManager,
                                  IPasswordHasher<CadastroUser> passwordHasher,
                                  CadastroContext cadastroContext,
                                  IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _cadastroContext = cadastroContext;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(RegisterModel registerModel)
        {
            using TransactionScope trans = new(TransactionScopeAsyncFlowOption.Enabled);
            var associado = new Associado
            {
                Nome = registerModel.Nome,
                CPF = registerModel.CPF,
                Telefone = registerModel.Telefone
            };

            await _cadastroContext.Associados.AddAsync(associado);
            await _cadastroContext.SaveChangesAsync();

            var veiculo = new Veiculo
            {
                Placa = registerModel.Placa,
                Modelo = registerModel.Modelo,
                TipoVeiculo = registerModel.TipoVeiculo,
                AssociadoId = associado.Id
            };

            await _cadastroContext.Veiculos.AddAsync(veiculo);
            await _cadastroContext.SaveChangesAsync();

            var endereco = new Endereco
            {
                CEP = registerModel?.CEP,
                Rua = registerModel?.Rua,
                Numero = registerModel?.Numero ?? 0,
                Bairro = registerModel?.Bairro,
                Cidade = registerModel?.Cidade,
                Estado = registerModel?.Estado,
                Pais = registerModel?.Pais,
                Observacao = registerModel?.Observacao,
                AssociadoId = associado.Id
            };
            await _cadastroContext.Enderecos.AddAsync(endereco);
            await _cadastroContext.SaveChangesAsync();

            var user = new CadastroUser()
            {
                CPF = registerModel?.CPF,
                Placa = registerModel?.Placa,
                UserName = registerModel?.Placa?.Trim(),
                PhoneNumber = registerModel?.Telefone,
                AssociadoId = associado.Id
            };

            var result = await _userManager.CreateAsync(user, registerModel.Placa);
            trans.Complete();
            
            return result;
        }

        public async Task<CadastroUser> UserExist(string cpf)
        {
            return await _cadastroContext.Users.Where(c => c.CPF == cpf).FirstOrDefaultAsync();
        }

        public async Task<SignInResult> Login(CadastroUser cadastroUser, string placa)
        {
            return await _signInManager.CheckPasswordSignInAsync(cadastroUser, placa, true);
        }

        public async Task<string> JWT(CadastroUser cadastroUser)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, cadastroUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, cadastroUser.Placa)
                };

                var types = await _userManager.GetRolesAsync(cadastroUser);

                foreach (var type in types)
                {
                    claims.Add(new Claim(ClaimTypes.Role, type));
                }

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Token")));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var handler = new JwtSecurityTokenHandler();
                var token = handler.CreateToken(tokenDescriptor);

                return handler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

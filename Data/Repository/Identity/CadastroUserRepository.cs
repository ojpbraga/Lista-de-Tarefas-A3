using Data.Context;
using Domain.Identity;
using Domain.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<IdentityResult> Register(CadastroUser cadastroUser)
        {
            var endereco = cadastroUser.CreateEndereco(cadastroUser);
            await _cadastroContext.Enderecos.AddAsync(endereco);
            await _cadastroContext.SaveChangesAsync();
            int enderecoId = endereco.Id;

            var carro = cadastroUser.CreateCarro(cadastroUser);
            await _cadastroContext.Carros.AddAsync(carro);
            await _cadastroContext.SaveChangesAsync();
            int carroId = carro.Id;

            var associado = cadastroUser.CreateAssociado(cadastroUser);
            associado.EnderecoId = enderecoId;
            associado.CarroId = carroId;
            await _cadastroContext.Associados.AddAsync(associado);
            await _cadastroContext.SaveChangesAsync();
            int associadoId = associado.Id;

            var user = cadastroUser.CreateUser(cadastroUser);
            user.AssociadoId = associadoId;

            return await _userManager.CreateAsync(user, cadastroUser.Placa);
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

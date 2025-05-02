using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Intefaces
{
    public interface ICadastroUserRepository
    {
        Task<IdentityResult> Register(CadastroUser cadastroUser);
        Task<CadastroUser> UserExist(string cpf);
        Task<SignInResult> Login(CadastroUser cadastroUser, string placa);
        Task<string> JWT(CadastroUser cadastroUser);
    }
}

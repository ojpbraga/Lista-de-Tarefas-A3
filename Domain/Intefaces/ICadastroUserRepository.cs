using Domain.Identity;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Intefaces
{
    public interface ICadastroUserRepository
    {
        Task<IdentityResult> Register(RegisterModel registerModel);
        Task<CadastroUser> UserExist(string cpf);
        Task<SignInResult> Login(CadastroUser cadastroUser, string placa);
        Task<string> JWT(CadastroUser cadastroUser);
    }
}

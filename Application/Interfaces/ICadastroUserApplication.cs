using Application.DTOs;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICadastroUserApplication
    {
        Task<IdentityResult> Register(RegisterDTO registerDTO);
        Task<string> JWT(CadastroUserDTO cadastroUserDTO);
        Task<CadastroUserDTO> UserExist(string cpf);
        Task<SignInResult> Login(LoginDTO loginDTO);
    }
}

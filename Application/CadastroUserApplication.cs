using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Identity;
using Domain.Intefaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class CadastroUserApplication : ICadastroUserApplication
    {
        private readonly ICadastroUserRepository _cadastroUserRepository;
        private readonly IMapper _mapper;

        public CadastroUserApplication(ICadastroUserRepository cadastroUserRepository, IMapper mapper)
        {
            _cadastroUserRepository = cadastroUserRepository;
            _mapper = mapper;
        }

        public async Task<string> JWT(CadastroUserDTO cadastroUserDTO)
        {
            return await _cadastroUserRepository.JWT(_mapper.Map<CadastroUser>(cadastroUserDTO));
        }

        public async Task<SignInResult> Login(LoginDTO loginDTO)
        {
            var user = await _cadastroUserRepository.UserExist(_mapper.Map<CadastroUser>(loginDTO).CPF);
            
            if (user == null)
            {
                return SignInResult.Failed;
            }
            
            var access = await _cadastroUserRepository.Login(user, _mapper.Map<CadastroUser>(loginDTO).Placa);
            
            if (access.Succeeded)
            {
                return SignInResult.Success;
            }

            return SignInResult.Failed;
        }

        public async Task<IdentityResult> Register(RegisterDTO registerDTO)
        {
            var user = _mapper.Map<CadastroUser>(registerDTO);
            var register = await _cadastroUserRepository.Register(user);

            if (register.Succeeded)
            {
                return IdentityResult.Success;
            }
            else
                return IdentityResult.Failed();
        }

        public async Task<CadastroUserDTO> UserExist(string cpf)
        {
            return _mapper.Map<CadastroUserDTO>(await _cadastroUserRepository.UserExist(cpf));
        }
    }
}

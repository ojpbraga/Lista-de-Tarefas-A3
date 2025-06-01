using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Identity;
using Domain.Model;

namespace Application.Mapper
{
    public class CadastroMapper : Profile
    {
        public CadastroMapper()
        {
            CreateMap<AssociadoDTO, Associado>().ReverseMap();
            CreateMap<VeiculoDTO, Veiculo>().ReverseMap();
            CreateMap<EnderecoDTO, Endereco>().ReverseMap();
            CreateMap<LoginDTO, CadastroUser>().ReverseMap();
            CreateMap<CadastroUserDTO, CadastroUser>().ReverseMap();
            CreateMap<RegisterDTO, RegisterModel>().ReverseMap();
        }
    }
}

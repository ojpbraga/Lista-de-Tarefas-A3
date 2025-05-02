using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Identity;

namespace Application.Mapper
{
    public class CadastroMapper : Profile
    {
        public CadastroMapper()
        {
            CreateMap<AssociadoDTO, Associado>().ReverseMap();
            CreateMap<CarroDTO, Carro>().ReverseMap();
            CreateMap<EnderecoDTO, Endereco>().ReverseMap();
            CreateMap<LoginDTO, CadastroUser>().ReverseMap();
            CreateMap<CadastroUserDTO, CadastroUser>().ReverseMap();
            CreateMap<RegisterDTO, CadastroUser>()
                .ForMember(
                    view => view.CPF,
                    prod => prod.MapFrom(src => src.CPF))
                .ForMember(
                    view => view.Placa,
                    prod => prod.MapFrom(src => src.Placa))
                .ForPath(
                    view => view.Associado.Nome,
                    prod => prod.MapFrom(src => src.Nome))
                .ForPath(
                    view => view.Associado.CPF,
                    prod => prod.MapFrom(src => src.CPF))
                .ForPath(
                    view => view.Associado.Telefone,
                    prod => prod.MapFrom(src => src.Telefone))
                .ForPath(
                    view => view.Associado.Carro.Placa,
                    prod => prod.MapFrom(src => src.Placa))
                .ForPath(
                    view => view.Associado.Carro.Modelo,
                    prod => prod.MapFrom(src => src.Modelo))
                .ForPath(
                    view => view.Associado.Endereco.CEP,
                    prod => prod.MapFrom(src => src.CEP))
                .ForPath(
                    view => view.Associado.Endereco.Rua,
                    prod => prod.MapFrom(src => src.Rua))
                .ForPath(
                    view => view.Associado.Endereco.Numero,
                    prod => prod.MapFrom(src => src.Numero))
                .ForPath(
                    view => view.Associado.Endereco.Bairro,
                    prod => prod.MapFrom(src => src.Bairro))
                .ForPath(
                    view => view.Associado.Endereco.Cidade,
                    prod => prod.MapFrom(src => src.Cidade))
                .ForPath(
                    view => view.Associado.Endereco.Estado,
                    prod => prod.MapFrom(src => src.Estado))
                .ForPath(
                    view => view.Associado.Endereco.Pais,
                    prod => prod.MapFrom(src => src.Pais))
                .ForPath(
                    view => view.Associado.Endereco.Observacao,
                    prod => prod.MapFrom(src => src.Observacao))
                .ForPath(
                    view => view.UserName,
                    prod => prod.MapFrom(src => src.Nome))
                .ReverseMap();
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Intefaces;

namespace Application
{
    public class EnderecoApplication : IEnderecoApplication
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public EnderecoApplication(IEnderecoRepository enderecoRepository, IMapper mapper)
        {
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
        }

        public async Task Add(EnderecoDTO enderecoDTO)
        {
            await _enderecoRepository.Add(_mapper.Map<Endereco>(enderecoDTO));
        }

        public async Task Delete(int id)
        {
            var endereco = await _enderecoRepository.Find(id);
            if (endereco == null)
            {
                throw new Exception("Endereço não encontrado.");
            }
            var enderecoDTO = _mapper.Map<AssociadoDTO>(endereco);
            _enderecoRepository.Delete(enderecoDTO.Id);
        }

        public async Task Edit(EnderecoDTO enderecoDTO)
        {
            var endereco = await _enderecoRepository.Find(enderecoDTO.Id);
            if (!_enderecoRepository.Exist(endereco.Id))
            {
                throw new Exception("Endereço não encontrado.");
            }
            else
                _enderecoRepository.Edit(_mapper.Map<Endereco>(enderecoDTO));
        }

        public async Task<EnderecoDTO> Get(int id)
        {
            if (_enderecoRepository.Exist(id))
            {
                return _mapper.Map<EnderecoDTO>(await _enderecoRepository.Get(id));
            }
            else
                throw new Exception("Endereço não encontrado.");
        }

        public async Task<List<EnderecoDTO>> GetAll()
        {
            var result = _mapper.Map<List<EnderecoDTO>>(await _enderecoRepository.GetAll());
            if (result == null)
            {
                throw new Exception("Nenhum dado encontrado.");
            }
            else
                return result;
        }

        public async Task<bool> Save()
        {
            return await _enderecoRepository.Save();
        }
    }
}

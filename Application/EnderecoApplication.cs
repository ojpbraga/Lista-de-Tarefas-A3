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
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IMapper _mapper;

        public EnderecoApplication(IEnderecoRepository enderecoRepository, IMapper mapper, IVeiculoRepository veiculoRepository)
        {
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
            _veiculoRepository = veiculoRepository;
        }

        public async Task Add(EnderecoDTO enderecoDTO)
        {
            await _enderecoRepository.Add(_mapper.Map<Endereco>(enderecoDTO));
        }

        public async Task Delete(int id)
        {
            var endereco = await _enderecoRepository.Find(id)
                ?? throw new Exception("Endereço não encontrado.");

            var enderecoDTO = _mapper.Map<AssociadoDTO>(endereco);
            _enderecoRepository.Delete(enderecoDTO.Id);
        }

        public async Task Edit(EnderecoDTO enderecoDTO)
        {
            var endereco = await _enderecoRepository.Find(enderecoDTO.Id);
            if (!_enderecoRepository.Exist(endereco.Id))
                throw new Exception("Endereço não encontrado.");
            
            _enderecoRepository.Edit(_mapper.Map<Endereco>(enderecoDTO));
        }

        public async Task EditByPlaca(EnderecoDTO enderecoDTO, string placa)
        {
            var veiculo = await _veiculoRepository.GetByPlaca(placa);
            if (veiculo == null)
                throw new Exception("Endereço não encontrado.");

            var endereco = await _enderecoRepository.GetEnderecoByAssociado(veiculo.AssociadoId);
            if (endereco == null)
                throw new Exception("Endereço não encontrado.");

            var entidadeAtualizada = _mapper.Map<Endereco>(enderecoDTO);
            entidadeAtualizada.Id = endereco.Id;
            entidadeAtualizada.AssociadoId = endereco.AssociadoId;

            _enderecoRepository.Edit(entidadeAtualizada);
            await Save();
        }

        public async Task<EnderecoDTO> Get(int id)
        {
            if (!_enderecoRepository.Exist(id))
                throw new Exception("Endereço não encontrado.");

            return _mapper.Map<EnderecoDTO>(await _enderecoRepository.Get(id));
        }

        public async Task<List<EnderecoDTO>> GetAll()
        {
            var result = _mapper.Map<List<EnderecoDTO>>(await _enderecoRepository.GetAll())
                ?? throw new Exception("Nenhum dado encontrado.");

            return result;
        }

        public async Task<bool> Save()
        {
            return await _enderecoRepository.Save();
        }
    }
}

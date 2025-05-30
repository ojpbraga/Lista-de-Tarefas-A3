using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Intefaces;

namespace Application
{
    public class VeiculoApplication : IVeiculoApplication
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IMapper _mapper;
        public VeiculoApplication(IVeiculoRepository veiculoRepository, IMapper mapper)
        {
            _veiculoRepository = veiculoRepository;
            _mapper = mapper;
        }

        public async Task Add(VeiculoDTO veiculoDTO)
        {
            await _veiculoRepository.Add(_mapper.Map<Veiculo>(veiculoDTO));
        }

        public async Task Delete(int id)
        {
            var veiculo = await _veiculoRepository.Find(id);
            if (veiculo == null)
                throw new Exception("Veiculo não encontrado.");
            
            var veiculoDTO = _mapper.Map<VeiculoDTO>(veiculo);
            _veiculoRepository.Delete(veiculoDTO.Id);
        }

        public async Task Edit(VeiculoDTO veiculoDTO)
        {
            var veiculo = await _veiculoRepository.Find(veiculoDTO.Id);

            if (!_veiculoRepository.Exist(veiculo.Id))
                throw new Exception("Veiculo não encontrado.");

            _veiculoRepository.Edit(_mapper.Map<Veiculo>(veiculoDTO));
        }

        public async Task<VeiculoDTO> Get(int id)
        {
            if (!_veiculoRepository.Exist(id))
                throw new Exception("Veiculo não encontrado.");

            return _mapper.Map<VeiculoDTO>(await _veiculoRepository.Get(id));
        }

        public async Task<List<VeiculoDTO>> GetAll()
        {
            var result = _mapper.Map<List<VeiculoDTO>>(await _veiculoRepository.GetAll());
            if (result.Count == 0)
                throw new Exception("Nenhum dado encontrado.");
                
            return result;
        }

        public async Task<bool> Save()
        {
            return await _veiculoRepository.Save();
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Intefaces;

namespace Application
{
    public class AssociadoApplication : IAssociadoApplication
    {
        private readonly IAssociadoRepository _associadoRepository;
        private readonly IMapper _mapper;

        public AssociadoApplication(IAssociadoRepository associadoRepository, IMapper mapper)
        {
            _associadoRepository = associadoRepository;
            _mapper = mapper;
        }

        public async Task Add(AssociadoDTO associadoDTO)
        {
            await _associadoRepository.Add(_mapper.Map<Associado>(associadoDTO));
        }

        public async Task Delete(int id)
        {
            var associado = await _associadoRepository.Find(id);
            if (associado == null)
            {
                throw new Exception("Associado não encontrado.");
            }
            var associadoDTO = _mapper.Map<AssociadoDTO>(associado);
            _associadoRepository.Delete(associadoDTO.Id);
        }

        public async Task Edit(AssociadoDTO associadoDTO)
        {
            var associado = await _associadoRepository.Find(associadoDTO.Id);
            if (!_associadoRepository.Exist(associado.Id))
            {
                throw new Exception("Produto não encontrado.");
            }
            else
                _associadoRepository.Edit(_mapper.Map<Associado>(associadoDTO));
        }

        public async Task<AssociadoDTO> Get(int id)
        {
            if (_associadoRepository.Exist(id))
            {
                return _mapper.Map<AssociadoDTO>(await _associadoRepository.Get(id));
            }
            else
                throw new Exception("Associado não encontrado.");
        }

        public async Task<List<AssociadoDTO>> GetAll()
        {
            var result = _mapper.Map<List<AssociadoDTO>>(await _associadoRepository.GetAll());
            if(result == null)
            {
                throw new Exception("Nenhum dado encontrado.");
            }
            else
                return result;
        }

        public async Task<AssociadoDTO> GetByPlaca(string placa)
        {
            var associado = _mapper.Map<AssociadoDTO>(await _associadoRepository.GetByPlaca(placa));
            if (associado == null)
            {
                throw new Exception("Associado não encontrado.");
            }
            else
                return associado;
        }

        public async Task<bool> Save()
        {
            return await _associadoRepository.Save();
        }
    }
}

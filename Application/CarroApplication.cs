using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Intefaces;

namespace Application
{
    public class CarroApplication : ICarroApplication
    {
        private readonly ICarroRepository _carroRepository;
        private readonly IMapper _mapper;
        public CarroApplication(ICarroRepository carroRepository, IMapper mapper)
        {
            _carroRepository = carroRepository;
            _mapper = mapper;
        }

        public async Task Add(CarroDTO carroDTO)
        {
            await _carroRepository.Add(_mapper.Map<Carro>(carroDTO));
        }

        public async Task Delete(int id)
        {
            var carro = await _carroRepository.Find(id);
            if (carro == null)
            {
                throw new Exception("Carro não encontrado.");
            }
            var carroDTO = _mapper.Map<CarroDTO>(carro);
            _carroRepository.Delete(carroDTO.Id);
        }

        public async Task Edit(CarroDTO carroDTO)
        {
            var carro = await _carroRepository.Find(carroDTO.Id);
            if (!_carroRepository.Exist(carro.Id))
            {
                throw new Exception("Carro não encontrado.");
            }
            else
                _carroRepository.Edit(_mapper.Map<Carro>(carroDTO));
        }

        public async Task<CarroDTO> Get(int id)
        {
            if (_carroRepository.Exist(id))
            {
                return _mapper.Map<CarroDTO>(await _carroRepository.Get(id));
            }
            else
                throw new Exception("Carro não encontrado.");
        }

        public async Task<List<CarroDTO>> GetAll()
        {
            var result = _mapper.Map<List<CarroDTO>>(await _carroRepository.GetAll());
            if (result == null)
            {
                throw new Exception("Nenhum dado encontrado.");
            }
            else
                return result;
        }

        public async Task<bool> Save()
        {
            return await _carroRepository.Save();
        }
    }
}

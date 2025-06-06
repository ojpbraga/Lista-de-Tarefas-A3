﻿using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Intefaces;

namespace Application
{
    public class AssociadoApplication : IAssociadoApplication
    {
        private readonly IAssociadoRepository _associadoRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public AssociadoApplication(
            IAssociadoRepository associadoRepository,
            IVeiculoRepository veiculoRepository,
            IEnderecoRepository enderecoRepository,
            IMapper mapper)
        {
            _associadoRepository = associadoRepository;
            _veiculoRepository = veiculoRepository;
            _enderecoRepository = enderecoRepository;
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
                throw new Exception("Produto não encontrado.");

            _associadoRepository.Edit(_mapper.Map<Associado>(associadoDTO));
        }

        public async Task<AssociadoDTO> Get(int id)
        {
            if (!_associadoRepository.Exist(id))
                throw new Exception("Associado não encontrado.");

            return _mapper.Map<AssociadoDTO>(await _associadoRepository.Get(id));
        }

        public async Task<List<AssociadoDTO>> GetAll()
        {
            var result = _mapper.Map<List<AssociadoDTO>>(await _associadoRepository.GetAll());
            if(result == null)
                throw new Exception("Nenhum dado encontrado.");

            return result;
        }

        public async Task<AssociadoDTO> GetByPlaca(string placa)
        {
            var veiculo = await _veiculoRepository.GetByPlaca(placa);
            if (veiculo == null)
                throw new Exception("Associado não encontrado com a placa informada.");

            var associado = await _associadoRepository.Get(veiculo.AssociadoId);
            if (associado == null)
                throw new Exception("Associado não encontrado com a placa informada.");
                
            var endereco = await _enderecoRepository.GetEnderecoByAssociado(associado.Id);
            if (endereco == null)
                throw new Exception("Associado não encontrado com a placa informada.");

            var resultado = _mapper.Map<AssociadoDTO>(associado);
            resultado.Endereco = _mapper.Map<EnderecoDTO>(endereco);
            resultado.Veiculo = _mapper.Map<VeiculoDTO>(veiculo);

            return resultado;
        }

        public async Task<bool> Save()
        {
            return await _associadoRepository.Save();
        }
    }
}

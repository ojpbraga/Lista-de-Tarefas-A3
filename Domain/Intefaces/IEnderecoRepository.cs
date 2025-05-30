using Domain.Entities;

namespace Domain.Intefaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> GetEnderecoByAssociado(int associadoId);
    }
}

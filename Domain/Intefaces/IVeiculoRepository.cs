using Domain.Entities;

namespace Domain.Intefaces
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        Task<Veiculo> GetByPlaca(string placa);
    }
}

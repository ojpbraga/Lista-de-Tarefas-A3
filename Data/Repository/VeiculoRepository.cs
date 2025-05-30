using Data.Context;
using Domain.Entities;
using Domain.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(CadastroContext cadastroContext): base(cadastroContext)
        {

        }
        public override bool Exist(int id)
        {
            return _cadastroContext.Veiculos.Any(a => a.Id == id);
        }

        public override async Task<Veiculo> Find(int id)
        {
            return await _cadastroContext.Veiculos.FindAsync(id);
        }

        public override async Task<Veiculo> Get(int id)
        {
            return await _cadastroContext.Veiculos.Where(a => a.Id == id).FirstAsync();
        }

        public override async Task<List<Veiculo>> GetAll()
        {
            return await _cadastroContext.Veiculos.ToListAsync();
        }

        public override async void Delete(int id)
        {
            var entity = await _cadastroContext.Veiculos.FindAsync(id);
            _cadastroContext.Veiculos.Remove(entity);
        }

        public async Task<Veiculo> GetByPlaca(string placa)
        {
            return await _cadastroContext.Veiculos.FirstOrDefaultAsync(veiculo => veiculo.Placa == placa);
        }
    }
}

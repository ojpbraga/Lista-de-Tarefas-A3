using Data.Context;
using Domain.Entities;
using Domain.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(CadastroContext cadastroContext) : base(cadastroContext)
        {

        }
        public override bool Exist(int id)
        {
            return _cadastroContext.Enderecos.Any(a => a.Id == id);
        }

        public override async Task<Endereco> Find(int id)
        {
            return await _cadastroContext.Enderecos.FindAsync(id);
        }

        public override async Task<Endereco> Get(int id)
        {
            return await _cadastroContext.Enderecos.Where(a => a.Id == id).FirstAsync();
        }

        public override async Task<List<Endereco>> GetAll()
        {
            return await _cadastroContext.Enderecos.ToListAsync();
        }

        public override async void Delete(int id)
        {
            var entity = await _cadastroContext.Enderecos.FindAsync(id);
            _cadastroContext.Enderecos.Remove(entity);
        }

        public async Task<Endereco> GetEnderecoByAssociado(int associadoId)
        {
            return await _cadastroContext.Enderecos.FirstOrDefaultAsync(end => end.AssociadoId == associadoId);
        }
    }
}

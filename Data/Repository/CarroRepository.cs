using Data.Context;
using Domain.Entities;
using Domain.Intefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CarroRepository : Repository<Carro>, ICarroRepository
    {
        public CarroRepository(CadastroContext cadastroContext): base(cadastroContext)
        {

        }
        public override bool Exist(int id)
        {
            return _cadastroContext.Carros.Any(a => a.Id == id);
        }

        public override async Task<Carro> Find(int id)
        {
            return await _cadastroContext.Carros.FindAsync(id);
        }

        public override async Task<Carro> Get(int id)
        {
            return await _cadastroContext.Carros.Where(a => a.Id == id).FirstAsync();
        }

        public override async Task<List<Carro>> GetAll()
        {
            return await _cadastroContext.Carros.ToListAsync();
        }

        public override async void Delete(int id)
        {
            var entity = await _cadastroContext.Carros.FindAsync(id);
            _cadastroContext.Carros.Remove(entity);
        }
    }
}

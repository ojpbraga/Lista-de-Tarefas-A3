using Data.Context;

namespace Data.Repository
{
    public abstract class Repository<T> where T : class
    {
        protected readonly CadastroContext _cadastroContext;
        public Repository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        public async Task Add(T entity)
        {
            await _cadastroContext.AddAsync(entity);
        }

        public void Edit(T entity)
        {
            _cadastroContext.ChangeTracker.Clear();
            _cadastroContext.Update(entity);
        }

        public virtual void Delete(int id)
        {
            _cadastroContext.Remove(id);
        }

        public async Task<bool> Save()
        {
            return await _cadastroContext.SaveChangesAsync() > 0;
        }

        public abstract bool Exist(int id);
        public abstract Task<T> Get(int id);
        public abstract Task<List<T>> GetAll();
        public abstract Task<T> Find(int id);
    }
}

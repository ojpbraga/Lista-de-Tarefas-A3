using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Intefaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task Add(T entiry);
        void Edit(T entidade);
        void Delete(int id);
        Task<bool> Save();
        bool Exist(int id);
        Task<T> Find(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstWordsAnalyzer.Interfaces
{
    public interface IRepositoryAsynk <T> where T : class
    {
        Task<List<T>> GetAllAsynk();
        Task<T> GetAsynk(int? id);
        Task<List<T>> FindAsynk(Func<T, Boolean> predicate);
        Task<T> Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}

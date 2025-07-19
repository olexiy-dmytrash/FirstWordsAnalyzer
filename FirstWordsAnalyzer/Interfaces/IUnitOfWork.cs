using FirstWordsAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirsWordsAnalyzer.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<WordsPopularityWithCognates> WordsPopularitiesWithCognates { get; }
        void Save();
    }
}

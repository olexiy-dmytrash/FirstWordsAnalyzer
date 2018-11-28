using FirstWordsAnalyzer.Models;
using FirsWordsAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirsWordsAnalyzer.DAL.Repositories
{
    class WordsPopularityWithCognatesRepository : IRepository<WordsPopularityWithCognates2>
    {

        private FirstWordsAnalyzerEntities db;

        public WordsPopularityWithCognatesRepository(FirstWordsAnalyzerEntities context)
        {
            this.db = context;
        }

        public void Create(WordsPopularityWithCognates2 item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WordsPopularityWithCognates2> Find(Func<WordsPopularityWithCognates2, bool> predicate)
        {
            return db.WordsPopularityWithCognates2.Where(predicate).ToList();
        }

        public WordsPopularityWithCognates2 Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WordsPopularityWithCognates2> GetAll()
        {
            return db.WordsPopularityWithCognates2;
        }

        public void Update(WordsPopularityWithCognates2 item)
        {
            throw new NotImplementedException();
        }
    }
}

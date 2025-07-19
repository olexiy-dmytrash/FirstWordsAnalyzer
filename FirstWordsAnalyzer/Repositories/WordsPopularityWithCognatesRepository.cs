using FirstWordsAnalyzer.Models;
using FirsWordsAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirsWordsAnalyzer.DAL.Repositories
{
    class WordsPopularityWithCognatesRepository : IRepository<WordsPopularityWithCognates>
    {

        private FirstWordsAnalyzerEntities db;

        public WordsPopularityWithCognatesRepository(FirstWordsAnalyzerEntities context)
        {
            this.db = context;
        }

        public void Create(WordsPopularityWithCognates item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WordsPopularityWithCognates> Find(Func<WordsPopularityWithCognates, bool> predicate)
        {
            return db.WordsPopularityWithCognates.Where(predicate).ToList();
        }

        public WordsPopularityWithCognates Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WordsPopularityWithCognates> GetAll()
        {
            return db.WordsPopularityWithCognates;
        }

        public void Update(WordsPopularityWithCognates item)
        {
            throw new NotImplementedException();
        }
    }
}

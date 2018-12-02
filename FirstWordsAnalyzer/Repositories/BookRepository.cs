using FirstWordsAnalyzer.Interfaces;
using FirstWordsAnalyzer.Models;
using FirsWordsAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FirstWordsAnalyzer.Repositories
{
    public class BookRepository : IRepositoryAsynk<Book>
    {

        private FirstWordsAnalyzerEntities db;

        public BookRepository(FirstWordsAnalyzerEntities context)
        {
            this.db = context;
        }
        public Task<Book> Create(Book item)
        {
    
            var myTask = Task.Run(() =>
            {
                db.Books.Add(item);
                db.SaveChangesAsync();
                return item;
            });

            return myTask;     
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> FindAsynk(Func<Book, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> GetAllAsynk()
        {
            return db.Books.ToListAsync();
        }

        public Task<Book> GetAsynk(int? id)
        {
            return db.Books.FindAsync(id);
        }

        public void Update(Book item)
        {
            throw new NotImplementedException();
        }
    }
}
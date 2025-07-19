using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstWordsAnalyzer.Models;
using PagedList.Mvc;
using PagedList;
using FirsWordsAnalyzer.DAL.Interfaces;
using FirsWordsAnalyzer.DAL.Repositories;

namespace FirstWordsAnalyzer.Controllers
{
    public class WordsPopularityWithCognates2Controller : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();
        private IRepository<WordsPopularityWithCognates2> repository;

        public WordsPopularityWithCognates2Controller(IRepository<WordsPopularityWithCognates2> moqRepository)
        {
            repository = moqRepository;
        }

        public WordsPopularityWithCognates2Controller()
        {
            this.repository = new WordsPopularityWithCognatesRepository(new FirstWordsAnalyzerEntities());
        }

        // GET: WordsPopularityWithCognates2
        public ActionResult Index(int? page)
        {
            int pageSize = 32;
            int pageNumber = (page ?? 1);
            return View(repository.GetAll().OrderBy(i => i.Quantity).Reverse().ToPagedList(pageNumber, pageSize));
        }

        // GET: WordsPopularityWithCognates2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@wordId", id);
            System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@wordId", id);
            System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@wordId", id);
            var derivedWordsChainWithContext = db.Database.SqlQuery<DerivedWordChainCellWithContext>("GetChainOfDerivedWordsWithContext @wordId", param);

            var basicWordsChainWithContext = db.Database.SqlQuery<DerivedWordChainCellWithContext>("GetChainOfBasicWordsWithContext @wordId", param1);

            var sentances = db.Database.SqlQuery<Sentence>("GetSentencesWithWord @wordId", param2);

            WordsPopularityWithCognates2 wordsPopularityWithCognates2 = db.WordsPopularityWithCognates2.Find(id);

            ViewBag.WordsPopularityWithCognates = wordsPopularityWithCognates2;
            ViewBag.SentencesWithWord = sentances.ToList();
            ViewBag.Id = id;
            //my comment in hotfix

            return View("DerivedWordsChainWithContextDetails", basicWordsChainWithContext.Union(derivedWordsChainWithContext).ToList());
        }

        // GET: WordsPopularityWithCognates2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WordsPopularityWithCognates2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WordId,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,Quantity,Differance")] WordsPopularityWithCognates2 wordsPopularityWithCognates2)
        {
            if (ModelState.IsValid)
            {
                db.WordsPopularityWithCognates2.Add(wordsPopularityWithCognates2);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(wordsPopularityWithCognates2);
        }

        // GET: WordsPopularityWithCognates2/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordsPopularityWithCognates2 wordsPopularityWithCognates2 = await db.WordsPopularityWithCognates2.FindAsync(id);
            if (wordsPopularityWithCognates2 == null)
            {
                return HttpNotFound();
            }
            return View(wordsPopularityWithCognates2);
        }

        // POST: WordsPopularityWithCognates2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WordId,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,Quantity,Differance")] WordsPopularityWithCognates2 wordsPopularityWithCognates2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wordsPopularityWithCognates2).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(wordsPopularityWithCognates2);
        }

        // GET: WordsPopularityWithCognates2/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordsPopularityWithCognates2 wordsPopularityWithCognates2 = await db.WordsPopularityWithCognates2.FindAsync(id);
            if (wordsPopularityWithCognates2 == null)
            {
                return HttpNotFound();
            }
            return View(wordsPopularityWithCognates2);
        }

        // POST: WordsPopularityWithCognates2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WordsPopularityWithCognates2 wordsPopularityWithCognates2 = await db.WordsPopularityWithCognates2.FindAsync(id);
            db.WordsPopularityWithCognates2.Remove(wordsPopularityWithCognates2);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

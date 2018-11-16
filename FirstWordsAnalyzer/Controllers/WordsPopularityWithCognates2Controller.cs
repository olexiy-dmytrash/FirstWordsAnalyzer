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

namespace FirstWordsAnalyzer.Controllers
{
    public class WordsPopularityWithCognates2Controller : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();

        // GET: WordsPopularityWithCognates2
        public ActionResult Index(int? page)
        {
            GetChainOfDerivedWords(12756);
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(db.WordsPopularityWithCognates2.ToList().OrderBy(i => i.Quantity).Reverse().ToPagedList(pageNumber, pageSize));
        }


        private void GetChainOfDerivedWords(int? wordId)
        {
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@wordId", wordId);
            var cognates = db.Database.SqlQuery<DerivedWordChainCell>("GetChainOfDerivedWords @wordId", param);
            foreach (var p in cognates)
                wordId = p.BasicWordId;
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
            var derivedWordsChainWithContext = db.Database.SqlQuery<DerivedWordChainCellWithContext>("GetChainOfDerivedWordsWithContext @wordId", param);

            WordsPopularityWithCognates2 wordsPopularityWithCognates2 = db.WordsPopularityWithCognates2.Find(id);
            ViewBag.WordsPopularityWithCognates = wordsPopularityWithCognates2;

            var sentances = db.Database.SqlQuery<Sentence>("GetSentencesWithWord @wordId", param1);
            ViewBag.SentencesWithWord = sentances.ToList();
            //if (derivedWordsChainWithContext == null || wordsPopularityWithCognates2 == null)
            //{
            //    return HttpNotFound();
            //}
            return View("DerivedWordsChainWithContextDetails", derivedWordsChainWithContext.ToList());
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

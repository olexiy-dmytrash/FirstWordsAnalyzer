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

namespace FirstWordsAnalyzer.Controllers
{
    public class SentencesController : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();

        // GET: Sentences
        public async Task<ActionResult> Index()
        {
            var sentences = db.Sentences.Include(s => s.Book);
            return View(await sentences.ToListAsync());
        }

        // GET: Sentences/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = await db.Sentences.FindAsync(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            return View(sentence);
        }

        // GET: Sentences/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Author");
            return View();
        }

        // POST: Sentences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BookId,Text,Length")] Sentence sentence)
        {
            if (ModelState.IsValid)
            {
                db.Sentences.Add(sentence);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Author", sentence.BookId);
            return View(sentence);
        }

        // GET: Sentences/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = await db.Sentences.FindAsync(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Author", sentence.BookId);
            return View(sentence);
        }

        // POST: Sentences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BookId,Text,Length")] Sentence sentence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sentence).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Author", sentence.BookId);
            return View(sentence);
        }

        // GET: Sentences/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = await db.Sentences.FindAsync(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            return View(sentence);
        }

        // POST: Sentences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sentence sentence = await db.Sentences.FindAsync(id);
            db.Sentences.Remove(sentence);
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

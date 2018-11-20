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
using PagedList;

namespace FirstWordsAnalyzer.Controllers
{
    public class WordsController : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();

        // GET: Words
        public  ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var words = db.Words.Select(w => w);
                           
            if (!String.IsNullOrEmpty(searchString))
            {
                words = db.Words.Where(w => w.Text.Contains(searchString));
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(words.OrderBy(w => w.Text).ToPagedList(pageNumber, pageSize));
        }

        // GET: Words/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // GET: Words/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,NoSense,NotBasic")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.Words.Add(word);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(word);
        }

        // GET: Words/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,NoSense,NotBasic")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.Entry(word).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(word);
        }

        // GET: Words/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Word word = await db.Words.FindAsync(id);
            db.Words.Remove(word);
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

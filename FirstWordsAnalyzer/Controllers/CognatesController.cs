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
    public class CognatesController : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();

        // GET: Cognates
        public ActionResult Index(string currentFilter, string searchString, int? page)
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
            var cognates = db.Cognates.Include(c => c.Word).Include(c => c.Word1);
            if (!String.IsNullOrEmpty(searchString))
            {
                cognates = db.Cognates.Include(c => c.Word).Include(c => c.Word1).Where(c => c.Word.Text.Contains(searchString) || c.Word1.Text.Contains(searchString));
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(cognates.OrderBy(c => c.Word.Text).ToPagedList(pageNumber, pageSize));
        }

        // GET: Cognates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cognate cognate = await db.Cognates.FindAsync(id);
            if (cognate == null)
            {
                return HttpNotFound();
            }
            return View(cognate);
        }

        // GET: Cognates/Create
        public ActionResult Create()
        {
            ViewBag.BasicWordId = new SelectList(db.Words, "Id", "Text");
            ViewBag.DerivedWordId = new SelectList(db.Words, "Id", "Text");
            return View();
        }

        // POST: Cognates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BasicWordId,DerivedWordId,WordPart,WrongAssociation,Id")] Cognate cognate)
        {
            if (ModelState.IsValid)
            {
                db.Cognates.Add(cognate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BasicWordId = new SelectList(db.Words, "Id", "Text", cognate.BasicWordId);
            ViewBag.DerivedWordId = new SelectList(db.Words, "Id", "Text", cognate.DerivedWordId);
            return View(cognate);
        }

        // GET: Cognates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cognate cognate = await db.Cognates.FindAsync(id);
            if (cognate == null)
            {
                return HttpNotFound();
            }
            ViewBag.BasicWordId = new SelectList(db.Words, "Id", "Text", cognate.BasicWordId);
            ViewBag.DerivedWordId = new SelectList(db.Words, "Id", "Text", cognate.DerivedWordId);
            return View(cognate);
        }

        // POST: Cognates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BasicWordId,DerivedWordId,WordPart,WrongAssociation,Id")] Cognate cognate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cognate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BasicWordId = new SelectList(db.Words, "Id", "Text", cognate.BasicWordId);
            ViewBag.DerivedWordId = new SelectList(db.Words, "Id", "Text", cognate.DerivedWordId);
            return View(cognate);
        }

        // GET: Cognates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cognate cognate = await db.Cognates.FindAsync(id);
            if (cognate == null)
            {
                return HttpNotFound();
            }
            return View(cognate);
        }

        // POST: Cognates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cognate cognate = await db.Cognates.FindAsync(id);
            db.Cognates.Remove(cognate);
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

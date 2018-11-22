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
using System.Text;

namespace FirstWordsAnalyzer.Controllers
{
    public class PotentialCognatesController : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();

        // GET: PotentialCognates
        public ActionResult Index()
        {

            return View(CreateCognates());

        }

        public IOrderedEnumerable<PotentialCognate> CreateCognates()
        {
            var potentialCognates = db.PotentialCognates.ToList().OrderBy(c => c.BasicWordText).ThenBy(c => c.DerivedWordText);
            StringBuilder shortPreviousText = new StringBuilder("text for first iteration");
            Cognate cognate = new Cognate();

            foreach (PotentialCognate element in potentialCognates)
            {

                if (!element.BasicWordText.Contains(shortPreviousText.ToString()))
                {
                    cognate = new Cognate();
                    cognate.BasicWordId = (int)element.BasicWordId;
                    cognate.DerivedWordId = element.DerivedWordId;
                    cognate.WordPart = element.DerivedWordText.Substring(element.BasicWordText.Length - 1);
                    db.Cognates.Add(cognate);

                }

                shortPreviousText = new StringBuilder(element.BasicWordText.Substring(0, element.BasicWordText.Length - 1));
            }

            db.SaveChanges();

            return potentialCognates;
        }

        // GET: PotentialCognates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialCognate potentialCognate = await db.PotentialCognates.FindAsync(id);
            if (potentialCognate == null)
            {
                return HttpNotFound();
            }
            return View(potentialCognate);
        }

        // GET: PotentialCognates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PotentialCognates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BasicWordId,DerivedWordId,BasicWordText,DerivedWordText,BasicWordFirstTranslationVariant,DerivedWordFirstTranslationVariant")] PotentialCognate potentialCognate)
        {
            if (ModelState.IsValid)
            {
                db.PotentialCognates.Add(potentialCognate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(potentialCognate);
        }

        // GET: PotentialCognates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialCognate potentialCognate = await db.PotentialCognates.FindAsync(id);
            if (potentialCognate == null)
            {
                return HttpNotFound();
            }
            return View(potentialCognate);
        }

        // POST: PotentialCognates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BasicWordId,DerivedWordId,BasicWordText,DerivedWordText,BasicWordFirstTranslationVariant,DerivedWordFirstTranslationVariant")] PotentialCognate potentialCognate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(potentialCognate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(potentialCognate);
        }

        // GET: PotentialCognates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialCognate potentialCognate = await db.PotentialCognates.FindAsync(id);
            if (potentialCognate == null)
            {
                return HttpNotFound();
            }
            return View(potentialCognate);
        }

        // POST: PotentialCognates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PotentialCognate potentialCognate = await db.PotentialCognates.FindAsync(id);
            db.PotentialCognates.Remove(potentialCognate);
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

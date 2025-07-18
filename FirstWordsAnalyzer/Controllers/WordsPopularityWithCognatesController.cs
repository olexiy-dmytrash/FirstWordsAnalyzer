﻿using System;
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
    public class WordsPopularityWithCognatesController : Controller
    {
        private FirstWordsAnalyzerEntities db = new FirstWordsAnalyzerEntities();
        private IRepository<WordsPopularityWithCognates> repository;

        public WordsPopularityWithCognatesController(IRepository<WordsPopularityWithCognates> moqRepository)
        {
            repository = moqRepository;
        }

        public WordsPopularityWithCognatesController()
        {
            this.repository = new WordsPopularityWithCognatesRepository(new FirstWordsAnalyzerEntities());
        }

        // GET: WordsPopularityWithCognates
        public ActionResult Index(int? page)
        {
            int pageSize = 32;
            int pageNumber = (page ?? 1);
            return View(repository.GetAll().OrderBy(i => i.Quantity).Reverse().ToPagedList(pageNumber, pageSize));
        }

        // GET: WordsPopularityWithCognates/Details/5
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

            WordsPopularityWithCognates WordsPopularityWithCognates = db.WordsPopularityWithCognates.Find(id);

            ViewBag.WordsPopularityWithCognates = WordsPopularityWithCognates;
            ViewBag.SentencesWithWord = sentances.ToList();
            ViewBag.Id = id;
            //my comment in hotfix

            return View("DerivedWordsChainWithContextDetails", basicWordsChainWithContext.Union(derivedWordsChainWithContext).ToList());
        }

        // GET: WordsPopularityWithCognates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WordsPopularityWithCognates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WordId,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,Quantity,Differance")] WordsPopularityWithCognates WordsPopularityWithCognates)
        {
            if (ModelState.IsValid)
            {
                db.WordsPopularityWithCognates.Add(WordsPopularityWithCognates);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(WordsPopularityWithCognates);
        }

        // GET: WordsPopularityWithCognates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordsPopularityWithCognates WordsPopularityWithCognates = await db.WordsPopularityWithCognates.FindAsync(id);
            if (WordsPopularityWithCognates == null)
            {
                return HttpNotFound();
            }
            return View(WordsPopularityWithCognates);
        }

        // POST: WordsPopularityWithCognates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WordId,Text,FirstTranslationVariant,SecondTranslationVariant,ThirdTranslationVariant,Quantity,Differance")] WordsPopularityWithCognates WordsPopularityWithCognates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(WordsPopularityWithCognates).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(WordsPopularityWithCognates);
        }

        // GET: WordsPopularityWithCognates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordsPopularityWithCognates WordsPopularityWithCognates = await db.WordsPopularityWithCognates.FindAsync(id);
            if (WordsPopularityWithCognates == null)
            {
                return HttpNotFound();
            }
            return View(WordsPopularityWithCognates);
        }

        // POST: WordsPopularityWithCognates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WordsPopularityWithCognates WordsPopularityWithCognates = await db.WordsPopularityWithCognates.FindAsync(id);
            db.WordsPopularityWithCognates.Remove(WordsPopularityWithCognates);
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

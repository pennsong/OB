using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OB.Models;
using OB.Models.DAL;
using System.Data.Entity.Infrastructure;

namespace OB.Controllers
{
    public class AccumulationStatusController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /AccumulationStatus/

        public ActionResult Index()
        {
            return View(db.AccumulationStatus.ToList());
        }

        //
        // GET: /AccumulationStatus/Details/5

        public ActionResult Details(int id = 0)
        {
            AccumulationStatus accumulationstatus = db.AccumulationStatus.Find(id);
            if (accumulationstatus == null)
            {
                return HttpNotFound();
            }
            return View(accumulationstatus);
        }

        //
        // GET: /AccumulationStatus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AccumulationStatus/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccumulationStatus accumulationstatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AccumulationStatus.Add(accumulationstatus);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }

            return View(accumulationstatus);
        }

        //
        // GET: /AccumulationStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AccumulationStatus accumulationstatus = db.AccumulationStatus.Find(id);
            if (accumulationstatus == null)
            {
                return HttpNotFound();
            }
            return View(accumulationstatus);
        }

        //
        // POST: /AccumulationStatus/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccumulationStatus accumulationstatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(accumulationstatus).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            return View(accumulationstatus);
        }

        //
        // GET: /AccumulationStatus/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AccumulationStatus accumulationstatus = db.AccumulationStatus.Find(id);
            if (accumulationstatus == null)
            {
                return HttpNotFound();
            }
            return View(accumulationstatus);
        }

        //
        // POST: /AccumulationStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccumulationStatus accumulationstatus = db.AccumulationStatus.Find(id);
            db.AccumulationStatus.Remove(accumulationstatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
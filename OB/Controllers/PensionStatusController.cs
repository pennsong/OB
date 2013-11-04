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
    public class PensionStatusController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /PensionStatus/

        public ActionResult Index()
        {
            return View(db.PensionStatus.ToList());
        }

        //
        // GET: /PensionStatus/Details/5

        public ActionResult Details(int id = 0)
        {
            PensionStatus pensionstatus = db.PensionStatus.Find(id);
            if (pensionstatus == null)
            {
                return HttpNotFound();
            }
            return View(pensionstatus);
        }

        //
        // GET: /PensionStatus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PensionStatus/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PensionStatus pensionstatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PensionStatus.Add(pensionstatus);
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

            return View(pensionstatus);
        }

        //
        // GET: /PensionStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PensionStatus pensionstatus = db.PensionStatus.Find(id);
            if (pensionstatus == null)
            {
                return HttpNotFound();
            }
            return View(pensionstatus);
        }

        //
        // POST: /PensionStatus/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PensionStatus pensionstatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(pensionstatus).State = EntityState.Modified;
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
            return View(pensionstatus);
        }

        //
        // GET: /PensionStatus/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PensionStatus pensionstatus = db.PensionStatus.Find(id);
            if (pensionstatus == null)
            {
                return HttpNotFound();
            }
            return View(pensionstatus);
        }

        //
        // POST: /PensionStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PensionStatus pensionstatus = db.PensionStatus.Find(id);
            db.PensionStatus.Remove(pensionstatus);
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
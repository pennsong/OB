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
    public class TaxTypeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /TaxType/

        public ActionResult Index()
        {
            return View(db.TaxType.ToList());
        }

        //
        // GET: /TaxType/Details/5

        public ActionResult Details(int id = 0)
        {
            TaxType taxtype = db.TaxType.Find(id);
            if (taxtype == null)
            {
                return HttpNotFound();
            }
            return View(taxtype);
        }

        //
        // GET: /TaxType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TaxType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaxType taxtype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.TaxType.Add(taxtype);
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

            return View(taxtype);
        }

        //
        // GET: /TaxType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TaxType taxtype = db.TaxType.Find(id);
            if (taxtype == null)
            {
                return HttpNotFound();
            }
            return View(taxtype);
        }

        //
        // POST: /TaxType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaxType taxtype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(taxtype).State = EntityState.Modified;
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
            return View(taxtype);
        }

        //
        // GET: /TaxType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TaxType taxtype = db.TaxType.Find(id);
            if (taxtype == null)
            {
                return HttpNotFound();
            }
            return View(taxtype);
        }

        //
        // POST: /TaxType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxType taxtype = db.TaxType.Find(id);
            db.TaxType.Remove(taxtype);
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
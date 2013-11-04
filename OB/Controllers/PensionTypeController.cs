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
    public class PensionTypeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /PensionType/

        public ActionResult Index()
        {
            return View(db.PensionType.ToList());
        }

        //
        // GET: /PensionType/Details/5

        public ActionResult Details(int id = 0)
        {
            PensionType pensiontype = db.PensionType.Find(id);
            if (pensiontype == null)
            {
                return HttpNotFound();
            }
            return View(pensiontype);
        }

        //
        // GET: /PensionType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PensionType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PensionType pensiontype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PensionType.Add(pensiontype);
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

            return View(pensiontype);
        }

        //
        // GET: /PensionType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PensionType pensiontype = db.PensionType.Find(id);
            if (pensiontype == null)
            {
                return HttpNotFound();
            }
            return View(pensiontype);
        }

        //
        // POST: /PensionType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PensionType pensiontype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(pensiontype).State = EntityState.Modified;
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
            return View(pensiontype);
        }

        //
        // GET: /PensionType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PensionType pensiontype = db.PensionType.Find(id);
            if (pensiontype == null)
            {
                return HttpNotFound();
            }
            return View(pensiontype);
        }

        //
        // POST: /PensionType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PensionType pensiontype = db.PensionType.Find(id);
            db.PensionType.Remove(pensiontype);
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
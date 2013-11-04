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
    public class HukouTypeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /HukouType/

        public ActionResult Index()
        {
            return View(db.HukouType.ToList());
        }

        //
        // GET: /HukouType/Details/5

        public ActionResult Details(int id = 0)
        {
            HukouType hukoutype = db.HukouType.Find(id);
            if (hukoutype == null)
            {
                return HttpNotFound();
            }
            return View(hukoutype);
        }

        //
        // GET: /HukouType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HukouType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HukouType hukoutype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.HukouType.Add(hukoutype);
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

            return View(hukoutype);
        }

        //
        // GET: /HukouType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HukouType hukoutype = db.HukouType.Find(id);
            if (hukoutype == null)
            {
                return HttpNotFound();
            }
            return View(hukoutype);
        }

        //
        // POST: /HukouType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HukouType hukoutype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(hukoutype).State = EntityState.Modified;
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
            return View(hukoutype);
        }

        //
        // GET: /HukouType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HukouType hukoutype = db.HukouType.Find(id);
            if (hukoutype == null)
            {
                return HttpNotFound();
            }
            return View(hukoutype);
        }

        //
        // POST: /HukouType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HukouType hukoutype = db.HukouType.Find(id);
            db.HukouType.Remove(hukoutype);
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
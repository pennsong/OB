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
    public class DegreeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Degree/

        public ActionResult Index()
        {
            return View(db.Degree.ToList());
        }

        //
        // GET: /Degree/Details/5

        public ActionResult Details(int id = 0)
        {
            Degree degree = db.Degree.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }

        //
        // GET: /Degree/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Degree/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Degree degree)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Degree.Add(degree);
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

            return View(degree);
        }

        //
        // GET: /Degree/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Degree degree = db.Degree.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }

        //
        // POST: /Degree/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Degree degree)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(degree).State = EntityState.Modified;
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
            return View(degree);
        }

        //
        // GET: /Degree/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Degree degree = db.Degree.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }

        //
        // POST: /Degree/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Degree degree = db.Degree.Find(id);
            db.Degree.Remove(degree);
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
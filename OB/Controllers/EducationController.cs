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
    public class EducationController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Education/

        public ActionResult Index()
        {
            var education = db.Education.Include(e => e.Employee);
            return View(education.ToList());
        }

        //
        // GET: /Education/Details/5

        public ActionResult Details(int id = 0)
        {
            Education education = db.Education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        //
        // GET: /Education/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name");
            return View();
        }

        //
        // POST: /Education/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Education education)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Education.Add(education);
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

            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", education.EmployeeId);
            return View(education);
        }

        //
        // GET: /Education/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Education education = db.Education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", education.EmployeeId);
            return View(education);
        }

        //
        // POST: /Education/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Education education)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(education).State = EntityState.Modified;
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
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", education.EmployeeId);
            return View(education);
        }

        //
        // GET: /Education/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Education education = db.Education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        //
        // POST: /Education/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Education education = db.Education.Find(id);
            db.Education.Remove(education);
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
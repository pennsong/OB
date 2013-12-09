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
    [Authorize(Roles = "Admin")]
    public class FamilyController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Family/

        public ActionResult Index()
        {
            var family = db.Family.Include(f => f.Employee);
            return View(family.ToList());
        }

        //
        // GET: /Family/Details/5

        public ActionResult Details(int id = 0)
        {
            Family family = db.Family.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        //
        // GET: /Family/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name");
            return View();
        }

        //
        // POST: /Family/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Family family)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Family.Add(family);
                    db.PPSave();
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

            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", family.EmployeeId);
            return View(family);
        }

        //
        // GET: /Family/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Family family = db.Family.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", family.EmployeeId);
            return View(family);
        }

        //
        // POST: /Family/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Family family)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(family).State = EntityState.Modified;
                    db.PPSave();
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
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", family.EmployeeId);
            return View(family);
        }

        //
        // GET: /Family/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Family family = db.Family.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        //
        // POST: /Family/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Family family = db.Family.Find(id);
            db.Family.Remove(family);
            db.PPSave();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
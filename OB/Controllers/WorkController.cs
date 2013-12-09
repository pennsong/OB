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
    public class WorkController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Work/

        public ActionResult Index()
        {
            var work = db.Work.Include(w => w.Employee);
            return View(work.ToList());
        }

        //
        // GET: /Work/Details/5

        public ActionResult Details(int id = 0)
        {
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        //
        // GET: /Work/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name");
            return View();
        }

        //
        // POST: /Work/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Work work)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Work.Add(work);
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

            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", work.EmployeeId);
            return View(work);
        }

        //
        // GET: /Work/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", work.EmployeeId);
            return View(work);
        }

        //
        // POST: /Work/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Work work)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(work).State = EntityState.Modified;
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
            ViewBag.EmployeeId = new SelectList(db.Employee, "Id", "Name", work.EmployeeId);
            return View(work);
        }

        //
        // GET: /Work/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        //
        // POST: /Work/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Work.Find(id);
            db.Work.Remove(work);
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
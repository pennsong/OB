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
using OB.Lib;

namespace OB.Controllers
{
    public class AccumulationTypeController : Controller
    {
        private OBContext db = new OBContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetAccumulationType";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetAccumulationType(int page = 1, string keyword = "")
        {
            var records = Common.GetAccumulationTypeQuery(db, keyword).OrderBy(a => a.Name);
            var rv = new { keyword = keyword };
            return PartialView(Common<AccumulationType>.Page(this, rv, records, page));
        }


        //
        // GET: /AccumulationType/Details/5

        public ActionResult Details(int id = 0)
        {
            AccumulationType accumulationtype = db.AccumulationType.Find(id);
            if (accumulationtype == null)
            {
                return HttpNotFound();
            }
            return View(accumulationtype);
        }

        //
        // GET: /AccumulationType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AccumulationType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccumulationType accumulationtype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AccumulationType.Add(accumulationtype);
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

            return View(accumulationtype);
        }

        //
        // GET: /AccumulationType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AccumulationType accumulationtype = db.AccumulationType.Find(id);
            if (accumulationtype == null)
            {
                return HttpNotFound();
            }
            return View(accumulationtype);
        }

        //
        // POST: /AccumulationType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccumulationType accumulationtype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(accumulationtype).State = EntityState.Modified;
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
            return View(accumulationtype);
        }

        //
        // GET: /AccumulationType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AccumulationType accumulationtype = db.AccumulationType.Find(id);
            if (accumulationtype == null)
            {
                return HttpNotFound();
            }
            return View(accumulationtype);
        }

        //
        // POST: /AccumulationType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccumulationType accumulationtype = db.AccumulationType.Find(id);
            db.AccumulationType.Remove(accumulationtype);
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
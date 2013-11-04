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
    public class WeightController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Weight/

        public ActionResult Index()
        {
            var weight = db.Weight.Include(w => w.WeightClient);
            return View(weight.ToList());
        }

        //
        // GET: /Weight/Details/5

        public ActionResult Details(int id = 0)
        {
            Weight weight = db.Weight.Find(id);
            if (weight == null)
            {
                return HttpNotFound();
            }
            return View(weight);
        }

        //
        // GET: /Weight/Create

        public ActionResult Create()
        {
            ViewBag.WeightClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /Weight/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Weight weight)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Weight.Add(weight);
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

            ViewBag.WeightClientId = new SelectList(db.Client, "Id", "Name", weight.WeightClientId);
            return View(weight);
        }

        //
        // GET: /Weight/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Weight weight = db.Weight.Find(id);
            if (weight == null)
            {
                return HttpNotFound();
            }
            ViewBag.WeightClientId = new SelectList(db.Client, "Id", "Name", weight.WeightClientId);
            return View(weight);
        }

        //
        // POST: /Weight/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Weight weight)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(weight).State = EntityState.Modified;
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
            ViewBag.WeightClientId = new SelectList(db.Client, "Id", "Name", weight.WeightClientId);
            return View(weight);
        }

        //
        // GET: /Weight/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Weight weight = db.Weight.Find(id);
            if (weight == null)
            {
                return HttpNotFound();
            }
            return View(weight);
        }

        //
        // POST: /Weight/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Weight weight = db.Weight.Find(id);
            db.Weight.Remove(weight);
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
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
    public class ContractTypeController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /ContractType/

        public ActionResult Index()
        {
            var contracttype = db.ContractType.Include(c => c.Client);
            return View(contracttype.ToList());
        }

        //
        // GET: /ContractType/Details/5

        public ActionResult Details(int id = 0)
        {
            ContractType contracttype = db.ContractType.Find(id);
            if (contracttype == null)
            {
                return HttpNotFound();
            }
            return View(contracttype);
        }

        //
        // GET: /ContractType/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /ContractType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContractType contracttype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.ContractType.Add(contracttype);
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

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", contracttype.ClientId);
            return View(contracttype);
        }

        //
        // GET: /ContractType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ContractType contracttype = db.ContractType.Find(id);
            if (contracttype == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", contracttype.ClientId);
            return View(contracttype);
        }

        //
        // POST: /ContractType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractType contracttype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(contracttype).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", contracttype.ClientId);
            return View(contracttype);
        }

        //
        // GET: /ContractType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ContractType contracttype = db.ContractType.Find(id);
            if (contracttype == null)
            {
                return HttpNotFound();
            }
            return View(contracttype);
        }

        //
        // POST: /ContractType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContractType contracttype = db.ContractType.Find(id);
            db.ContractType.Remove(contracttype);
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
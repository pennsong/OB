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
    public class ZhangtaoController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Zhangtao/

        public ActionResult Index()
        {
            var zhangtao = db.Zhangtao.Include(z => z.Client);
            return View(zhangtao.ToList());
        }

        //
        // GET: /Zhangtao/Details/5

        public ActionResult Details(int id = 0)
        {
            Zhangtao zhangtao = db.Zhangtao.Find(id);
            if (zhangtao == null)
            {
                return HttpNotFound();
            }
            return View(zhangtao);
        }

        //
        // GET: /Zhangtao/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        //
        // POST: /Zhangtao/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Zhangtao zhangtao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Zhangtao.Add(zhangtao);
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

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", zhangtao.ClientId);
            return View(zhangtao);
        }

        //
        // GET: /Zhangtao/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Zhangtao zhangtao = db.Zhangtao.Find(id);
            if (zhangtao == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", zhangtao.ClientId);
            return View(zhangtao);
        }

        //
        // POST: /Zhangtao/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Zhangtao zhangtao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(zhangtao).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", zhangtao.ClientId);
            return View(zhangtao);
        }

        //
        // GET: /Zhangtao/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Zhangtao zhangtao = db.Zhangtao.Find(id);
            if (zhangtao == null)
            {
                return HttpNotFound();
            }
            return View(zhangtao);
        }

        //
        // POST: /Zhangtao/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zhangtao zhangtao = db.Zhangtao.Find(id);
            db.Zhangtao.Remove(zhangtao);
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
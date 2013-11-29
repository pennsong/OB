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
using WebMatrix.WebData;

namespace OB.Controllers
{
    public class AssuranceController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /Assurance/

        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index()
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.Action = "GetAssurance";
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetAssurance(int page = 1, string keyword = "")
        {
            var records = Common.GetHRAdminAssuranceQuery(WebSecurity.CurrentUserId, db, keyword);
            records = records.OrderBy(a => a.Name);
            var rv = new { keyword = keyword };
            return PartialView(Common<Assurance>.Page(this, rv, records, page));
        }
        //
        // GET: /Assurance/Details/5

        public ActionResult Details(int id = 0)
        {
            Assurance assurance = db.Assurance.Find(id);
            if (assurance == null)
            {
                return HttpNotFound();
            }
            return View(assurance);
        }

        //
        // GET: /Assurance/Create

        [Authorize(Roles = "HRAdmin")]
        public ActionResult Create()
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.ClientId = new SelectList(Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId), "Id", "Name");
            return View();
        }

        //
        // POST: /Assurance/Create

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Assurance assurance)
        {
            ViewBag.Path1 = "参数设置";
            if (ModelState.IsValid)
            {
                try
                {
                    //确认客户id在用户权限范围内
                    var result = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == assurance.ClientId).SingleOrDefault();
                    if (result == null)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    db.Assurance.Add(assurance);
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
                catch (UnauthorizedAccessException ex)
                {
                    ModelState.AddModelError(string.Empty, "超出权限范围!");
                }
            }

            ViewBag.ClientId = new SelectList(Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId), "Id", "Name", assurance.ClientId);
            return View(assurance);
        }

        //
        // GET: /Assurance/Edit/5
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Path1 = "参数设置";
            Assurance assurance = db.Assurance.Find(id);
            if (assurance == null)
            {
                return HttpNotFound();
            }
            try
            {
                //确认客户id在用户权限范围内
                var result = Common.GetHRAdminClientQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == assurance.ClientId).SingleOrDefault();
                if (result == null)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError(string.Empty, "超出权限范围!");
            }

            return View(assurance);
        }

        //
        // POST: /Assurance/Edit/5
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assurance assurance)
        {
            ViewBag.Path1 = "参数设置";
            if (ModelState.IsValid)
            {
                try
                {
                    //确认客户id在用户权限范围内
                    var resultOld = Common.GetHRAdminAssuranceQuery(WebSecurity.CurrentUserId, db, "").Where(a => a.Id == assurance.Id).SingleOrDefault();
                    if (resultOld == null)
                    {
                        throw new UnauthorizedAccessException();
                    }

                    resultOld.Name = assurance.Name;
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
                catch (UnauthorizedAccessException ex)
                {
                    ModelState.AddModelError(string.Empty, "超出权限范围!");
                }
            }
            return View(assurance);
        }

        //
        // GET: /Assurance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Assurance assurance = db.Assurance.Find(id);
            if (assurance == null)
            {
                return HttpNotFound();
            }
            return View(assurance);
        }

        //
        // POST: /Assurance/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assurance assurance = db.Assurance.Find(id);
            db.Assurance.Remove(assurance);
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
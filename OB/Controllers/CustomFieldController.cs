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
using System.Web.Routing;

namespace OB.Controllers
{
    public class CustomFieldController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /CustomField/
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "GetCustomField" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetCustomField(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Client.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<CustomField>.Page(this, rv, results));
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(CustomField model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            if (ModelState.IsValid)
            {
                try
                {
                    db.CustomField.Add(model);
                    db.PPSave();
                    Common.RMOk(this, "记录:'" + model + "'新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", model);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        //
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(CustomField model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == model.Id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            if (ModelState.IsValid)
            {
                try
                {
                    result.BasicInfo1 = model.BasicInfo1;
                    result.BasicInfo2 = model.BasicInfo2;
                    result.BasicInfo3 = model.BasicInfo3;
                    result.BasicInfo4 = model.BasicInfo4;
                    result.BasicInfo5 = model.BasicInfo5;
                    result.BasicInfo6 = model.BasicInfo6;
                    result.BasicInfo7 = model.BasicInfo7;
                    result.BasicInfo8 = model.BasicInfo8;
                    result.BasicInfo9 = model.BasicInfo9;
                    result.BasicInfo10 = model.BasicInfo10;

                    result.PensionInfo1 = model.PensionInfo1;
                    result.PensionInfo2 = model.PensionInfo2;
                    result.PensionInfo3 = model.PensionInfo3;
                    result.PensionInfo4 = model.PensionInfo4;
                    result.PensionInfo5 = model.PensionInfo5;

                    result.HireInfo1 = model.HireInfo1;
                    result.HireInfo2 = model.HireInfo2;
                    result.HireInfo3 = model.HireInfo3;
                    result.HireInfo4 = model.HireInfo4;
                    result.HireInfo5 = model.HireInfo5;
                    result.HireInfo6 = model.HireInfo6;
                    result.HireInfo7 = model.HireInfo7;
                    result.HireInfo8 = model.HireInfo8;
                    result.HireInfo9 = model.HireInfo9;
                    result.HireInfo10 = model.HireInfo10;
                    result.HireInfo11 = model.HireInfo11;
                    result.HireInfo12 = model.HireInfo12;
                    result.HireInfo13 = model.HireInfo13;
                    result.HireInfo14 = model.HireInfo14;
                    result.HireInfo15 = model.HireInfo15;
                    result.HireInfo16 = model.HireInfo16;
                    result.HireInfo17 = model.HireInfo17;
                    result.HireInfo18 = model.HireInfo18;
                    result.HireInfo19 = model.HireInfo19;
                    result.HireInfo20 = model.HireInfo20;

                    db.PPSave();
                    Common.RMOk(this, "记录:" + model + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", model);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.CustomField.Remove(result);
                db.PPSave();
                Common.RMOk(this, "记录:" + removeName + "删除成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    Common.RMError(this, "记录" + removeName + "被其他记录引用, 不能删除!");
                }
                else
                {
                    Common.RMError(this, "记录" + removeName + "删除失败!");
                }
            }
            return Redirect(Url.Content(returnUrl));
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreSave(CustomField record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminCustomFieldQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            try
            {
                result.IsDeleted = false;
                db.PPSave();
                Common.RMOk(this, "记录:" + result + "恢复成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.RMOk(this, "记录" + result + "恢复失败!" + e.ToString());
            }
            return Redirect(Url.Content(returnUrl));
        }

        [ChildActionOnly]
        public PartialViewResult Abstract(int id)
        {
            var result = db.CustomField.Find(id);
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
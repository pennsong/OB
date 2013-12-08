﻿using System;
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
using OB.Models.ViewModel;

namespace OB.Controllers
{
    public class ClientCitySupplierHukouController : Controller
    {
        private OBContext db = new OBContext();

        //
        // GET: /ClientCitySupplierHukou/
        [Authorize(Roles = "HRAdmin")]
        public ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "参数设置";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "GetClientCitySupplierHukou" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetClientCitySupplierHukou(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var tmpResults = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId, includeSoftDeleted, keyword);
            var results = tmpResults.OrderBy(a => a.HukouType).OrderBy(a => a.Supplier.Name).OrderBy(a => a.City.Name).OrderBy(a => a.Client.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<ClientCitySupplierHukou>.Page(this, rv, results));
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult CreateSave(ClientCitySupplierHukou model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            if (ModelState.IsValid)
            {
                try
                {
                    model.Client = db.Client.Find(model.ClientId);
                    model.City = db.City.Find(model.CityId);
                    model.Supplier = db.Supplier.Find(model.SupplierId);
                    db.ClientCitySupplierHukou.Add(model);
                    db.PPSave();
                    Common.RMOk(this, "记录:'" + model.Name + "'新建成功!");
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
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            var model = new EditClientCitySupplierHukou
            {
                ClientCitySupplierHukouId = result.Id,
                Name = result.Name,
                PensionTypeIds = result.PensionTypes.Select(a => a.Id).ToList(),
                AccumulationTypeIds = result.AccumulationTypes.Select(a => a.Id).ToList(),
            };

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        //
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(EditClientCitySupplierHukou model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId).Include(a => a.PensionTypes).Include(a => a.AccumulationTypes).Where(a => a.Id == model.ClientCitySupplierHukouId).SingleOrDefault();
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
                    if (model.PensionTypeIds == null)
                    {
                        model.PensionTypeIds = new List<int> { };
                    }
                    if (model.AccumulationTypeIds == null)
                    {
                        model.AccumulationTypeIds = new List<int> { };
                    }
                    var pensionTypes = Common.GetPensionTypeQuery(db).Where(a => model.PensionTypeIds.Any(b => b == a.Id)).ToList();
                    var accumulationTypes = Common.GetAccumulationTypeQuery(db).Where(a => model.PensionTypeIds.Any(b => b == a.Id)).ToList();
                    result.PensionTypes = pensionTypes;
                    result.AccumulationTypes = accumulationTypes;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + result.ToString() + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
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
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
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
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            try
            {
                db.ClientCitySupplierHukou.Remove(result);
                db.PPSave();
                Common.RMOk(this, "记录:" + result.ToString() + "删除成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    Common.RMError(this, "记录" + result.ToString() + "被其他记录引用, 不能删除!");
                }
                else
                {
                    Common.RMError(this, "记录" + result.ToString() + "删除失败!");
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
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult RestoreSave(ClientCitySupplierHukou record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "参数设置";
            //检查记录在权限范围内
            var result = Common.GetHRAdminClientCitySupplierHukouQuery(db, WebSecurity.CurrentUserId, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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
                Common.RMOk(this, "记录:" + result.ToString() + "恢复成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.RMOk(this, "记录" + result.ToString() + "恢复失败!" + e.ToString());
            }
            return Redirect(Url.Content(returnUrl));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
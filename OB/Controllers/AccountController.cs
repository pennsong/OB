using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using OB.Filters;
using OB.Models;
using OB.Models.DAL;
using OB.Lib;
using OB.Models.ViewModel;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Web.Routing;

namespace OB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private OBContext db = new OBContext();
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            WebSecurity.Logout();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //检查是否用户被删除
            if (Common.GetUserQuery(db).Where(a => a.Name.ToUpper() == model.UserName.ToUpper()).SingleOrDefault() == null)
            {
            }
            else if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ModelState.AddModelError("", "提供的用户名或密码不正确。");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult HRAdminIndex(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "用户";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "HRAdminIndex" }, { "actionAjax", "GetHRAdmin" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetHRAdmin(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRAdminQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<User>.Page(this, rv, results));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHRAdmin(string returnUrl = "HRAdminIndex")
        {
            ViewBag.Path1 = "用户";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHRAdminSave(RegisterModel model, string returnUrl = "HRAdminIndex")
        {
            ViewBag.Path1 = "用户";
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                try
                {
                    string tmpPassword = GenerateRandomPassword(6);
                    WebSecurity.CreateUserAndAccount(model.UserName, tmpPassword, new { Mail = model.Mail, IsDeleted = false });

                    Roles.AddUsersToRoles(new[] { model.UserName }, new[] { "HRAdmin" });
                    Common.MailTo(model.Mail, "E-Onboarding新建用户通知", "您的用户名:" + model.UserName + ",密码:" + tmpPassword + ",登陆网址:" + "<a href='" + Url.Action("Login", "Account", null, "http") + "'>登陆E-Onboarding</a>");
                    Common.RMOk(this, "记录:'" + model.UserName + "'新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewBag.ReturnUrl = returnUrl;
            return View("CreateHRAdmin", model);
        }

        [Authorize(Roles = "HRAdmin")]
        public ActionResult HRIndex(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "用户";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "HRIndex" }, { "actionAjax", "GetHR" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public PartialViewResult GetHR(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetHRQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<User>.Page(this, rv, results));
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHR(string returnUrl = "HRIndex")
        {
            ViewBag.Path1 = "用户";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHRSave(RegisterModel model, string returnUrl = "HRIndex")
        {
            ViewBag.Path1 = "用户";
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                try
                {
                    string tmpPassword = GenerateRandomPassword(6);
                    WebSecurity.CreateUserAndAccount(model.UserName, tmpPassword, new { Mail = model.Mail, IsDeleted = false });

                    Roles.AddUsersToRoles(new[] { model.UserName }, new[] { "HR" });
                    Common.MailTo(model.Mail, "E-Onboarding新建用户通知", "您的用户名:" + model.UserName + ",密码:" + tmpPassword + ",登陆网址:" + "<a href='" + Url.Action("Login", "Account", null, "http") + "'>登陆E-Onboarding</a>");

                    Common.RMOk(this, "记录:'" + model.UserName + "'新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewBag.ReturnUrl = returnUrl;
            return View("CreateHR", model);
        }

        [Authorize(Roles = "HR")]
        public ActionResult CandidateIndex(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.Path1 = "用户";
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "CandidateIndex" }, { "actionAjax", "GetCandidate" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HR")]
        public PartialViewResult GetCandidate(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetCandidateQuery(db, includeSoftDeleted, keyword);
            results = results.OrderBy(a => a.Name);
            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<User>.Page(this, rv, results));
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCandidate(string returnUrl = "CandidateIndex")
        {
            ViewBag.Path1 = "用户";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCandidateSave(CreateCandidate model, string returnUrl = "CandidateIndex")
        {
            ViewBag.Path1 = "用户";
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        // transaction
                        string tmpPassword = GenerateRandomPassword(6);
                        WebSecurity.CreateUserAndAccount(model.UserName, tmpPassword, new { Mail = model.Mail, IsDeleted = false });

                        Roles.AddUsersToRoles(new[] { model.UserName }, new[] { "Candidate" });

                        var user = db.User.Where(a => a.Name == model.UserName).Single();
                        var employee = new Employee { UserId = user.Id, User = user, EmployeeStatus = EmployeeStatus.新增未通知, ChineseName = model.ChineseName, ClientId = model.ClientId };
                        db.Employee.Add(employee);

                        db.PPSave();

                        Common.MailTo(model.Mail, "E-Onboarding新建用户通知", "您的用户名:" + model.UserName + ",密码:" + tmpPassword + ",登陆网址:" + "<a href='" + Url.Action("Login", "Account", null, "http") + "'>登陆E-Onboarding</a>");

                        scope.Complete();
                        // end transaction
                    }
                    Common.RMOk(this, "记录:'" + model.UserName + "'新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewBag.ReturnUrl = returnUrl;
            return View("CreateCandidate", model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // 只有在当前登录用户是所有者时才取消关联帐户
            if (ownerAccount == User.Identity.Name)
            {
                // 使用事务来防止用户删除其上次使用的登录凭据
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "你的密码,邮箱已更改。"
                : message == ManageMessageId.SetPasswordSuccess ? "已设置你的密码。"
                : message == ManageMessageId.RemoveLoginSuccess ? "已删除外部登录。"
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            LocalPasswordModel localPasswordModel = new LocalPasswordModel { Mail = (db.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single()).Mail };
            return View(localPasswordModel);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // 在某些出错情况下，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                            db.User.Find(WebSecurity.CurrentUserId).Mail = model.Mail;
                            db.PPSave();
                            scope.Complete();
                        }
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("无法创建本地帐户。可能已存在名为“{0}”的帐户。", User.Identity.Name));
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // 如果当前用户已登录，则添加新帐户
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // 该用户是新用户，因此将要求该用户提供所需的成员名称
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // 将新用户插入到数据库
                using (UsersContext db = new UsersContext())
                {
                    User user = db.User.FirstOrDefault(u => u.Name.ToLower() == model.UserName.ToLower());
                    // 检查用户是否已存在
                    if (user == null)
                    {
                        // 将名称插入到配置文件表
                        db.User.Add(new User { Name = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "用户名已存在。请输入其他用户名。");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {
            //check user existance
            var user = Membership.GetUser(UserName);
            if (user == null)
            {
                TempData["Message"] = "用户不存在.";
            }
            else
            {
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(UserName, 15);
                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = UserName, rt = token }, "http") + "'>重置密码</a>";
                //get user emailid
                var emailid = (from i in db.User
                               where i.Name.ToUpper() == UserName.ToUpper()
                               select i.Mail).SingleOrDefault();
                //send mail
                string subject = "E-Onboarding重置密码";
                string body = "<b>请点击以下链接重置密码</b><br/>" + resetLink; //edit it
                try
                {
                    Common.MailTo(emailid, subject, body);
                    TempData["Message"] = "密码重置邮件已发送到:" + emailid + ",请于15分钟内操作.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "邮件发送错误." + ex.Message;
                }
                //only for testing
                //TempData["Message"] = resetLink;
            }

            return View();
        }

        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            var userid = (from i in db.User
                          where i.Name == un
                          select i.Id).FirstOrDefault();
            //check userid and token matches
            bool any = (from j in db.webpages_Memberships
                        where (j.UserId == userid)
                        && (j.PasswordVerificationToken == rt)
                        && (j.PasswordVerificationTokenExpirationDate > DateTime.UtcNow)
                        select j).Any();

            if (any == true)
            {
                //generate random password
                string newpassword = GenerateRandomPassword(6);
                //reset password
                bool response = WebSecurity.ResetPassword(rt, newpassword);
                if (response == true)
                {
                    //get user emailid to send password
                    var emailid = (from i in db.User
                                   where i.Name == un
                                   select i.Mail).FirstOrDefault();
                    //send email
                    string subject = "E-Onboarding新密码";
                    string body = "<b>重置的新密码</b><br/>" + newpassword; //edit it
                    try
                    {
                        Common.MailTo(emailid, subject, body);
                        TempData["Message"] = "重置的新密码已发送到:" + emailid;
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "邮件发送错误." + ex.Message;
                    }

                    //display message
                    TempData["Message"] = "成功! 请检查您的邮箱:" + emailid + ". 您的新密码是 " + newpassword;
                }
                else
                {
                    TempData["Message"] = "非法操作!.";
                }
            }
            else
            {
                TempData["Message"] = "不合法令牌或令牌已过期!.";
            }

            return View();
        }

        #region 帮助程序
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteHRAdmin(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteHRAdminSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.User.Remove(result);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreHRAdmin(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreHRAdminSave(User record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHRAdmin(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHRAdminSave(User model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db).Where(a => a.Id == model.Id).SingleOrDefault();
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
                    result.Mail = model.Mail;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + result + "保存成功!");
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

            return View("EditHRAdmin", model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRAdminDetails(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRAdminQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult DeleteHR(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult DeleteHRSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.User.Remove(result);
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
        public ActionResult RestoreHR(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult RestoreHRSave(User record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHR(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        public ActionResult EditHRSave(User model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db).Where(a => a.Id == model.Id).SingleOrDefault();
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
                    result.Mail = model.Mail;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + result + "保存成功!");
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

            return View("EditHR", model);
        }


        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRDetails(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetHRQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCandidate(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCandidateSave(int id, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end
            var removeName = result.ToString();
            try
            {
                db.User.Remove(result);
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

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreCandidate(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreCandidateSave(User record, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db, true).Where(a => a.IsDeleted == true).Where(a => a.Id == record.Id).SingleOrDefault();
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

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCandidate(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db).Where(a => a.Id == id).SingleOrDefault();
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
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCandidateSave(User model, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db).Where(a => a.Id == model.Id).SingleOrDefault();
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
                    result.Mail = model.Mail;
                    db.PPSave();
                    Common.RMOk(this, "记录:" + result + "保存成功!");
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

            return View("EditCandidate", model);
        }


        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CandidateDetails(int id = 0, string returnUrl = "Index")
        {
            ViewBag.Path1 = "用户";
            //检查记录在权限范围内
            var result = Common.GetCandidateQuery(db).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end

            ViewBag.ReturnUrl = returnUrl;

            return View(result);
        }

        [ChildActionOnly]
        public PartialViewResult HRAdminAbstract(int id)
        {
            var result = Common.GetHRAdminQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            return PartialView(result);
        }

        [ChildActionOnly]
        public PartialViewResult HRAbstract(int id)
        {
            var result = Common.GetHRQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            return PartialView(result);
        }

        [ChildActionOnly]
        public PartialViewResult CandidateAbstract(int id)
        {
            var result = Common.GetCandidateQuery(db, true).Where(a => a.Id == id).SingleOrDefault();
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

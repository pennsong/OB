﻿using FrameLog.Contexts;
using OB.Models;
using OB.Models.DAL;
using OB.Models.FrameLog;
using OB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace OB.Lib
{
    public class Common<T>
    {
        public static IQueryable<T> PageTest(Controller c, RouteValueDictionary rv, IQueryable<T> q, int size = 2)
        {
            var tmpPage = rv.Where(a => a.Key == "page").Select(a => a.Value).SingleOrDefault();
            int page = int.Parse(tmpPage.ToString());
            var tmpTotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            page = page > tmpTotalPage ? tmpTotalPage : page;
            rv.Add("totalPage", tmpTotalPage);
            rv["page"] = page;

            c.ViewBag.RV = rv;
            return q.Skip(((tmpTotalPage > 0 ? page : 1) - 1) * size).Take(size);
        }

        public static IQueryable<T> Page(Controller c, object rv, IQueryable<T> q, int size = 2)
        {
            var rvObj = (object)rv;
            int page = int.Parse(rvObj.GetType().GetProperty("page").GetValue(rvObj, null).ToString());

            var tmpTotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            page = page > tmpTotalPage ? tmpTotalPage : page;

            c.ViewBag.RV = rv;
            return q.Skip(((tmpTotalPage > 0 ? page : 1) - 1) * size).Take(size);
        }
    }

    public class Common
    {
        //带消息提示的返回索引页面
        public static void Rd(Controller controller, MsgType msgType, string msg = "权限范围内没有找到对应记录")
        {
            Msg message = new Msg { MsgType = msgType, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static IQueryable<User> GetUserList(OBContext context, string filter = "")
        {
            filter = filter.ToUpper();
            var usernames = Roles.GetUsersInRole("Candidate");
            var curUserHRClients = context.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);
            var users = from a in context.User
                        join b in context.Employee
                        on a.Id equals b.UserId
                        where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                        select a;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));
            }
            return users;
        }

        public static IQueryable<User> HRCandidateList(OBContext context, string filter = "")
        {
            filter = filter.ToUpper();
            var usernames = Roles.GetUsersInRole("Candidate");
            var curUserHRClients = context.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);
            var users = from a in context.User
                        join b in context.Employee
                        on a.Id equals b.UserId
                        where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                        select a;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));
            }
            return users;
        }

        public static IList<Document> ClientDocumentList(string clientName, OBContext context = null)
        {
            if (context == null)
            {
                using (var context2 = new OBContext())
                {
                    return _ClientDocumentList(clientName, context2);
                }
            }
            else
            {
                return _ClientDocumentList(clientName, context);
            }
        }

        private static IList<Document> _ClientDocumentList(string clientName, OBContext context)
        {
            return context.Document.Where(x => x.Client.Name == clientName).ToList();
        }

        //common get record
        public static List<AccumulationType> GetAccumulationTypeList(OBContext db, string keyword = "")
        {
            return _GetAccumulationType(db, keyword).ToList();
        }

        public static IQueryable<AccumulationType> GetAccumulationTypeQuery(OBContext db, string keyword = "")
        {
            return _GetAccumulationType(db, keyword);
        }

        private static IQueryable<AccumulationType> _GetAccumulationType(OBContext db, string keyword = "")
        {
            keyword = keyword.ToUpper();

            var result = db.AccumulationType.Where(a => a.Name.ToUpper().Contains(keyword));

            return result;
        }

        public static IQueryable<Assurance> GetHRAdminAssuranceQuery(int userHRAdmin, OBContext db = null, string keyword = "")
        {
            if (db == null)
            {
                using (var db2 = new OBContext())
                {
                    return _GetAssurance(db2, keyword, 0, userHRAdmin);
                }
            }
            else
            {
                return _GetAssurance(db, keyword, 0, userHRAdmin);
            }
        }

        public static IQueryable<Assurance> GetClientAssuranceQuery(int client, OBContext db = null)
        {
            if (db == null)
            {
                using (var db2 = new OBContext())
                {
                    return _GetAssurance(db2, "", client, 0);
                }
            }
            else
            {
                return _GetAssurance(db, "", client, 0);
            }
        }

        private static IQueryable<Assurance> _GetAssurance(OBContext db, string keyword = "", int client = 0, int userHRAdmin = 0)
        {
            keyword = keyword.ToUpper();

            var result = db.Assurance.Where(a => a.Name.Contains(keyword) || a.Client.Name.ToUpper().Contains(keyword));

            if (client > 0)
            {
                result = result.Where(a => a.ClientId == 0);
            }
            if (userHRAdmin > 0)
            {
                result = result.Where(a => db.User.Where(c => c.Id == userHRAdmin).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.ClientId));
            }
            return result;
        }

        public static IQueryable<Client> GetClientQuery(OBContext db, bool includeSoftDeleted = false, string keyword = "")
        {
            return _GetClient(db, includeSoftDeleted, 0, keyword);
        }

        public static IQueryable<Client> GetHRAdminClientQuery(OBContext db, int userHRAdmin, string keyword = "")
        {
            return _GetClient(db, false, userHRAdmin, keyword);
        }

        private static IQueryable<Client> _GetClient(OBContext db, bool includeSoftDeleted = false, int userHRAdmin = 0, string keyword = "")
        {
            keyword = keyword.ToUpper();

            var result = db.Client.Where(a => a.Name.Contains(keyword));

            if (userHRAdmin > 0)
            {
                result = result.Where(a => db.User.Where(b => b.Id == userHRAdmin).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.Id));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            return result;
        }

        public static List<User> GetUserList(string role, bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return _GetUser(db, role, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<User> GetUserQuery(OBContext db, string role, bool includeSoftDeleted = false, string filter = "")
        {
            return _GetUser(db, role, includeSoftDeleted, filter);
        }

        private static IQueryable<User> _GetUser(OBContext db, string role, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole(role);

            var result = db.User.Where(x => usernames.Contains(x.Name)).Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }

        // city
        public static List<City> GetCityList(bool includeSoftDeleted = false, string filter = "")
        {
            using (var db = new OBContext())
            {
                return _GetCity(db, includeSoftDeleted, filter).ToList();
            }
        }

        public static IQueryable<City> GetCityQuery(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            return _GetCity(db, includeSoftDeleted, filter);
        }

        private static IQueryable<City> _GetCity(OBContext db, bool includeSoftDeleted = false, string filter = "")
        {
            filter = filter.ToUpper();

            var result = db.City.Where(a => a.Name.ToUpper().Contains(filter));

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }

        //end common get record
    }
}
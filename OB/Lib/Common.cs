using OB.Models;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
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
        public static IQueryable<T> Page(Controller c, string action, object rv, IQueryable<T> q, int page = 1, int size = 2)
        {
            c.ViewBag.Action = action;
            c.ViewBag.TotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            c.ViewBag.Page = page;
            c.ViewBag.RV = rv;
            return q.Skip((page - 1) * size).Take(size);
        }
    }

    public class Common
    {
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

        public static IQueryable<Assurance> GetHRAdminAssurance(int userHRAdmin, OBContext db = null, string keyword = "")
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

        public static IQueryable<Assurance> GetClientAssurance(int client, OBContext db = null)
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

        public static IQueryable<Client> GetClient(OBContext db = null, string keyword = "")
        {
            if (db == null)
            {
                using (var db2 = new OBContext())
                {
                    return _GetClient(db2, keyword);
                }
            }
            else
            {
                return _GetClient(db, keyword);
            }
        }

        public static IQueryable<Client> GetHRAdminClient(int userHRAdmin, OBContext db = null, string keyword = "")
        {
            if (db == null)
            {
                using (var db2 = new OBContext())
                {
                    return _GetClient(db2, keyword, userHRAdmin);
                }
            }
            else
            {
                return _GetClient(db, keyword, userHRAdmin);
            }
        }

        private static IQueryable<Client> _GetClient(OBContext db, string keyword = "", int userHRAdmin = 0)
        {
            keyword = keyword.ToUpper();

            var result = db.Client.Where(a => a.Name.Contains(keyword));

            if (userHRAdmin > 0)
            {
                result = result.Where(a => db.User.Where(b => b.Id == userHRAdmin).FirstOrDefault().HRAdminClients.Select(b => b.Id).Contains(a.Id));
            }

            return result;
        }

        public static List<User> UserList(string role, string filter = "")
        {
            using (var db = new OBContext())
            {
                return _UserQuery(role, db, filter).ToList();
            }
        }

        public static IQueryable<User> UserQuery(string role, OBContext db, string filter = "")
        {
            return _UserQuery(role, db, filter);
        }

        private static IQueryable<User> _UserQuery(string role, OBContext db, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole(role);

            var users = db.User.Where(x => usernames.Contains(x.Name)).Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

            return users;
        }
        //end common get record
    }
}
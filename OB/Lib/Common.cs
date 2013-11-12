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
        public static IQueryable<T> Page(Controller c, string action, object rv, IQueryable<T> q, int page = 1, int size = 10)
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

        public static IQueryable<User> UserList(string role, OBContext context = null, string filter = "")
        {
            if (context == null)
            {
                using (var context2 = new OBContext())
                {
                    return _UserList(role, context2, filter);
                }
            }
            else
            {
                return _UserList(role, context, filter);
            }
        }

        private static IQueryable<User> _UserList(string role, OBContext context, string filter = "")
        {
            filter = filter.ToUpper();

            var usernames = Roles.GetUsersInRole(role);

            var users = context.User.Where(x => usernames.Contains(x.Name)).Where(a => a.Name.ToUpper().Contains(filter) || a.Mail.ToUpper().Contains(filter));

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
    }
}
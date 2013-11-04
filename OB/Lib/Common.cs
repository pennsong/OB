using OB.Models;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace OB.Lib
{
    public class Common
    {
        public static IList<User> HRCandidateList(OBContext context)
        {
            var usernames = Roles.GetUsersInRole("Candidate");
            var curUserHRClients = context.User.Where(a => a.Id == WebSecurity.CurrentUserId).Single().HRClients.Select(a => a.Id);
            var users = from a in context.User
                        join b in context.Employee
                        on a.Id equals b.UserId
                        where usernames.Contains(a.Name) && curUserHRClients.Contains(b.ClientId)
                        select a;

            return users.ToList();
        }

        public static IList<User> UserList(string role, OBContext context = null)
        {
            if (context == null)
            {
                using (var context2 = new OBContext())
                {
                    return _UserList(role, context2);
                }
            }
            else
            {
                return _UserList(role, context);
            }
        }

        private static IList<User> _UserList(string role, OBContext context)
        {
            var usernames = Roles.GetUsersInRole(role);

            return context.User.Where(x => usernames.Contains(x.Name)).ToList();
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
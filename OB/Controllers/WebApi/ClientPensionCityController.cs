using OB.Lib;
using OB.Models;
using OB.Models.DAL;
using OB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OB.Controllers.WebApi
{
    public class ClientPensionCityController : ApiController
    {
        private OBContext db = new OBContext();

        public IEnumerable<IdName> GetClientPensionCity(string id)
        {
            int tmpId = 0;
            int.TryParse(id, out tmpId);
            return Common.GetClientPensionCityQuery(db, tmpId).Select(a => new IdName { Id = a.Id, Name = a.Name });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

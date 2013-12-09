using FrameLog;
using OB.Models.Base;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class ClientPensionCityDocument : SoftDelete, IHasLoggingReference
    {
        public int Id { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }
        [DisplayName("社保城市")]
        public int? PensionCityId { get; set; }

        [DisplayName("资料列表")]
        public virtual ICollection<Document> Documents { get; set; }

        public virtual Client Client { get; set; }
        public virtual City PensionCity { get; set; }

        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }

        public override string ToString()
        {
            if (Client == null)
            {
                using (var db = new OBContext())
                {
                    Client = db.Client.Find(ClientId);
                    PensionCity = db.City.Find(PensionCityId);
                }
            }
            return Client + "_" + PensionCity + "_" + "资料";
        }
    }
}
using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class ClientPensionCityDocument : SoftDelete
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

        public override string ToString()
        {
            return Client.Name + "_" + (PensionCity == null ? "无" : PensionCity.Name);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class ClientCitySupplierHukou
    {
        public int Id { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }
        [DisplayName("城市")]
        public int CityId { get; set; }
        [DisplayName("供应商")]
        public int SupplierId { get; set; }
        [DisplayName("户口性质")]
        public HukouType HukouType { get; set; }

        [DisplayName("社保类型")]
        public virtual ICollection<PensionType> PensionTypes { get; set; }
        [DisplayName("公积金类型")]
        public virtual ICollection<AccumulationType> AccumulationTypes { get; set; }

        public virtual Client Client { get; set; }
        public virtual City City { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
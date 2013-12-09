using FrameLog;
using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class AccumulationType : SoftDelete, IHasLoggingReference
    {
        public AccumulationType()
        {
            ClientCitySupplierHukous = new List<ClientCitySupplierHukou> { };
        }

        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ClientCitySupplierHukou> ClientCitySupplierHukous { get; set; }

        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
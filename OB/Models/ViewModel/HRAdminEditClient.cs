using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class HRAdminEditClient
    {
        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户ID")]
        public int ClientId { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户名称")]
        public string ClientName { get; set; }

        public ICollection<int> HRIds { get; set; }
        public ICollection<int> PensionCities { get; set; }
        public ICollection<int> TaxCities { get; set; }

        public override string ToString()
        {
            return ClientId + "-" + ClientName;
        }
    }
}
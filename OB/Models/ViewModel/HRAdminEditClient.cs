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

        [DisplayName("HR列表")]
        public ICollection<int> HRIds { get; set; }
        [DisplayName("社保城市列表")]
        public ICollection<int> PensionCities { get; set; }
        [DisplayName("计税城市列表")]
        public ICollection<int> TaxCities { get; set; }

        public override string ToString()
        {
            return ClientId + "-" + ClientName;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditClientCitySupplierHukou
    {
        public EditClientCitySupplierHukou()
        {
            PensionTypeIds = new List<int> { };
            AccumulationTypeIds = new List<int> { };
        }


        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户城市供应商户口ID")]
        public int ClientCitySupplierHukouId { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("社保类型")]
        public ICollection<int> PensionTypeIds { get; set; }
        [DisplayName("公积金类型")]
        public ICollection<int> AccumulationTypeIds { get; set; }
    }
}
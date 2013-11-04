using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditClientPensionCityDocument
    {
        [HiddenInput(DisplayValue = false)]
        public int ClientPensionCityDocumentId { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户名称")]
        public string ClientName { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("社保城市")]
        public string PensionCityName { get; set; }

        public ICollection<int> DocumentIds { get; set; }
    }
}
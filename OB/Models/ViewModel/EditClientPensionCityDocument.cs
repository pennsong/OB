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
        public int ClientPensionCityDocumentId { get; set; }

        public int ClientId { get; set; }

        [DisplayName("资料列表")]
        public ICollection<int> DocumentIds { get; set; }
    }
}
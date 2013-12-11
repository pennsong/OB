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
        public HRAdminEditClient()
        {
            HRIds = new List<int> { };
            PensionCities = new List<int> { };
            TaxCities = new List<int> { };
        }

        [DisplayName("客户ID")]
        public int ClientId { get; set; }

        [DisplayName("客户名称")]
        public string ClientName { get; set; }

        [DisplayName("个人资料填写说明")]
        [MaxLength(3000)]
        [DataType(DataType.MultilineText)]
        public string PersonInfoNote { get; set; }

        [DisplayName("教育经历填写说明")]
        [MaxLength(3000)]
        [DataType(DataType.MultilineText)]
        public string EducationNote { get; set; }

        [DisplayName("工作经历填写说明")]
        [MaxLength(3000)]
        [DataType(DataType.MultilineText)]
        public string WorkNote { get; set; }

        [DisplayName("家庭信息填写说明")]
        [MaxLength(3000)]
        [DataType(DataType.MultilineText)]
        public string FamilyNote { get; set; }

        [DisplayName("HR列表")]
        public ICollection<int> HRIds { get; set; }
        [DisplayName("社保城市列表")]
        public ICollection<int> PensionCities { get; set; }
        [DisplayName("计税城市列表")]
        public ICollection<int> TaxCities { get; set; }
    }
}
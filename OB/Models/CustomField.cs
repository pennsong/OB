using FrameLog;
using OB.Models.Base;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class CustomField : SoftDelete, IHasLoggingReference
    {
        public int Id { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息1")]
        public string BasicInfo1 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息2")]
        public string BasicInfo2 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息3")]
        public string BasicInfo3 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息4")]
        public string BasicInfo4 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息5")]
        public string BasicInfo5 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息6")]
        public string BasicInfo6 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息7")]
        public string BasicInfo7 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息8")]
        public string BasicInfo8 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息9")]
        public string BasicInfo9 { get; set; }

        [MaxLength(100)]
        [DisplayName("基本信息10")]
        public string BasicInfo10 { get; set; }

        // 参保信息
        [MaxLength(100)]
        [DisplayName("参保信息1")]
        public string PensionInfo1 { get; set; }

        [MaxLength(100)]
        [DisplayName("参保信息2")]
        public string PensionInfo2 { get; set; }

        [MaxLength(100)]
        [DisplayName("参保信息3")]
        public string PensionInfo3 { get; set; }

        [MaxLength(100)]
        [DisplayName("参保信息4")]
        public string PensionInfo4 { get; set; }

        [MaxLength(100)]
        [DisplayName("参保信息5")]
        public string PensionInfo5 { get; set; }

        // 雇佣信息
        [MaxLength(100)]
        [DisplayName("雇佣信息1")]
        public string HireInfo1 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息2")]
        public string HireInfo2 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息3")]
        public string HireInfo3 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息4")]
        public string HireInfo4 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息5")]
        public string HireInfo5 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息6")]
        public string HireInfo6 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息7")]
        public string HireInfo7 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息8")]
        public string HireInfo8 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息9")]
        public string HireInfo9 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息10")]
        public string HireInfo10 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息11")]
        public string HireInfo11 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息12")]
        public string HireInfo12 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息13")]
        public string HireInfo13 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息14")]
        public string HireInfo14 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息15")]
        public string HireInfo15 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息16")]
        public string HireInfo16 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息17")]
        public string HireInfo17 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息18")]
        public string HireInfo18 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息19")]
        public string HireInfo19 { get; set; }

        [MaxLength(100)]
        [DisplayName("雇佣信息20")]
        public string HireInfo20 { get; set; }

        public virtual Client Client { get; set; }

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
                }
            }
            return Client + "_" + "客户化项目";
        }
    }
}
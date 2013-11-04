using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models
{
    public class EditEmployeeFront
    {
        [HiddenInput(DisplayValue = true)]
        [DisplayName("员工编号")]
        public int EmployeeId { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        // 基本信息
        [Required]
        [MaxLength(100)]
        [DisplayName("英文名")]
        public string EnglishName { get; set; }

        [DisplayName("性别")]
        public int SexId { get; set; }

        [DisplayName("婚姻状况")]
        public int? MarriageId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("国籍")]
        public string Nationality { get; set; }

        [MaxLength(100)]
        [DisplayName("民族")]
        public string Nation { get; set; }

        [DisplayName("证件类型")]
        public int CertificateId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("证件号码")]
        public string CertificateNumber { get; set; }

        [DisplayName("出生日期")]
        public DateTime? Birthday { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("居住地址")]
        public string JuzhuAddress { get; set; }

        [MaxLength(100)]
        [DisplayName("居住地邮编")]
        public string JuzhudiZipCode { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("手机号码1")]
        public string Mobile1 { get; set; }

        [MaxLength(100)]
        [DisplayName("手机号码2")]
        public string Mobile2 { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("紧急联系人")]
        public string EmergencyContract { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("紧急联系人电话")]
        public string EmergencyContractPhone { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("私人邮箱")]
        public string PrivateMail { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("户籍地址")]
        public string HujiAddress { get; set; }

        [MaxLength(100)]
        [DisplayName("户籍地邮编")]
        public string HujiZipCode { get; set; }

        [MaxLength(100)]
        [DisplayName("居住证号码")]
        public string JuzhuzhengNumber { get; set; }

        [DisplayName("居住证到期日")]
        public DateTime? JuzhuzhengDueDate { get; set; }

        [DisplayName("社会工龄起始日期")]
        public DateTime? SocialGonglingStartDate { get; set; }

        [MaxLength(100)]
        [DisplayName("开户银行")]
        public string Bank { get; set; }

        [MaxLength(100)]
        [DisplayName("银行帐号")]
        public string BankAccount { get; set; }

        [MaxLength(100)]
        [DisplayName("开户名")]
        public string BankAccountName { get; set; }

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
        [DisplayName("户口性质")]
        public int HukouTypeId { get; set; }

        [DisplayName("社保状态")]
        public int? PensionStatusId { get; set; }

        [DisplayName("是否办理过医保卡")]
        public bool? YibaokaAvailable { get; set; }

        [DisplayName("公积金状态")]
        public int? AccumulationStatusId { get; set; }

        [MaxLength(100)]
        [DisplayName("本人公积金帐号")]
        public string AccumulationNumber { get; set; }

        [MaxLength(100)]
        [DisplayName("档案所在地")]
        public string DanganAddress { get; set; }

        [MaxLength(100)]
        [DisplayName("档案存放机构名称")]
        public string DanganOrganization { get; set; }

        [MaxLength(100)]
        [DisplayName("档案保管号")]
        public string DanganNumber { get; set; }

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

        public virtual Employee Employee { get; set; }
    }
}
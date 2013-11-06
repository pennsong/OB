using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data.Entity;


namespace OB.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        [DisplayName("用户名")]
        public int UserId { get; set; }

        [Required]
        [DisplayName("员工状态")]
        public EmployeeStatus EmployeeStatus { get; set; }

        [MaxLength(1000)]
        [DisplayName("Offer")]
        public string OfferContent { get; set; }

        // 基本信息
        [Required]
        [MaxLength(100)]
        [DisplayName("中文名")]
        public string ChineseName { get; set; }

        [MaxLength(100)]
        [DisplayName("英文名")]
        public string EnglishName { get; set; }

        [DisplayName("性别")]
        public int? SexId { get; set; }

        [DisplayName("婚姻状况")]
        public int? MarriageId { get; set; }

        [MaxLength(100)]
        [DisplayName("国籍")]
        public string Nationality { get; set; }

        [MaxLength(100)]
        [DisplayName("民族")]
        public string Nation { get; set; }

        [DisplayName("证件类型")]
        public int? CertificateId { get; set; }

        [MaxLength(100)]
        [DisplayName("证件号码")]
        public string CertificateNumber { get; set; }

        [DisplayName("出生日期")]
        public DateTime? Birthday { get; set; }

        [MaxLength(100)]
        [DisplayName("居住地址")]
        public string JuzhuAddress { get; set; }

        [MaxLength(100)]
        [DisplayName("居住地邮编")]
        public string JuzhudiZipCode { get; set; }

        [MaxLength(100)]
        [DisplayName("手机号码1")]
        public string Mobile1 { get; set; }

        [MaxLength(100)]
        [DisplayName("手机号码2")]
        public string Mobile2 { get; set; }

        [MaxLength(100)]
        [DisplayName("紧急联系人")]
        public string EmergencyContract { get; set; }

        [MaxLength(100)]
        [DisplayName("紧急联系人电话")]
        public string EmergencyContractPhone { get; set; }

        [MaxLength(100)]
        [DisplayName("私人邮箱")]
        public string PrivateMail { get; set; }

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

        [MaxLength(1000)]
        [DisplayName("员工备注")]
        public string EmployeeNote { get; set; }

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
        public int? HukouTypeId { get; set; }

        [DisplayName("参保城市")]
        public int? PensionCityId { get; set; }

        [DisplayName("公积金城市")]
        public int? AccumulationCityId { get; set; }

        [DisplayName("社保类型")]
        public int? PensionTypeId { get; set; }

        [DisplayName("公积金类型")]
        public int? AccumulationTypeId { get; set; }

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
        [DisplayName("所属客户")]
        public int ClientId { get; set; }

        [MaxLength(100)]
        [DisplayName("外派客户")]
        public string WorkClient { get; set; }

        [MaxLength(100)]
        [DisplayName("公司邮箱")]
        public string CompanyMail { get; set; }

        [DisplayName("入职日期")]
        public DateTime? EnterDate { get; set; }

        [DisplayName("试用期到期日")]
        public DateTime? ProbationDueDate { get; set; }

        [DisplayName("入职客户方日期")]
        public DateTime? EnterClientDate { get; set; }

        [DisplayName("公司工龄调整(年)")]
        public decimal? CompanyYearAdjust { get; set; }

        [DisplayName("社会工龄调整(年)")]
        public decimal? SocialYearAdjust { get; set; }

        [DisplayName("年假天数")]
        public decimal? VacationDays { get; set; }

        [DisplayName("工作城市")]
        public int? WorkCityId { get; set; }

        [DisplayName("部门")]
        public int? DepartmentId { get; set; }

        [DisplayName("职级")]
        public int? LevelId { get; set; }

        [DisplayName("职位")]
        public int? PositionId { get; set; }

        [MaxLength(100)]
        [DisplayName("合同编号")]
        public string ContractNumber { get; set; }

        [DisplayName("合同开始日期")]
        public DateTime? ContractBeginDate { get; set; }

        [DisplayName("合同终止日期")]
        public DateTime? ContractEndDate { get; set; }

        [DisplayName("合同类型")]
        public int? ContractTypeId { get; set; }

        [DisplayName("试用期工资")]
        public decimal? ProbationSalary { get; set; }

        [DisplayName("基本工资")]
        public decimal? Salary { get; set; }

        private DateTime? _PensionStartMonth { get; set; }
        [DisplayName("社保起缴月份")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "日期格式不正确")]
        public DateTime? PensionStartMonth
        {
            get
            {
                return _PensionStartMonth;
            }
            set
            {
                if (value != null)
                {
                    _PensionStartMonth = new DateTime(value.Value.Year, value.Value.Month, 1);
                }
                else
                {
                    _PensionStartMonth = null;
                }
            }
        }

        private DateTime? _AccumulationStartMonth { get; set; }
        [DisplayName("社保起缴月份")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "日期格式不正确")]
        public DateTime? AccumulationStartMonth
        {
            get
            {
                return _AccumulationStartMonth;
            }
            set
            {
                if (value != null)
                {
                    _AccumulationStartMonth = new DateTime(value.Value.Year, value.Value.Month, 1);
                }
                else
                {
                    _AccumulationStartMonth = null;
                }
            }
        }

        [DisplayName("是否公司承担个人福利")]
        public bool? PayByCompany { get; set; }

        [DisplayName("养老基数")]
        public decimal? Yljs { get; set; }

        [DisplayName("失业基数")]
        public decimal? Syjs { get; set; }

        [DisplayName("医疗基数")]
        public decimal? Yiliaojs { get; set; }

        [DisplayName("工伤基数")]
        public decimal? Gsjs { get; set; }

        [DisplayName("生育基数")]
        public decimal? Shengyujs { get; set; }

        [DisplayName("其他基数")]
        public decimal? Qtjs { get; set; }

        [DisplayName("补充基数")]
        public decimal? Bcjs { get; set; }

        [DisplayName("公积金基数")]
        public decimal? Gjjjs { get; set; }

        [DisplayName("补充公积金基数")]
        public decimal? Bcgjjjs { get; set; }

        [DisplayName("纳税类型")]
        public int? TaxTypeId { get; set; }

        [DisplayName("薪资账套")]
        public int? ZhangtaoId { get; set; }

        [DisplayName("报税城市")]
        public int? TaxCityId { get; set; }

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

        [DisplayName("完成度")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal Percent
        {
            get
            {
                // 取权重
                Weight weight;
                using (var db = new OBContext())
                {
                    weight = db.Weight.Where(a => a.WeightClientId == ClientId).Single();
                    if (weight == null)
                    {
                        weight = db.Weight.Where(a => a.WeightClientId == null).Single();
                    }
                }

                // 取得普通权重
                PropertyInfo[] fs = typeof(Weight).GetProperties();
                decimal count = 0;
                decimal total = 0;
                string[] exclude = { "Id", "WeightClientId", "WeightClient" };
                string[] collection = { "Educations", "Works", "Families" };
                foreach (PropertyInfo item in fs)
                {
                    if (exclude.Contains(item.Name))
                    {
                        continue;
                    }
                    else
                    {
                        var t = typeof(Employee).GetProperty(item.Name);
                        var v = t.GetValue(this);
                        if ((collection.Contains(item.Name) && Convert.ToInt32(v.GetType().GetProperty("Count").GetValue(v)) > 0) || (!(collection.Contains(item.Name)) && v != null))
                        {
                            count += Convert.ToInt32(typeof(Weight).GetProperty(item.Name).GetValue(weight));
                        }
                    }
                    total += Convert.ToInt32(typeof(Weight).GetProperty(item.Name).GetValue(weight));
                }

                // 取得上传文件权重
                var clientPensionCityDocument = Client.ClientPensionCityDocuments.Where(a => (a.PensionCityId == null && PensionCityId == null) || (a.PensionCityId == PensionCityId)).SingleOrDefault();
                if (clientPensionCityDocument == null)
                {
                    return 0;
                }
                foreach (var item in clientPensionCityDocument.Documents)
                {
                    var doc = EmployeeDocs.Where(a => a.DocumentId == item.Id).SingleOrDefault();
                    if (doc != null && !String.IsNullOrWhiteSpace(doc.ImgPath))
                    {
                        count += item.Weight;
                    }
                    total += item.Weight;
                }

                if (total == 0)
                {
                    return 0;
                }
                else
                {
                    return (count / total);
                }
            }
        }

        public virtual User User { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual Marriage Marriage { get; set; }
        public virtual Certificate Certificate { get; set; }
        public virtual HukouType HukouType { get; set; }
        public virtual City PensionCity { get; set; }
        public virtual City AccumulationCity { get; set; }
        public virtual PensionType PensionType { get; set; }
        public virtual AccumulationType AccumulationType { get; set; }
        public virtual PensionStatus PensionStatus { get; set; }
        public virtual AccumulationStatus AccumulationStatus { get; set; }
        public virtual Client Client { get; set; }
        public virtual City WorkCity { get; set; }
        public virtual Department Department { get; set; }
        public virtual Level Level { get; set; }
        public virtual Position Position { get; set; }
        public virtual ContractType ContractType { get; set; }
        public virtual TaxType TaxType { get; set; }
        public virtual Zhangtao Zhangtao { get; set; }
        public virtual City TaxCity { get; set; }

        public virtual ICollection<Family> Families { get; set; }
        public virtual ICollection<Work> Works { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<BudgetCenter> BudgetCenters { get; set; }
        public virtual ICollection<Assurance> Assurances { get; set; }
        public virtual ICollection<EmployeeDoc> EmployeeDocs { get; set; }
    }
}
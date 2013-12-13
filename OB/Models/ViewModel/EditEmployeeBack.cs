using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models.ViewModel
{
    public class EditEmployeeBack
    {
        public EditEmployeeBack()
        {
            BudgetCenterIds = new List<int> { };
            AssuranceIds = new List<int> { };
        }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("员工ID")]
        public int EmployeeId { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        // 基本信息
        [Required]
        [MaxLength(100)]
        [DisplayName("中文名")]
        public string ChineseName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("手机号码1")]
        public string Mobile1 { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("私人邮箱")]
        public string PrivateMail { get; set; }

        [MaxLength(1000)]
        [DisplayName("员工备注")]
        public string EmployeeNote { get; set; }

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
        [DisplayName("参保城市")]
        public int PensionCityId { get; set; }

        [DisplayName("公积金城市")]
        public int AccumulationCityId { get; set; }

        [DisplayName("社保类型")]
        public int PensionTypeId { get; set; }

        [DisplayName("公积金类型")]
        public int AccumulationTypeId { get; set; }

        // 雇佣信息
        [MaxLength(100)]
        [DisplayName("外派客户")]
        public string WorkClient { get; set; }

        [MaxLength(100)]
        [DisplayName("公司邮箱")]
        public string CompanyMail { get; set; }

        [Required]
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
        public int DepartmentId { get; set; }

        [DisplayName("职级")]
        public int? LevelId { get; set; }

        [DisplayName("职位")]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("合同编号")]
        public string ContractNumber { get; set; }

        [Required]
        [DisplayName("合同开始日期")]
        public DateTime? ContractBeginDate { get; set; }

        [Required]
        [DisplayName("合同终止日期")]
        public DateTime? ContractEndDate { get; set; }

        [DisplayName("合同类型")]
        public int ContractTypeId { get; set; }

        [Required]
        [DisplayName("试用期工资")]
        public decimal? ProbationSalary { get; set; }

        [Required]
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

        [Required]
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
        public TaxType? TaxType { get; set; }

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

        public virtual Employee Employee { get; set; }

        public ICollection<int> BudgetCenterIds { get; set; }
        public ICollection<int> AssuranceIds { get; set; }
    }
}
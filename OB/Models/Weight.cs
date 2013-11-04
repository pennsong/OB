using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Weight
    {
        public int Id { get; set; }

        [DisplayName("客户")]
        [DisplayFormat(NullDisplayText = "默认")]
        public int? WeightClientId { get; set; }

        // 多条信息
        [DisplayName("教育信息")]
        public int? Educations { get; set; }
        [DisplayName("工作经历")]
        public int? Works { get; set; }
        [DisplayName("家庭信息")]
        public int? Families { get; set; }

        // 基本信息
        [DisplayName("中文名")]
        public int? ChineseName { get; set; }

        [DisplayName("英文名")]
        public int? EnglishName { get; set; }

        [DisplayName("性别")]
        public int? SexId { get; set; }

        [DisplayName("婚姻状况")]
        public int? MarriageId { get; set; }

        [DisplayName("国籍")]
        public int? Nationality { get; set; }

        [DisplayName("民族")]
        public int? Nation { get; set; }

        [DisplayName("证件类型")]
        public int? CertificateId { get; set; }

        [DisplayName("证件号码")]
        public int? CertificateNumber { get; set; }

        [DisplayName("出生日期")]
        public int? Birthday { get; set; }

        [DisplayName("居住地址")]
        public int? JuzhuAddress { get; set; }

        [DisplayName("居住地邮编")]
        public int? JuzhudiZipCode { get; set; }

        [DisplayName("手机号码1")]
        public int? Mobile1 { get; set; }

        [DisplayName("手机号码2")]
        public int? Mobile2 { get; set; }

        [DisplayName("紧急联系人")]
        public int? EmergencyContract { get; set; }

        [DisplayName("紧急联系人电话")]
        public int? EmergencyContractPhone { get; set; }

        [DisplayName("私人邮箱")]
        public int? PrivateMail { get; set; }

        [DisplayName("户籍地址")]
        public int? HujiAddress { get; set; }

        [DisplayName("户籍地邮编")]
        public int? HujiZipCode { get; set; }

        [DisplayName("居住证号码")]
        public int? JuzhuzhengNumber { get; set; }

        [DisplayName("居住证到期日")]
        public int? JuzhuzhengDueDate { get; set; }

        [DisplayName("社会工龄起始日期")]
        public int? SocialGonglingStartDate { get; set; }

        [DisplayName("开户银行")]
        public int? Bank { get; set; }

        [DisplayName("银行帐号")]
        public int? BankAccount { get; set; }

        [DisplayName("开户名")]
        public int? BankAccountName { get; set; }

        [DisplayName("员工备注")]
        public int? EmployeeNote { get; set; }

        [DisplayName("基本信息1")]
        public int? BasicInfo1 { get; set; }

        [DisplayName("基本信息2")]
        public int? BasicInfo2 { get; set; }

        [DisplayName("基本信息3")]
        public int? BasicInfo3 { get; set; }

        [DisplayName("基本信息4")]
        public int? BasicInfo4 { get; set; }

        [DisplayName("基本信息5")]
        public int? BasicInfo5 { get; set; }

        [DisplayName("基本信息6")]
        public int? BasicInfo6 { get; set; }

        [DisplayName("基本信息7")]
        public int? BasicInfo7 { get; set; }

        [DisplayName("基本信息8")]
        public int? BasicInfo8 { get; set; }

        [DisplayName("基本信息9")]
        public int? BasicInfo9 { get; set; }

        [DisplayName("基本信息10")]
        public int? BasicInfo10 { get; set; }

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
        public int? YibaokaAvailable { get; set; }

        [DisplayName("公积金状态")]
        public int? AccumulationStatusId { get; set; }

        [DisplayName("本人公积金帐号")]
        public int? AccumulationNumber { get; set; }

        [DisplayName("档案所在地")]
        public int? DanganAddress { get; set; }

        [DisplayName("档案存放机构名称")]
        public int? DanganOrganization { get; set; }

        [DisplayName("档案保管号")]
        public int? DanganNumber { get; set; }

        [DisplayName("参保信息1")]
        public int? PensionInfo1 { get; set; }

        [DisplayName("参保信息2")]
        public int? PensionInfo2 { get; set; }

        [DisplayName("参保信息3")]
        public int? PensionInfo3 { get; set; }

        [DisplayName("参保信息4")]
        public int? PensionInfo4 { get; set; }

        [DisplayName("参保信息5")]
        public int? PensionInfo5 { get; set; }

        // 雇佣信息
        [DisplayName("所属客户")]
        public int? ClientId { get; set; }

        [DisplayName("外派客户")]
        public int? WorkClient { get; set; }

        [DisplayName("公司邮箱")]
        public int? CompanyMail { get; set; }

        [DisplayName("入职日期")]
        public int? EnterDate { get; set; }

        [DisplayName("试用期到期日")]
        public int? ProbationDueDate { get; set; }

        [DisplayName("入职客户方日期")]
        public int? EnterClientDate { get; set; }

        [DisplayName("公司工龄调整(年)")]
        public int? CompanyYearAdjust { get; set; }

        [DisplayName("社会工龄调整(年)")]
        public int? SocialYearAdjust { get; set; }

        [DisplayName("年假天数")]
        public int? VacationDays { get; set; }

        [DisplayName("工作城市")]
        public int? WorkCityId { get; set; }

        [DisplayName("部门")]
        public int? DepartmentId { get; set; }

        [DisplayName("职级")]
        public int? LevelId { get; set; }

        [DisplayName("职位")]
        public int? PositionId { get; set; }

        [DisplayName("合同编号")]
        public int? ContractNumber { get; set; }

        [DisplayName("合同开始日期")]
        public int? ContractBeginDate { get; set; }

        [DisplayName("合同终止日期")]
        public int? ContractEndDate { get; set; }

        [DisplayName("合同类型")]
        public int? ContractTypeId { get; set; }

        [DisplayName("试用期工资")]
        public int? ProbationSalary { get; set; }

        [DisplayName("基本工资")]
        public int? Salary { get; set; }

        [DisplayName("社保起缴月份")]
        public int? PensionStartMonth { get; set; }

        [DisplayName("社保起缴月份")]
        public int? AccumulationStartMonth { get; set; }

        [DisplayName("是否公司承担个人福利")]
        public int? PayByCompany { get; set; }

        [DisplayName("养老基数")]
        public int? Yljs { get; set; }

        [DisplayName("失业基数")]
        public int? Syjs { get; set; }

        [DisplayName("医疗基数")]
        public int? Yiliaojs { get; set; }

        [DisplayName("工伤基数")]
        public int? Gsjs { get; set; }

        [DisplayName("生育基数")]
        public int? Shengyujs { get; set; }

        [DisplayName("其他基数")]
        public int? Qtjs { get; set; }

        [DisplayName("补充基数")]
        public int? Bcjs { get; set; }

        [DisplayName("公积金基数")]
        public int? Gjjjs { get; set; }

        [DisplayName("补充公积金基数")]
        public int? Bcgjjjs { get; set; }

        [DisplayName("纳税类型")]
        public int? TaxTypeId { get; set; }

        [DisplayName("薪资账套")]
        public int? ZhangtaoId { get; set; }

        [DisplayName("报税城市")]
        public int? TaxCityId { get; set; }

        [DisplayName("雇佣信息1")]
        public int? HireInfo1 { get; set; }

        [DisplayName("雇佣信息2")]
        public int? HireInfo2 { get; set; }

        [DisplayName("雇佣信息3")]
        public int? HireInfo3 { get; set; }

        [DisplayName("雇佣信息4")]
        public int? HireInfo4 { get; set; }

        [DisplayName("雇佣信息5")]
        public int? HireInfo5 { get; set; }

        [DisplayName("雇佣信息6")]
        public int? HireInfo6 { get; set; }

        [DisplayName("雇佣信息7")]
        public int? HireInfo7 { get; set; }

        [DisplayName("雇佣信息8")]
        public int? HireInfo8 { get; set; }

        [DisplayName("雇佣信息9")]
        public int? HireInfo9 { get; set; }

        [DisplayName("雇佣信息10")]
        public int? HireInfo10 { get; set; }

        [DisplayName("雇佣信息11")]
        public int? HireInfo11 { get; set; }

        [DisplayName("雇佣信息12")]
        public int? HireInfo12 { get; set; }

        [DisplayName("雇佣信息13")]
        public int? HireInfo13 { get; set; }

        [DisplayName("雇佣信息14")]
        public int? HireInfo14 { get; set; }

        [DisplayName("雇佣信息15")]
        public int? HireInfo15 { get; set; }

        [DisplayName("雇佣信息16")]
        public int? HireInfo16 { get; set; }

        [DisplayName("雇佣信息17")]
        public int? HireInfo17 { get; set; }

        [DisplayName("雇佣信息18")]
        public int? HireInfo18 { get; set; }

        [DisplayName("雇佣信息19")]
        public int? HireInfo19 { get; set; }

        [DisplayName("雇佣信息20")]
        public int? HireInfo20 { get; set; }

        public virtual Client WeightClient { get; set; }
    }
}
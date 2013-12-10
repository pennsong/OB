using FrameLog;
using FrameLog.Contexts;
using OB.Models.Base;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Client : SoftDelete, IHasLoggingReference
    {
        public Client()
        {
            HRs = new List<User> { };

            WorkCities = new List<City> { };
            TaxCities = new List<City> { };
            PensionCities = new List<City> { };
            AccumulationCities = new List<City> { };

            Assurances = new List<Assurance> { };
            BudgetCenters = new List<BudgetCenter> { };
            ContractTypes = new List<ContractType> { };
            Departments = new List<Department> { };
            Documents = new List<Document> { };
            Levels = new List<Level> { };
            Positions = new List<Position> { };
            Zhangtaos = new List<Zhangtao> { };

            ClientPensionCityDocuments = new List<ClientPensionCityDocument> { };
        }

        [DisplayName("ID")]
        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("HRAdmin")]
        public int? HRAdminId { get; set; }

        [ForeignKey("HRAdminId")]
        public virtual User HRAdmin { get; set; }

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
        public virtual ICollection<User> HRs { get; set; }

        [DisplayName("工作城市列表")]
        public virtual ICollection<City> WorkCities { get; set; }
        [DisplayName("计税城市列表")]
        public virtual ICollection<City> TaxCities { get; set; }
        [DisplayName("社保城市列表")]
        public virtual ICollection<City> PensionCities { get; set; }
        [DisplayName("公积金城市列表")]
        public virtual ICollection<City> AccumulationCities { get; set; }

        [DisplayName("商业保险列表")]
        public virtual ICollection<Assurance> Assurances { get; set; }
        [DisplayName("成本中心列表")]
        public virtual ICollection<BudgetCenter> BudgetCenters { get; set; }
        [DisplayName("合同类型列表")]
        public virtual ICollection<ContractType> ContractTypes { get; set; }
        [DisplayName("部门列表")]
        public virtual ICollection<Department> Departments { get; set; }
        [DisplayName("资料列表")]
        public virtual ICollection<Document> Documents { get; set; }
        [DisplayName("职级列表")]
        public virtual ICollection<Level> Levels { get; set; }
        [DisplayName("职位列表")]
        public virtual ICollection<Position> Positions { get; set; }
        [DisplayName("账套列表")]
        public virtual ICollection<Zhangtao> Zhangtaos { get; set; }

        [DisplayName("社保城市材料列表")]
        public virtual ICollection<ClientPensionCityDocument> ClientPensionCityDocuments { get; set; }

        public List<User> GetHRs()
        {
            return HRs.Where(a => a.IsDeleted == false).ToList();
        }

        public List<City> GetWorkCities()
        {
            return WorkCities.Where(a => a.IsDeleted == false).ToList();
        }

        public List<City> GetTaxCities()
        {
            return TaxCities.Where(a => a.IsDeleted == false).ToList();
        }

        public List<City> GetPensionCities()
        {
            return PensionCities.Where(a => a.IsDeleted == false).ToList();
        }

        public List<City> GetAccumulationCities()
        {
            return AccumulationCities.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Assurance> GetAssurances()
        {
            return Assurances.Where(a => a.IsDeleted == false).ToList();
        }

        public List<BudgetCenter> GetBudgetCenters()
        {
            return BudgetCenters.Where(a => a.IsDeleted == false).ToList();
        }

        public List<ContractType> GetContractTypes()
        {
            return ContractTypes.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Department> GetDepartments()
        {
            return Departments.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Document> GetDocuments()
        {
            return Documents.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Level> GetLevels()
        {
            return Levels.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Position> GetPositions()
        {
            return Positions.Where(a => a.IsDeleted == false).ToList();
        }

        public List<Zhangtao> GetZhangtaos()
        {
            return Zhangtaos.Where(a => a.IsDeleted == false).ToList();
        }

        public List<ClientPensionCityDocument> GetClientPensionCityDocuments()
        {
            return ClientPensionCityDocuments.Where(a => a.IsDeleted == false).ToList();
        }


        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
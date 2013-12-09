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

        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? HRAdminId { get; set; }

        [ForeignKey("HRAdminId")]
        public virtual User HRAdmin { get; set; }

        public virtual ICollection<User> HRs { get; set; }

        public virtual ICollection<City> WorkCities { get; set; }
        public virtual ICollection<City> TaxCities { get; set; }
        public virtual ICollection<City> PensionCities { get; set; }
        public virtual ICollection<City> AccumulationCities { get; set; }

        public virtual ICollection<Assurance> Assurances { get; set; }
        public virtual ICollection<BudgetCenter> BudgetCenters { get; set; }
        public virtual ICollection<ContractType> ContractTypes { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Zhangtao> Zhangtaos { get; set; }

        public virtual ICollection<ClientPensionCityDocument> ClientPensionCityDocuments { get; set; }

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
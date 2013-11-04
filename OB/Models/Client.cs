using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class Client
    {
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
    }
}
using OB.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OB.Models
{
    public class City: SoftDelete
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Client> WorkCityClients { get; set; }
        public virtual ICollection<Client> TaxCityClients { get; set; }
        public virtual ICollection<Client> PensionCityClients { get; set; }
        public virtual ICollection<Client> AccumulationCityClients { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
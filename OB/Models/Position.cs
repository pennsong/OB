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
    public class Position : SoftDelete
    {
        public int Id { get; set; }
        [DisplayName("客户")]
        public int ClientId { get; set; }
        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual Client Client { get; set; }

        public override string ToString()
        {
            if (Client == null)
            {
                using (var db = new OBContext())
                {
                    Client = db.Client.Find(ClientId);
                }
            }
            return Client + "_" + Name;
        }
    }
}
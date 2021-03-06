﻿using FrameLog;
using OB.Models.Base;
using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OB.Models
{
    public class Document : SoftDelete, IHasLoggingReference
    {
        public Document()
        {
            ClientPensionCityDocuments = new List<ClientPensionCityDocument> { };
        }

        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = true)]
        [DisplayName("客户")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DisplayName("权重")]
        public int Weight { get; set; }

        [DisplayName("模版")]
        public string TemplatePath { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<ClientPensionCityDocument> ClientPensionCityDocuments { get; set; }

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
            return Client + "_" + Name;
        }
    }
}
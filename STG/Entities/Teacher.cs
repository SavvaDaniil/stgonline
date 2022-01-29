using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("teacher")]
    public class Teacher
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(516)")]
        public string name { get; set; }

        [Column("email",TypeName = "varchar(516)")]
        public string email { get; set; }

        [Column("is_curator", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isCurator { get; set; }

        [Column("price_curator", TypeName = "int(11)")]
        [System.ComponentModel.DefaultValue("0")]
        public int priceCurator { get; set; }



        [Column("price_tariff_1", TypeName = "int(11)")]
        [System.ComponentModel.DefaultValue("0")]
        public int priceTariff1 { get; set; }

        [Column("price_tariff_2", TypeName = "int(11)")]
        [System.ComponentModel.DefaultValue("0")]
        public int priceTariff2 { get; set; }

        [Column("price_tariff_3", TypeName = "int(11)")]
        [System.ComponentModel.DefaultValue("0")]
        public int priceTariff3 { get; set; }



        [Column("short_description", TypeName = "text")]
        public string shortDescription { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string instagram { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string youtube { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string facebook { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string vk { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int active { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }

        [Column("mentor_bio", TypeName = "text")]
        public string mentor_bio { get; set; }

        [Column("mentor_awards", TypeName = "text")]
        public string mentor_awards { get; set; }

    }
}

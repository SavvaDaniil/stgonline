using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("package_day")]
    public class PackageDay
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_package")]
        public Package package { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [DefaultValue("0")]
        public int orderInList { get; set; }

        [Column("date_of_add")]
        public DateTime? dateOfAdd { get; set; }
    }
}

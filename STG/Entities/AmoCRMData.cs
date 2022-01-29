using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("amocrmdata")]
    public class AmoCRMData
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("name", TypeName = "varchar(32)")]
        public string name { get; set; }

        [Column("strValue", TypeName = "Text")]
        public string strValue { get; set; }

        [Column("intValue", TypeName = "int(32)")]
        [DefaultValue("0")]
        public int intValue { get; set; }
    }
}

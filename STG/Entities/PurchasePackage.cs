using STG.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("purchase_package")]
    public class PurchasePackage : PurchaseAbstract
    {
        [Column("id_of_package")]
        public Package package { get; set; }
    }
}

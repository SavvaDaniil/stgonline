using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STG.Abstract;

namespace STG.Entities
{
    [Table("purchase_lesson")]
    public class PurchaseLesson : PurchaseAbstract
    {


        public Lesson lesson { get; set; }

        /*
        [Key, Column(Order = 0)]
        public int id { get; set; }

        public User user { get; set; }
        public Lesson lesson { get; set; }
        public Payment payment { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("1")]
        public int active { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isPayed { get; set; }

        public DateTime dateOfAdd { get; set; }
        public DateTime dateOfActivation { get; set; }
        */
    }
}

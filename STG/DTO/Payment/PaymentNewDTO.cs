using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Payment
{
    public class PaymentNewDTO
    {
        public int id_of_lesson { get; set; }

        public int single { get; set; }

        public int id_of_subscription { get; set; }

        public int is_prolongation { get; set; }

        public int id_of_package { get; set; }

        public int is_it_prolongation { get; set; }
        public STG.Entities.Payment payment_that_extended { get; set; }
    }
}

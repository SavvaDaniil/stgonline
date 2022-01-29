using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.AmoCRM
{
    public class AmoCRMNewContactResponse
    {
        public int id { get; set; }

        public AmoCRMNewContactResponse(int id)
        {
            this.id = id;
        }
    }
}

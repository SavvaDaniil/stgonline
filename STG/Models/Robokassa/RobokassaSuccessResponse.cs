using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Robokassa
{
    public class RobokassaSuccessResponse
    {
        public string OutSum { get; set; }
        public int InvId { get; set; }
        public string SignatureValue { get; set; }
    }
}

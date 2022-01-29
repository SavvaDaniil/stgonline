using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Payment
{
    public class TinkoffGetStateViewModel
    {
        public string TerminalKey { get; set; }
        public int PaymentId { get; set; }
        public string Token { get; set; }

        public TinkoffGetStateViewModel(string terminalKey, int paymentId, string token)
        {
            TerminalKey = terminalKey;
            PaymentId = paymentId;
            Token = token;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    public class TinkoffReceipt
    {
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Taxation")]
        public string Taxation { get; }
        [JsonProperty("Items")]
        public TinkoffItem[] Items { get; set; }

        public TinkoffReceipt(string email, string name, int quantity, int price)
        {
            Email = email;
            Taxation = "usn_income";
            Items = new TinkoffItem[] { new TinkoffItem(name, quantity, price) };
        }
    }
}

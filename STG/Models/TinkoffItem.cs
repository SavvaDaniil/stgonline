using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    public class TinkoffItem
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("Amount")]
        public int Amount { get; }
        [JsonProperty("Price")]
        public int Price { get; set; }
        [JsonProperty("Tax")]
        public string Tax { get; }

        public TinkoffItem(string name, int quantity, int price)
        {
            Name = name;
            Quantity = quantity;
            Amount = price * quantity;
            Price = price;
            Tax = "none";
        }
    }
}

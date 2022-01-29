using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    [Serializable]
    public class ExtendOfPurchaseSubscription
    {
        [JsonProperty("id_of_extend")]
        public int id_of_extend { get; set; }

        [JsonProperty("id_of_purchase_subscription")]
        public int id_of_purchase_subscription { get; set; }

    }
}

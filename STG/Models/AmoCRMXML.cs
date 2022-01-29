using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STG.Models
{
    [Serializable]
    public class AmoCRMXML
    {
        [XmlElement(ElementName = "status")]
        public bool status { get; set; }

        [XmlElement(ElementName = "token_type")]
        public string token_type { get; set; }

        [XmlElement(ElementName = "expires_in")]
        public int expires_in { get; set; }

        [XmlElement(ElementName = "access_token")]
        public string access_token { get; set; }

        [XmlElement(ElementName = "refresh_token")]
        public string refresh_token { get; set; }



        [XmlElement(ElementName = "date_of_set_str")]
        public int date_of_set_str { get; set; }

    }
}

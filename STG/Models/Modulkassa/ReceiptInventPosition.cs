using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Modulkassa
{
    public class ReceiptInventPosition
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public string barcode { get; set; }
        public int vatTag { get; set; }

        public ReceiptInventPosition()
        {
        }

        public ReceiptInventPosition(string name, decimal price, decimal quantity, string barcode)
        {
            this.name = name;
            this.price = price;
            this.quantity = quantity;
            this.barcode = barcode;
        }
    }
}

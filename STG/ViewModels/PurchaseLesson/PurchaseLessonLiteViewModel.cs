using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.PurchaseLesson
{
    public class PurchaseLessonLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int active { get; set; }

        public PurchaseLessonLiteViewModel(int id, string name, int price, int active)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.active = active;
        }
    }
}

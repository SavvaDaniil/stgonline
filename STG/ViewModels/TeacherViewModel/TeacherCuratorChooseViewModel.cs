using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherCuratorChooseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string shortDescription { get; set; }
        public string posterSrc { get; set; }
        public string posterRectSrc { get; set; }
        public int priceTariff1 { get; set; }
        public int priceTariff2 { get; set; }
        public int priceTariff3 { get; set; }

        public TeacherCuratorChooseViewModel(int id, string name, int price, string shortDescription, string posterSrc, int priceTariff1, int priceTariff2, int priceTariff3)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.shortDescription = shortDescription;
            this.posterSrc = posterSrc;
            this.priceTariff1 = priceTariff1;
            this.priceTariff2 = priceTariff2;
            this.priceTariff3 = priceTariff3;
        }

        public TeacherCuratorChooseViewModel(int id, string name, int price, string shortDescription, string posterSrc, string posterRectSrc, int priceTariff1, int priceTariff2, int priceTariff3)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.shortDescription = shortDescription;
            this.posterSrc = posterSrc;
            this.posterRectSrc = posterRectSrc;
            this.priceTariff1 = priceTariff1;
            this.priceTariff2 = priceTariff2;
            this.priceTariff3 = priceTariff3;
        }
    }
}

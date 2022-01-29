using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string instagram { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public int active { get; set; }
        public int is_curator { get; set; }
        public string mentorBio { get; set; }
        public string mentorAwards { get; set; }

        public int price_curator { get; set; }
        public int priceTariff1 { get; set; }
        public int priceTariff2 { get; set; }
        public int priceTariff3 { get; set; }


        public string posterSrc { get; set; }
        public string posterRectSrc { get; set; }

        public TeacherLiteViewModel(int id, string name, string email, string instagram, string shortDescription, string description, int active, int is_curator, string mentorBio, string mentorAwards, int price_curator, int priceTariff1, int priceTariff2, int priceTariff3, string posterSrc, string posterRectSrc)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.instagram = instagram;
            this.shortDescription = shortDescription;
            this.description = description;
            this.active = active;
            this.is_curator = is_curator;
            this.mentorBio = mentorBio;
            this.mentorAwards = mentorAwards;
            this.price_curator = price_curator;
            this.priceTariff1 = priceTariff1;
            this.priceTariff2 = priceTariff2;
            this.priceTariff3 = priceTariff3;
            this.posterSrc = posterSrc;
            this.posterRectSrc = posterRectSrc;
        }
    }
}

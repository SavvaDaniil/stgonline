using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageEditViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public string description { get; set; }

        public int order_in_list { get; set; }
        public string posterSrc { get; set; }

        public Level level { get; set; }
        public IEnumerable<Level> levelList { get; set; }

        public Style style { get; set; }
        public IEnumerable<Style> styleList { get; set; }

        public Teacher teacher { get; set; }
        public IEnumerable<Teacher> teacherList { get; set; }

        public Tariff tariff { get; set; }
        public IEnumerable<Tariff> tariffs { get; set; }

        public int statusOfChatNone0Homework1AndChat2 { get; set; }

        public PackageEditViewModel(int id, string name, int active, int price, int days, string description, int order_in_list, string posterSrc, Level level, Style style, Teacher teacher, Tariff tariff, int statusOfChatNone0Homework1AndChat2)
        {
            this.id = id;
            this.name = name;
            this.active = active;
            this.price = price;
            this.days = days;
            this.description = description;
            this.order_in_list = order_in_list;
            this.posterSrc = posterSrc;
            this.level = level;
            this.style = style;
            this.teacher = teacher;
            this.tariff = tariff;
            this.statusOfChatNone0Homework1AndChat2 = statusOfChatNone0Homework1AndChat2;
        }
    }
}

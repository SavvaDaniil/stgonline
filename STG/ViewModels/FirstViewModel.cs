using STG.Entities;
using STG.ViewModels.Subscription;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class FirstViewModel
    {

        public List<(int, string)> arrayContentSection2 { get; set; }
        public List<TeacherLiteIndexViewModel> teachers { get; set; }
        public List<TeacherCuratorChooseViewModel> curators { get; set; }
        public List<SubscriptionLiteViewModel> subscriptions { get; set; }
        public IEnumerable<Region> regionEnum { get; set; }

        public FirstViewModel(List<(int, string)> arrayContentSection2, List<TeacherLiteIndexViewModel> teachers, List<TeacherCuratorChooseViewModel> curators, List<SubscriptionLiteViewModel> subscriptions, IEnumerable<Region> regionEnum)
        {
            this.arrayContentSection2 = arrayContentSection2;
            this.teachers = teachers;
            this.curators = curators;
            this.subscriptions = subscriptions;
            this.regionEnum = regionEnum;
        }
    }
}

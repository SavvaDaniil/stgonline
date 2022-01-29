using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class EnumStylesViewModel
    {
        public IEnumerable<Style> styles { get; set; }

        public EnumStylesViewModel(IEnumerable<Style> styles)
        {
            this.styles = styles;
        }
    }
}

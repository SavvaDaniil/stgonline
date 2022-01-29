using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Interface
{
    interface IStyleService
    {
        List<Style> listAllActive();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class ConnectionUserToPrivatePackageNewDTO
    {
        [Required(ErrorMessage = "no id_of_user")]
        public int id_of_user { get; set; }

        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.ConnectionUserToPrivatePackage
{
    public class ConnectionUserToPrivatePackageEditDTO
    {
        [Required(ErrorMessage = "no id_of_user")]
        public int id_of_user { get; set; }

        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }

        [Required(ErrorMessage = "no id_of_active")]
        public int active { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STG.Entities
{
    [Table("admin")]
    public class Admin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("id", Order = 0)]
        public int Id { get; set; }

        [Column("username", TypeName = "varchar(256)")]
        public string Username { get; set; }

        [Column("password", TypeName = "varchar(256)")]
        public string Password { get; set; }

        [Column("auth_key", TypeName = "varchar(256)")]
        public string AuthKey { get; set; }

        [Column("access_token", TypeName = "varchar(256)")]
        public string AccessToken { get; set; }

        [Column("active", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int active { get; set; }

        [Column("level", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int level { get; set; }

        [Column("position", TypeName = "varchar(256)")]
        public string position { get; set; }

        public DateTime? dateOfAdd { get; set; }
        public DateTime? dateOfLastUpdateProfile { get; set; }


        [Column("panel_users", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_users { get; set; }

        [Column("panel_statements", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_statements { get; set; }

        [Column("panel_mentoring", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_mentoring { get; set; }

        [Column("panel_homeworks", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_homeworks { get; set; }

        [Column("panel_lessons", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_lessons { get; set; }

        [Column("panel_packages", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_packages { get; set; }

        [Column("panel_videos", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_videos { get; set; }

        [Column("panel_teachers", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_teachers { get; set; }

        [Column("panel_styles", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_styles { get; set; }

        [Column("panel_lessontypes", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_lessontypes { get; set; }

        [Column("panel_admins", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int panel_admins { get; set; }



    }
}

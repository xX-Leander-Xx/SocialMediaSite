using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_Benutzer", Schema = "dbo")]
    public class Benutzer
    {
        [Key]
        [ReadOnly(true)]
        public int id_Benutzer { get; set; }
        [Display(Name = "Benutzer")]
        public string Benutzername { get; set; }
        public string Passwort { get; set; }
    }
}

using System;
using System.Collections.Generic;
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

        [Required]
        [StringLength(20)]
        public string Benutzername { get; set; }
        [Required]
        [StringLength(20)]
        public string Passwort { get; set; }
        public string isAdmin { get; set; }
        public BenutzerKategorie BenutzerKategorie { get; set; }
        public ICollection<BenutzerBenutzer> BenutzerBenutzerFolgen { get; set; }
        public ICollection<BenutzerBenutzer> BenutzerBenutzerGefolgt { get; set; }
    }
}
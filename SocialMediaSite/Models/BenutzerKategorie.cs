using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_BenutzerKategorie", Schema = "dbo")]
    public class BenutzerKategorie
    {
        [Key]
        [ReadOnly(true)]
        public int id_BenutzerKategorie { get; set; }

        [ForeignKey("benutzer")]
        public int fk_id_Benutzer { get; set; }
        public virtual Benutzer benutzer { get; set; }

        [ForeignKey("kategorie")]
        public int fk_id_Kategorie { get; set; }
        public virtual Kategorie kategorie { get; set; }

    }
}

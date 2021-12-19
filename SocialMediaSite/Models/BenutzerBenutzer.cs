using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_BenutzerBenutzer", Schema = "dbo")]
    public class BenutzerBenutzer
    {
        [Key]
        [ReadOnly(true)]
        public int id_BenutzerBenutzer { get; set; }

        [ForeignKey("benutzer")]
        public int fk_id_BenutzerFolgen { get; set; }
        public virtual Benutzer benutzerFolgen { get; set; }

        [ForeignKey("benutzer")]
        public int fk_id_BenutzerGefolgt { get; set; }
        public virtual Benutzer benutzerGefolgt { get; set; }

    }
}

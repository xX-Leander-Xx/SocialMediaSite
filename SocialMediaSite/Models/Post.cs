using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_Posts", Schema = "dbo")]
    public class Post
    {
        [Key]
        [ReadOnly(true)]
        public int id_Post { get; set; }
        [Display(Name = "Titel")]
        public string Titel { get; set; }
        [Display(Name = "Inhalt")]
        public string Inhalt { get; set; }
        
        [Display(Name = "Datum")]
        public DateTime PostDate { get; set; }

        [Display(Name = "Kategorie")]
        [ForeignKey("kategorie")]
        public int fk_id_Kategorie { get; set; }
        public virtual Kategorie kategorie { get; set; }

        [ForeignKey("benutzer")]
        public int fk_id_Benutzer { get; set; }
        public virtual Benutzer benutzer { get; set; }

    }
} 
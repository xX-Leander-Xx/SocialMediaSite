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
        [Display(Name = "Kategorie")]
        public string fk_Kategorie { get; set; }
    }
}

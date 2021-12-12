using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_Kategorie", Schema = "dbo")]
    public class Kategorie
    {
        [Key]
        [ReadOnly(true)]
        public int id_Kategorie { get; set; }
        [Display(Name = "Kategorie")]
        public string KategorieName { get; set; }

    }
}

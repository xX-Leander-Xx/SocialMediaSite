using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaSite.Models
{
    [Table("tbl_Logs", Schema = "dbo")]
    public class Logs
    {
        [Key]
        [ReadOnly(true)]
        public int id_Log { get; set; }
        public string LogInfo { get; set; }
        public DateTime LogDate { get; set; }

    }
}


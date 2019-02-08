using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server
{
    [Table("filemanager")]
    public class File
    {
        [Key]
        [Column("file_id")]
        public int Id { get; set; }

        [Required]
        [Column("file_name")]
        public string FileName { get; set; }

        [Column("file")]
        public  byte[] TheFile { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("time_created")]
        public DateTime TimeCreated { get; set; }

        [Column("expiration")]
        public DateTime Expiration { get; set; }

        [Column("max_download")]
        public int MaxDownload { get; set; }

        [Column("total_download")]
        public int TotalDownload { get; set; }

        public override string ToString()
        {
          return $"Name:{FileName} Created:{TimeCreated} Expiration:{Expiration}, Maximum Download: {MaxDownload}, Download Left: {MaxDownload - TotalDownload}";
        }
    }
}

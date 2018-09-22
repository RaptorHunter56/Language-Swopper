using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("F")]
    public class OpenFilePath
    {
        [Key]
        [Display(Name = "F1")]
        [Column("F1")]
        public int OpenFilePathID { get; set; }
        [Display(Name = "F2")]
        [Column("F2")]
        public string PathEncoded { get; set; }
        [NotMapped]
        public string Path
        {
            get { return Cipher.Decrypt(PathEncoded, "Path"); }
            set { PathEncoded = Cipher.Encrypt(value, "Path"); }
        }

        [Display(Name = "F3")]
        [Column("F3")]
        public int? FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}

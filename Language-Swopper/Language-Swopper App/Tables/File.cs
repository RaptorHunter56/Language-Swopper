using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("B")]
    class File
    {
        [Key]
        [Display(Name = "B1")]
        [Column("B1")]
        public int FileID { get; set; }
        [Display(Name = "B2")]
        [Column("B2")]
        public string NameEncoded { get; set; }
        [NotMapped]
        public string Name
        {
            get { return Cipher.Decrypt(NameEncoded, "FileName"); }
            set { NameEncoded = Cipher.Encrypt(value, "FileName"); }
        }
    }
}

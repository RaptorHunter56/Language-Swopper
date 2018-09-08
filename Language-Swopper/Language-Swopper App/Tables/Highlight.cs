using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("E")]
    public class Highlight
    {
        [Key]
        [Display(Name = "E1")]
        [Column("E1")]
        public int HighlightID { get; set; }
        [Display(Name = "E2")]
        [Column("E2")]
        public string TextEncoded { get; set; }
        [NotMapped]
        public string Text
        {
            get { return Cipher.Decrypt(TextEncoded, "HighlightName"); }
            set { TextEncoded = Cipher.Encrypt(value, "HighlightName"); }
        }
        [Display(Name = "E3")]
        [Column("E3")]
        public string ColorEncoded { get; set; }
        [NotMapped]
        public string Color
        {
            get { return Cipher.Decrypt(ColorEncoded, "HighlightColor"); }
            set { ColorEncoded = Cipher.Encrypt(value, "HighlightColor"); }
        }

        [Display(Name = "E4")]
        [Column("E4")]
        public int? FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("C")]
    public class NameSpace
    {
        public NameSpace()
        {
            Class = new List<Class>();
        }

        [Key]
        [Display(Name = "C1")]
        [Column("C1")]
        public int NameSpaceID { get; set; }
        [Display(Name = "C2")]
        [Column("C2")]
        public string NameEncoded { get; set; }
        [NotMapped]
        public string Name
        {
            get { return Cipher.Decrypt(NameEncoded, "NameSpaceName"); }
            set { NameEncoded = Cipher.Encrypt(value, "NameSpaceName"); }
        }

        [Display(Name = "C3")]
        [Column("C3")]
        public int? FileId { get; set; }
        public File File { get; set; }

        public List<Class> Class { get; set; }
    }
}

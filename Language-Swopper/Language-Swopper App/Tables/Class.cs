using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("D")]
    public class Class
    {
        [Key]
        [Display(Name = "D1")]
        [Column("D1")]
        public int ClassID { get; set; }
        [Display(Name = "D2")]
        [Column("D2")]
        public string NameEncoded { get; set; }
        [NotMapped]
        public string Name
        {
            get { return Cipher.Decrypt(NameEncoded, "ClassName"); }
            set { NameEncoded = Cipher.Encrypt(value, "ClassName"); }
        }

        [Display(Name = "D3")]
        [Column("D3")]
        public int? NameSpaceId { get; set; }
        public NameSpace NameSpace { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App.Tables
{
    [Table("A")]
    class Folder
    {
        public Folder()
        {
            Files = new List<File>();
            Highlights = new List<Highlight>();
        }

        [Key]
        [Display(Name = "A1")]
        [Column("A1")]
        public int FolderID { get; set; }
        [Display(Name = "A2")]
        [Column("A2")]
        public string NameEncoded { get; set; }
        [NotMapped]
        public string Name
        {
            get { return Cipher.Decrypt(NameEncoded, "FolderName"); }
            set { NameEncoded = Cipher.Encrypt(value, "FolderName"); }
        }
        [Display(Name = "A3")]
        [Column("A3")]
        public string PathEncoded { get; set; }
        [NotMapped]
        public string Path
        {
            get { return Cipher.Decrypt(PathEncoded, "FolderPath"); }
            set { PathEncoded = Cipher.Encrypt(value, "FolderPath"); }
        }
        public void FullPath(string FolderPath)
        {
            Name = FolderPath.Split("\\".ToCharArray().First()).Last();
            Path = FolderPath.TrimEnd(Name.ToCharArray());
        }
        
        public List<File> Files { get; set; }
        public List<Highlight> Highlights { get; set; }
    }
}

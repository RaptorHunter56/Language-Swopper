using Language_Swopper_App.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App
{
    class FileContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<NameSpace> NameSpaces { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Highlight> Highlights { get; set; }
    }
}

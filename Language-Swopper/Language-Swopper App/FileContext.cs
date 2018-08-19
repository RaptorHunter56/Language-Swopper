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
        public void ClearAll()
        {
            this.Folders.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(A, RESEED, 0);");
            this.Files.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(B, RESEED, 0);");
            this.NameSpaces.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(C, RESEED, 0);");
            this.Classes.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(D, RESEED, 0);");
            this.Highlights.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(E, RESEED, 0);");
        }
    }

    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }

    public partial class MainWindow
    {
        public static void UpdateDatabase()
        {
            string pPath = @"C:\Users\Steven Bown\Source\Repos\Language-Swopper\Language-Swopper\Language-Swopper App\Swopper";
            List<string> directories = System.IO.Directory.GetDirectories(pPath).ToList();
            using (FileContext _context = new FileContext())
            {
                _context.ClearAll();
                foreach (var singledirectories in directories)
                {
                    Folder temp = new Folder();
                    temp.FullPath(singledirectories);
                    _context.Folders.Add(temp);
                    _context.SaveChanges();
                    List<string> filedirectories = System.IO.Directory.GetFiles(singledirectories).ToList();
                    foreach (var singlefiledirectories in filedirectories)
                    {
                        _context.Files.Add(new File()
                        {
                            Name = singlefiledirectories.Split("\\".ToCharArray().First()).Last().Split('.').First()
                        });
                        _context.SaveChanges();
                        string filetext = System.IO.File.ReadAllText(singlefiledirectories);
                        int occurrencenumber = filetext.Split("namespace".ToCharArray()).Count();
                        for (int i = 0; i < occurrencenumber; i++)
                        {

                        }
                    }

                }
            }
        }
    }
}

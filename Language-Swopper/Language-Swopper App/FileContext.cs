using Language_Swopper_App.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App
{
    public class FileContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<NameSpace> NameSpaces { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Highlight> Highlights { get; set; }
        public DbSet<OpenFilePath> OpenFilePaths { get; set; }
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
            this.OpenFilePaths.Clear();
            this.Database.ExecuteSqlCommand("DBCC CHECKIDENT(F, RESEED, 0);");
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
            string pPath = @"...\...\Swopper";
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
                    _context.Entry(temp).GetDatabaseValues();
                    List<string> filedirectories = System.IO.Directory.GetFiles(singledirectories).ToList();
                    foreach (var singlefiledirectories in filedirectories)
                    {
                        var tempfile = new File() { Name = singlefiledirectories.Split("\\".ToCharArray().First()).Last().Split('.').First() };
                        _context.Files.Add(tempfile);
                        _context.SaveChanges();

                        _context.Entry(tempfile).GetDatabaseValues();
                        var folder = _context.Folders.ToList().LastOrDefault();
                        tempfile.Folder = folder;
                        _context.SaveChanges();

                        string filetext = System.IO.File.ReadAllText(singlefiledirectories).Split(new[] { "########" }, StringSplitOptions.None)[0];
                        string[] InputFilePath = filetext.Split(new[] { "||||||||||" }, StringSplitOptions.None);
                        if (InputFilePath.Count() > 1)
                        {
                            var inputfilepath = new OpenFilePath() { Path = InputFilePath[1].Trim("\r\n//".ToCharArray())};
                            _context.OpenFilePaths.Add(inputfilepath);
                            _context.SaveChanges();

                            _context.Entry(inputfilepath).GetDatabaseValues();
                            inputfilepath.Folder = folder;
                            _context.SaveChanges();
                        }
                        int occurrencenumber = filetext.Split(new[] { "namespace" }, StringSplitOptions.None).Count();
                        for (int i = 1; i < occurrencenumber; i++)
                        {
                            var tempnamespaces = new NameSpace() { Name = filetext.Split(new[] { "namespace" }, StringSplitOptions.None)[i].Split(' ')[1].Replace("{", "").Trim() };
                            _context.NameSpaces.Add(tempnamespaces);
                            _context.SaveChanges();

                            _context.Entry(tempnamespaces).GetDatabaseValues();
                            var file = _context.Files.ToList().LastOrDefault();
                            tempnamespaces.File = file;
                            _context.SaveChanges();

                            int classoccurrencenumber = filetext.Split(new[] { "namespace" }, StringSplitOptions.None)[i].Split(new[] { "class" }, StringSplitOptions.None).Count();
                            for (int j = 1; j < classoccurrencenumber; j++)
                            {
                                var tempclass = new Class() { Name = filetext.Split(new[] { "namespace" }, StringSplitOptions.None)[i].Split(new[] { "class" }, StringSplitOptions.None)[j].Split(' ')[1].Replace("{", "").Trim() };
                                _context.Classes.Add(tempclass);
                                _context.SaveChanges();

                                _context.Entry(tempclass).GetDatabaseValues();
                                var tnamespace = _context.NameSpaces.ToList().LastOrDefault();
                                tempclass.NameSpace = tnamespace;
                                _context.SaveChanges();
                            }
                        }
                        try
                        {
                            var ColorList = System.IO.File.ReadAllText(singlefiledirectories).Split(new[] { "########" }, StringSplitOptions.None)[1].Trim().Split(new[] { "\r\n" }, StringSplitOptions.None);
                            foreach (var item in ColorList)
                            {
                                var temphighlight = new Highlight() { Text = item.Split(',')[0].TrimStart("//".ToCharArray()), Color = item.Split(',')[1], Type = (Highlight.Types)Enum.Parse(typeof(Highlight.Types), item.Split(',')[2], true) };
                                _context.Highlights.Add(temphighlight);
                                _context.SaveChanges();

                                _context.Entry(temphighlight).GetDatabaseValues();
                                temphighlight.Folder = folder;
                                _context.SaveChanges();
                            }
                        } catch (Exception) { }
                    }

                }
            }
        }
    }
}

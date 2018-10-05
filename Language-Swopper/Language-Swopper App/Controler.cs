using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Language_Swopper_App
{
    public class Controler : IDisposable
    {
        public Controler(/*FileContext fileContext*/)
        {
            resetList();
            addAssembly();
            resetResults();
        }
        List<string> dList = new List<string>();
        public Controler(string lin, string lout)
        {
            In = lin;
            Out = lout;
            addAssembly();
            resetResults();
            //context = fileContext;
        }
        //FileContext context;

        public void resetList()
        {
            dList.Clear();
            DirectoryInfo d = new DirectoryInfo($@"...\...\Swopper\{In}");
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }
            d = new DirectoryInfo($@"...\...\Swopper\{Out}");
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }
            d = new DirectoryInfo(@"...\...\Swopper\Base");
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }
            dList = dList.Distinct().ToList();
        }
        public void addAssembly()
        {
            compilerParams.ReferencedAssemblies.Add(typeof(System.Text.RegularExpressions.Regex).Assembly.Location);
        }
        public void resetResults()
        {
            results = provider.CompileAssemblyFromFile(compilerParams, dList.ToArray());
        }

        public string _In { get { return In; } set { In = value; resetList(); resetResults(); } }
        public string _Out { get { return Out; } set { Out = value; resetList(); resetResults(); } }
        private string In = "Python";
        private string Out = "CSharp";

        private static CompilerParameters compilerParams = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };
        private static Dictionary<string, string> providerOptions = new Dictionary<string, string> { {"CompilerVersion", "v3.5"} };
        private CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
        private CompilerResults results;
        
        public string Lines(string[] Line)
        {
            return LoadOut(LoadIn(Line));
        }

        private object LoadIn(string[] Line)
        {
            if (results.Errors.Count != 0)
            {
                try
                {
                    System.IO.File.WriteAllText(@"C:\Users\Steven Bown\Desktop\Mission Aport.txt", string.Empty);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Steven Bown\Desktop\Mission Aport.txt", true))
                    {
                        file.WriteLine($"No. {results.Errors.Count}");
                        file.WriteLine($"===");
                        foreach (CompilerError item in results.Errors)
                        {
                            file.WriteLine($"{item.FileName} {item.Column}/{item.Line}");
                            file.WriteLine($"{item.ErrorText}");
                            file.WriteLine($"===");
                        }
                    }
                }
                catch { }
                throw new Exception("Mission failed!");
            }
            try
            {
                System.IO.File.WriteAllText(@"C:\Users\Steven Bown\Desktop\Mission Aport.txt", "No. 0");
            }
            catch { }

            object o = results.CompiledAssembly.CreateInstance($"Lsw{In}.{In}Controler");
            MethodInfo mc = o.GetType().GetMethod("In");
            return mc.Invoke(o, new object[] { Line});
        }
        private string LoadOut(object Line)
        {
            if (results.Errors.Count != 0)
                throw new Exception("Mission failed!");

            object o = results.CompiledAssembly.CreateInstance($"Lsw{Out}.{Out}Controler");
            MethodInfo mc = o.GetType().GetMethod("Out");
            return (string)(mc.Invoke(o, new object[] { Line }));
        }

        public void Dispose()
        {
            
            provider.Dispose();
            //context.Dispose();
        }
    }
}

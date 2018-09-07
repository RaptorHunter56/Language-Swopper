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
    public class Controler
    {
        public Controler(FileContext fileContext)
        {

        }
        public string In = "Python";
        public string Out = "Python";
        public string line(string Line)
        {
            object InLine;
            string OutLine;
        }

        private void LoadIn()
        {
            DirectoryInfo d = new DirectoryInfo(@"...\...\Swopper\Python");
            List<string> dList = new List<string>();
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }
            d = new DirectoryInfo(@"...\...\Swopper\Base");
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }

            Dictionary<string, string> providerOptions = new Dictionary<string, string>
            {
                {"CompilerVersion", "v3.5"}
            };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            //CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source);
            CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, dList.ToArray());

            if (results.Errors.Count != 0)
                throw new Exception("Mission failed!");

            object o = results.CompiledAssembly.CreateInstance("LswString.Equals");
            MethodInfo mc = o.GetType().GetMethod("Read");
            //var returnValue = mc.Invoke(o, new object[] { "Name = 'some\"'" });
            //var returnValue = mc.Invoke(o, new object[] {
            //    new TextRange(
            //        MainTextControl.MainRichTextBox.Document.ContentStart,
            //        MainTextControl.MainRichTextBox.Document.ContentEnd).Text});
            //mc = o.GetType().GetMethod("Print");
            //returnValue = mc.Invoke(o, new object[] { returnValue });
            //MainTextControl.MainRichTextBox.Document.Blocks.Clear();
            //MainTextControl.MainRichTextBox.AppendText(returnValue.ToString());
        }
    }
}

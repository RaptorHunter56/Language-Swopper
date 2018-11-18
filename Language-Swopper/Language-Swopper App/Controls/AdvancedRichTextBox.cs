using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Language_Swopper_App.Tables;

namespace Language_Swopper_App.Controls
{
    public class RTX
    {
        #region Static
        public static string ReadRTX(ref RichTextBox rtb)
        {
            string rtfFromRtb = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                TextRange range2 = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range2.Save(ms, DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    rtfFromRtb = sr.ReadToEnd();
                }
            }
            return rtfFromRtb;
        }
        public static string Read(ref RichTextBox rtb)
        {
            TextRange range2 = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return range2.Text;
        }

        public static void WrightRTX(ref RichTextBox rtb, string rtf)
        {
//            rtf = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat{\fonttbl{\f0\fnil\fcharset0 Georgia;}}
//{\colortbl ;\red0\green0\blue0;\red155\green0\blue211;}
//{\*\generator Riched20 10.0.17134}\viewkind4\uc1 
//\pard\qj\cf1\f0\fs24\lang5129 44444444\par
//4444\par
//44\cf1  true\lang1033\par
//}";

            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            range.Load(stream, DataFormats.Rtf);
        }
        #endregion
        #region Not Static
        public RTX() { }
        public List<FormattedWord> words = new List<FormattedWord>();
        public string Premix = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat{\fonttbl{\f0\fnil\fcharset0 Georgia;}}
{\colortbl ;\red0\green0\blue0;";
        public string Postmix = @"\par
}
 ";
        public List<char> Parts = " (){}[]<>;\t".ToCharArray().ToList();

        public void update(ref RichTextBox rtb, bool back = false, bool del = false)
        {
            Parts.AddRange("\n".ToCharArray());

            int caretPos = rtb.Document.ContentStart.GetOffsetToPosition(rtb.CaretPosition);
            if (back) caretPos -= 2;
            else if (del) caretPos -= 2;
            else caretPos -= 3;

            //WrightRTX(ref rtb, "");
            string input =  Read(ref rtb);
            if (input.Trim() == "") return;
            input = input.Substring(0, input.Length - 2) + " ";

            string substring = "";
            int place = 0;
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            List<NumberdWord> pairs = new List<NumberdWord>();

            while (place < input.Length)
            {
                if (Parts.Contains(input[place]))
                {
                    if (ToPartList(words.Where(w => w.types == Highlight.Types.Normal).ToList()).Contains(substring.TrimEnd('\r')))
                    {
                        colors.Add(words.Where(x => x.key == substring.TrimEnd('\r')).SingleOrDefault().color);
                        pairs.Add(new NumberdWord() { Key = pairs.Count, Count = colors.Count, Word = substring.TrimEnd('\r') });
                        if (place != input.Length - 1)
                        {
                            if (input[place] == '\n')
                                pairs.Add(new NumberdWord() { Key = pairs.Count + 1, Count = 0, Word = "\r\n".ToString() });
                            else
                                pairs.Add(new NumberdWord() { Key = pairs.Count + 1, Count = 0, Word = input[place].ToString() });
                        }
                        substring = "";
                    }
                    else
                    {
                        if (place != input.Length - 1)
                        {
                            pairs.Add(new NumberdWord() { Key = pairs.Count, Count = 0, Word = substring + input[place] });
                        }
                        else
                        {
                            if (input[place] == '\n')
                                pairs.Add(new NumberdWord() { Key = pairs.Count, Count = 0, Word = "\r\n".ToString() });
                            else
                                pairs.Add(new NumberdWord() { Key = pairs.Count, Count = 0, Word = substring });
                        }
                        substring = "";
                    }
                }
                else
                    substring += input[place];
                place++;
            }


            string output = Premix;
            foreach (var item in colors)
            {
                output += $@"\red{item.R.ToString()}\green{item.G.ToString()}\blue{item.B.ToString()};";
            }
            output += @"}
{\*\generator Riched20 10.0.17134}\viewkind4\uc1
\pard\qj";
            bool start = true;
            foreach (var item in pairs)
            {
                output += $@"\cf{item.Count + 1}";
                if (start){ output += @"\f0\fs24\lang5129"; start = false; }

                string temp = item.Word.Replace(@"\",@"\\");
                temp = temp.Replace(@"{", @"\{");
                temp = temp.Replace(@"}",@"\}");
                temp = temp.Replace("\r\n", @"\par
");
                output += $@" {temp}";
            }
            output += Postmix;
            WrightRTX(ref rtb, output);

            //caretPos = caretPos.GetPositionAtOffset(1, LogicalDirection.Forward);
            rtb.CaretPosition = rtb.Document.ContentStart;
            try
            {
                rtb.CaretPosition = rtb.CaretPosition.GetPositionAtOffset(caretPos, LogicalDirection.Forward);
            }
            catch (Exception)
            {
                rtb.CaretPosition = rtb.Document.ContentEnd;
            }


            int rt = 300;
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string[] vs = textRange.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in vs)
            {
                FormattedText ft = new FormattedText(item,
                                               System.Globalization.CultureInfo.CurrentCulture,
                                               FlowDirection.LeftToRight,
                                               new Typeface(rtb.FontFamily, rtb.FontStyle, rtb.FontWeight, rtb.FontStretch),
                                               rtb.FontSize + 3,
                                               System.Windows.Media.Brushes.Black);
                if (ft.Width > rt)
                    rt = Convert.ToInt32(ft.Width.ToString().Split('.')[0]);
            }
            rtb.Document.PageWidth = rt + (rt / 29);
        }
        public List<string> ToPartList(List<FormattedWord> words)
        {
            List<string> vs = new List<string>();
            foreach (var item in words)
            {
                vs.Add(item.key);
            }
            return vs;
        }
        #endregion
    }

    public struct FormattedWord
    {
        public string key;
        public System.Windows.Media.Color color;
        public Highlight.Types types;

        public FormattedWord(string key, System.Windows.Media.Color color, Highlight.Types types) : this()
        {
            this.key = key;
            this.color = color;
            this.types = types;
        }
    }
    public struct NumberdWord
    {
        public string Word { get; set; }
        public int Count { get; set; }
        public int Key { get; set; }
    }
}

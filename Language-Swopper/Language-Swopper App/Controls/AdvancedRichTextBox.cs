﻿using System;
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
        public List<char> Parts = " (){}[]<>,;\t".ToCharArray().ToList();

        public struct Values
        {
            public int caretPositionFormated;
            public int caretPosition;
            public bool back;
            public bool del;
            public string input;
            public string oldinput;
            public List<System.Windows.Media.Color> colors;
            public List<NumberdWord> pairs;
        }

        private int newCharetPosition(ref RichTextBox rtb, Values values)
        {
            int newposition = values.caretPositionFormated;
            //if (values.back) newposition -= 2;
            //else if (values.del) newposition -= 2;
            //else newposition -= 3;
            //if (values.caretPositionFormated == values.caretPosition)
            //{
            //    if (values.oldinput == "") newposition += 1;
            //    if (values.oldinput == "\r\n") newposition += 1;
            //    if (values.oldinput.Length > 2)
            //    {
            //        if (values.oldinput.Substring(0, values.oldinput.Length - 2).EndsWith("\r\n")) newposition += 1;
            //        if ((values.oldinput.Substring(0, values.oldinput.Length - 2) + " ").Split("\r\n".ToCharArray()).Length - 1 < values.input.Split("\r\n".ToCharArray()).Length - 1) newposition -= 4;
            //    }
            //}

            TextPointer start = rtb.Document.ContentStart;
            TextPointer caret = rtb.CaretPosition;
            TextRange range = new TextRange(start, caret);
            int indexInText = range.Text.Length;

            int blueadd = 0;
            int blueaddindex = 0;
            foreach (var item in values.pairs)
            {
                if (item.Count > 0) blueadd += 4;
                int itemcpart = 0;
                foreach (var itemc in item.Word)
                {
                    blueaddindex++;
                    itemcpart++;
                    try
                    {
                        if (values.pairs[values.pairs.IndexOf(item) + 1].Word == "\r\n" &&
                            item.Word.Length == itemcpart)
                        { if (indexInText == blueaddindex) { blueadd -= 2; break; } }
                        else
                        { if (indexInText == blueaddindex) break; }
                    }
                    catch
                    { if (indexInText == blueaddindex) break; }
                }
                if (indexInText == blueaddindex) break;
                try
                {
                    if (values.pairs[values.pairs.IndexOf(item) + 1].Word != "\r\n")
                    { if (item.Count > 0) blueadd += 4; }
                    else
                    { if (item.Count > 0) blueadd += 0; }
                }
                catch
                { if (item.Count > 0) blueadd += 0; }
            }

            int point = 0;
            int newpoint = 0;
            int Lengthpoint = 1;
            bool startpoint = true;
            var b = rtb.Document.Blocks;
            foreach (var item in b)
            {
                var brange = new TextRange(item.ContentStart, item.ElementEnd);

                if (point <= indexInText && indexInText < (point + 2 + brange.Text.Length))
                {
                    if (!startpoint) { if (brange.Text != "") newpoint += 3 + Lengthpoint; }
                    else { if (brange.Text == "") newpoint += 3; }
                    int position = 0;
                    foreach (var item2 in brange.Text)
                    {
                        position++;
                        if (indexInText == point + position) { newpoint += position; break; }
                    }
                    break;
                }
                else
                {
                    Lengthpoint -= 1;
                    if (startpoint)
                    {
                        if (brange.Text != "") newpoint += 3;
                        startpoint = false;
                    }
                    else
                    {
                        if (brange.Text == "") newpoint += 3;
                        else newpoint += 7;
                    }
                    newpoint += brange.Text.Length;
                }
                point += (2 + brange.Text.Length);
            }
            #region Old
            //if (oldText.Length > 0)
            //{
            //    if ((oldText.Substring(0, oldText.Length - 2) + " ").Length == input.Length + 1 ||
            //        (oldText.Substring(0, oldText.Length - 2) + " ").Length == input.Length - 1)
            //    {
            //        if (oldText != "" && oldText != "\r\n")
            //        {
            //            oldText = oldText.Substring(0, oldText.Length - 2) + " ";
            //            whilePlace(oldText, ref oldcolors, ref oldpairs);
            //        }
            //        else
            //        {
            //            caretPos += 1;
            //        }
            //        int position = 0;
            //        int removle = 0;
            //        int Totalremovle = 0;
            //        startcaretPos = caretPos;
            //        foreach (var item in pairs)
            //        {
            //            if (item.Count > 0 && startcaretPos >= position + item.Word.Length)
            //            {
            //                caretPos += 6;
            //                //caretPos -= removle;
            //                Totalremovle += 1;
            //                removle += 1;
            //            }
            //            else if (item.Count > 0)
            //            {
            //                Totalremovle += 1;
            //            }
            //            position += item.Word.Length;
            //        }
            //        foreach (var item in pairs)
            //        {
            //            if (item.Count > 0)
            //            {
            //                caretPos += 1;
            //                if (Totalremovle == removle)
            //                {
            //                    caretPos -= 1;
            //                }
            //                break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (back) caretPos += 1;
            //        if ((oldText.Substring(0, oldText.Length - 2) + " ").Length < input.Length + 1)
            //        {
            //            caretPos -= 2;
            //        }
            //        if (oldText != "" && oldText != "\r\n")
            //        {
            //            oldText = oldText.Substring(0, oldText.Length - 2) + " ";
            //            whilePlace(oldText, ref oldcolors, ref oldpairs);
            //            caretPos -= 1;
            //        }
            //        else
            //        {
            //            caretPos += 1;
            //        }
            //        int c = (oldText.Substring(0, oldText.Length - 2) + " ").Length;
            //        if ((c - input.Length) <= -3)
            //        {
            //            caretPos += 3;
            //        }
            //    }
            //}
            //else
            //{
            //    caretPos += 1;
            //}
            #endregion
            return blueadd + newpoint;
        }

        public void update(ref RichTextBox rtb, bool back = false, bool del = false, string oldText = "")
        {
            Parts.AddRange("\n".ToCharArray());

            int caretPos = rtb.Document.ContentStart.GetOffsetToPosition(rtb.CaretPosition);

            TextRange documentRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            //documentRange.ClearAllProperties();

            int startcaretPosFormated = caretPos;
            caretPos = rtb.Document.ContentStart.GetOffsetToPosition(rtb.CaretPosition);

            //if (back) caretPos -= 2;
            //else if (del) caretPos -= 2;
            //else caretPos -= 3;
            //if (caretPos < 0) caretPos = 1;

            //WrightRTX(ref rtb, "");
            string input =  Read(ref rtb);
            if (input.Trim() == "") return;
            input = input.Substring(0, input.Length - 2) + " ";

            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            List<NumberdWord> pairs = new List<NumberdWord>();

            List<System.Windows.Media.Color> oldcolors = new List<System.Windows.Media.Color>();
            List<NumberdWord> oldpairs = new List<NumberdWord>();

            ///
            whilePlace(input, ref colors, ref pairs);
            caretPos = newCharetPosition(ref rtb, new Values()
            {
                caretPositionFormated = startcaretPosFormated,
                caretPosition = caretPos,
                back = back,
                del = del,
                input = input,
                oldinput = oldText,
                colors = colors,
                pairs = pairs
            });
            ///

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
        public void whilePlace(string input, ref List<System.Windows.Media.Color> colors, ref List<NumberdWord> pairs)
        {
            string substring = "";
            int place = 0;
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
                    else if (Conected(substring))
                    {
                        colors.Add(words.Where(x => x.key == substring[0].ToString()).SingleOrDefault().color);
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
                    else if (StartToEnd(substring.TrimEnd('\r')))
                    {
                        colors.Add(words.Where(x => x.key == substring[0].ToString()).SingleOrDefault().color);
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

        }

        private bool StartToEnd(string substring)
        {
            if (substring.Length > 1)
            {
                if (substring[0] == substring.ToCharArray().Last())
                {
                    if (ToPartList(words.Where(w => w.types == Highlight.Types.StartToEnd).ToList()).Contains(substring[0].ToString()))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool Conected(string substring)
        {
            bool Return = false;
            bool First = true;
            foreach (var item in substring.TrimEnd('\r'))
            {
                if (!ToPartList(words.Where(w => w.types == Highlight.Types.Connected).ToList()).Contains(item.ToString()))
                    Return = false;
                else if (First)
                {
                    First = !First;
                    Return = true;
                }
            }
            return Return;
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

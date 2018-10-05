using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Language_Swopper_App
{

    /// <summary>
    /// Interaction logic for TextControl.xaml
    /// </summary>
    public partial class TextControl : UserControl
    {
        public string LsLanguage
        {
            get { return (string)GetValue(LsLanguageProperty); }
            set { SetValue(LsLanguageProperty, value); }
        }
        public static readonly DependencyProperty LsLanguageProperty = DependencyProperty.Register("LsLanguage", typeof(string), typeof(TextControl), new UIPropertyMetadata(""));
        

        private Dictionary<string, MainWindow.ColorType> dictionary;
        public Dictionary<string, MainWindow.ColorType> Dictionary
        {
            get
            {
                return dictionary;
            }
            set
            {
                ColorTags.ResetTags(new List<string>(value.Keys));
                dictionary = value;
                TextChangedMethod(100);
            }
        }

        public TextControl()
        {
            InitializeComponent();
            Dictionary = new Dictionary<string, MainWindow.ColorType>();
        }

        void RichTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var rtb = e.Source as RichTextBox;
            if (rtb == null)
                return;

            if (!rtb.Selection.Start.Equals(rtb.Selection.End))
            {
                e.Handled = true;
            }
        }

        #region textbox
        private int LineMoveCount(int x)
        {
            ///y=5\left(\frac{1}{1+e^{\left(\frac{1}{10}*-x\right)+4}}\right)
            //return (int)(5 * (1 / (1 + Math.Pow(Math.E, (((1 / 5) * -x) + 4)))));
            double c = 0.2;
            c = c * -x;
            c += 4;
            c = Math.Pow(Math.E, c);
            c++;
            c = 1 / c;
            c = 5 * c;
            return (int)Math.Round(c);
        }
        public int GetLineNumber(bool TillEnd = false)
        {
            TextPointer caretLineStart = MainRichTextBox.CaretPosition.GetLineStartPosition(0);
            TextPointer p = MainRichTextBox.Document.ContentStart.GetLineStartPosition(0);
            int caretLineNumber = 1;

            while (true)
            {
                if (!TillEnd)
                {
                    if (caretLineStart.CompareTo(p) < 0)
                    {
                        break;
                    }
                }

                int result;
                p = p.GetLineStartPosition(1, out result);

                if (result == 0)
                {
                    break;
                }

                caretLineNumber++;
            }
            return caretLineNumber;
        }
        private bool OneChange = true;
        private void TextChangedMethod(int AddedLength)
        {
            int LineNumber = GetLineNumber() - 1;
            int LineEndNumber = GetLineNumber(true);
            int LineMove = LineMoveCount(LineEndNumber);
            int LineCount = 0;
            OneChange = false;
            if (MainRichTextBox.Document == null)
                return;
            TextRange documentRange;
            TextPointer navigator;
            if (LineEndNumber == 1 || AddedLength != 1)
            {
                documentRange = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
                documentRange.ClearAllProperties();
                navigator = MainRichTextBox.Document.ContentStart;
            }
            else
            {
                TextPointer t1 = MainRichTextBox.Document.ContentStart.GetLineStartPosition(0).GetLineStartPosition(LineNumber - LineMove - 1);
                TextPointer t2;
                try
                {
                    t2 = MainRichTextBox.Document.ContentStart.GetLineStartPosition(0).GetLineStartPosition(LineNumber + LineMove);
                    t2 = t2.GetInsertionPosition(LogicalDirection.Backward);
                }
                catch { t2  = MainRichTextBox.Document.ContentEnd; }
                documentRange = new TextRange(t1, t2);
                string ttext = documentRange.Text;
                documentRange.ClearAllProperties();
                navigator = MainRichTextBox.CaretPosition.DocumentStart;
            }

            while (navigator.CompareTo(MainRichTextBox.Document.ContentEnd) < 0)
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                {
                    if (LineEndNumber == 1)
                        CheckWordsInRun((Run)navigator.Parent);

                    LineCount++;
                    if (!(LineCount < LineNumber - LineMove - 1) &&!(LineCount > LineNumber + LineMove + 1))
                    {
                        CheckWordsInRun((Run)navigator.Parent);
                    }
                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            Format();
            OneChange = true;
        }
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            try { if (OneChange) TextChangedMethod(e.Changes.FirstOrDefault().AddedLength); } catch { }
        }
        new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

            Color? color;
            public Color Color { get { return color ?? new Color() { A = 255, R = 0, G = 0, B = 205 }; } set { color = value; } }

            Tables.Highlight.Types? types;
            public Tables.Highlight.Types Types { get { return types ?? Tables.Highlight.Types.Normal; } set { types = value; } }

        }
        List<Tag> m_tags = new List<Tag>();
        void Format()
        {
            MainRichTextBox.TextChanged -= this.TextChangedEventHandler;

            for (int i = 0; i < m_tags.Count; i++)
            {
                TextRange range = new TextRange(m_tags[i].StartPosition, m_tags[i].EndPosition);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(m_tags[i].Color));
                range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            m_tags.Clear();

            MainRichTextBox.TextChanged += this.TextChangedEventHandler;
        }

        void CheckWordsInRun(Run run)
        {
            string text = run.Text + " ";

            int sIndex = 0;
            int eIndex = 0;
            char tt;
            char ttt;
            for (int i = 0; i < text.Length; i++)
            {
                try { tt = text[i - 1]; } catch { }
                try { ttt = text[i]; } catch { }
                if (Char.IsWhiteSpace(text[i]) | ColorTags.GetSpecials.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1])))
                    {
                        eIndex = i - 1;
                        string word = text.Substring(sIndex, eIndex - sIndex + 1);
                        try
                        {
                            if (dictionary[word[0].ToString()].Types == Tables.Highlight.Types.Connected)
                            {
                                List<Tag> templ = new List<Tag>();
                                foreach (char singlechar in word)
                                {
                                    if (ColorTags.IsKnownTag(singlechar.ToString()))
                                    {
                                        Tag t = new Tag();
                                        t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                        t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                        t.Word = singlechar.ToString();
                                        try
                                        {
                                            t.Color = dictionary[singlechar.ToString()].Color;
                                            t.Types = dictionary[singlechar.ToString()].Types;
                                        }
                                        catch { }
                                        m_tags.Add(t);
                                        templ.Add(t);
                                        sIndex = i + 1;
                                    }
                                    else
                                    {
                                        foreach (var item in templ)
                                        {
                                            m_tags.Remove(item);
                                        }
                                        break;
                                    }
                                }
                            }
                            else if (dictionary[word[0].ToString()].Types == Tables.Highlight.Types.StartToEnd)
                            {
                                foreach (char singlechar in word)
                                {
                                    Tag t = new Tag();
                                    t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                    t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                    t.Word = singlechar.ToString();
                                    try
                                    {
                                        t.Color = dictionary[singlechar.ToString()].Color;
                                        t.Types = dictionary[singlechar.ToString()].Types;
                                    }
                                    catch { }
                                    m_tags.Add(t);
                                }
                                sIndex = i + word.Length;
                            }
                            else
                            {
                                if (ColorTags.IsKnownTag(word))
                                {
                                    Tag t = new Tag();
                                    t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                    t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                    t.Word = word;
                                    try
                                    {
                                        t.Color = dictionary[word].Color;
                                        t.Types = dictionary[word].Types;
                                    }
                                    catch { }
                                    m_tags.Add(t);
                                }
                                sIndex = i + 1;
                            }
                        }
                        catch (Exception)
                        {
                            if (ColorTags.IsKnownTag(word))
                            {
                                Tag t = new Tag();
                                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                t.Word = word;
                                try
                                {
                                    t.Color = dictionary[word].Color;
                                    t.Types = dictionary[word].Types;
                                }
                                catch { }
                                m_tags.Add(t);
                            }
                            sIndex += word.Length + 1;
                        }
                    }
                }
            }
            if (text.Length - sIndex <= 0)
                return;
            string lastWord = text.Substring(sIndex, text.Length - sIndex);
            if (ColorTags.IsKnownTag(lastWord))
            {
                Tag t = new Tag();
                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                t.Word = lastWord;
                m_tags.Add(t);
            }
        }
        #endregion
        
        private void MainRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                string ss = new TextRange(MainRichTextBox.CaretPosition.GetLineStartPosition(0), MainRichTextBox.CaretPosition).Text;
                if (ss[ss.Length - 1] == '\t')
                {
                    MainRichTextBox.CaretPosition = MainRichTextBox.CaretPosition.GetPositionAtOffset(-1, LogicalDirection.Backward);
                    MainRichTextBox.CaretPosition.DeleteTextInRun(1);
                    MainRichTextBox.CaretPosition = MainRichTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
                }
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    MainRichTextBox.Focus();
                }));
                e.Handled = true;
                return;
            }
            else if (e.Key == Key.Tab)
            {
                string textRange = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd).Text;
                MainRichTextBox.CaretPosition = MainRichTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
                MainRichTextBox.CaretPosition.InsertTextInRun("\t");
                textRange = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd).Text;
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    MainRichTextBox.Focus();
                }));
                e.Handled = true;
                return;
            }
        }
    }

    class ColorTags
    {
        static List<string> tags = new List<string>();
        static List<char> specials = new List<char>();
        #region ColorTags
        static ColorTags()
        {
            string[] strs = {
                "test"
            };
            tags = new List<string>(strs);

            char[] chrs = { '.', ')', '(', '[', ']', '>', '<', ':', ';', '\n', '\t', '='};
            specials = new List<char>(chrs);
        }
        #endregion
        public static List<char> GetSpecials
        {
            get { return specials; }
        }
        public static List<string> GetTags
        {
            get { return tags; }
        }
        public static bool IsKnownTag(string tag)
        {
            return tags.Exists(delegate (string s) { return s.ToLower().Equals(tag.ToLower()); });
        }
        public static List<string> GetJSProvider(string tag)
        {
            return tags.FindAll(delegate (string s) { return s.ToLower().StartsWith(tag.ToLower()); });
        }
        public static void UpDateTags(List<string> listoftags)
        {
            tags.AddRange(listoftags);
        }
        public static void ResetTags(List<string> listoftags)
        {
            tags.Clear();
            UpDateTags(listoftags);
        }
    }
}

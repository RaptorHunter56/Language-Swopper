using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Language_Swopper_App
{
    /// <summary>
    /// Interaction logic for TextControl.xaml
    /// </summary>
    public partial class TextControl : UserControl
    {
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
                TextChangedMethod();
            }
        }

        public TextControl()
        {
            InitializeComponent();
            dictionary = new Dictionary<string, MainWindow.ColorType>();
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
        private void TextChangedMethod()
        {
            if (MainRichTextBox.Document == null)
                return;

            TextRange documentRange = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
            documentRange.ClearAllProperties();

            TextPointer navigator = MainRichTextBox.Document.ContentStart;
            while (navigator.CompareTo(MainRichTextBox.Document.ContentEnd) < 0)
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                {
                    CheckWordsInRun((Run)navigator.Parent);
                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            Format();
        }
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            TextChangedMethod();
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

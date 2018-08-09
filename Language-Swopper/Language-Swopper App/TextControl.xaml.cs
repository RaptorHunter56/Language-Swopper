﻿using System;
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
        private Dictionary<string, Color> dictionary;
        public Dictionary<string, Color> Dictionary
        {
            get
            {
                return dictionary;
            }
            set
            {
                ColorTags.ResetTags(new List<string>(value.Keys));
                dictionary = value;
            }
        }

        public TextControl()
        {
            InitializeComponent();
            dictionary = new Dictionary<string, Color>();
        }

        #region textbox
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
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
        new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

            Color? color;
            public Color Color { get { return color ?? new Color() { A = 255, R = 255, G = 0, B = 0 }; } set { color = value; } }

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
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text[i]) | ColorTags.GetSpecials.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1]) | ColorTags.GetSpecials.Contains(text[i - 1])))
                    {
                        eIndex = i - 1;
                        string word = text.Substring(sIndex, eIndex - sIndex + 1);

                        if (ColorTags.IsKnownTag(word))
                        {
                            Tag t = new Tag();
                            t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                            t.Word = word;
                            try
                            {
                                t.Color = dictionary[word];
                            }
                            catch { }
                            m_tags.Add(t);
                        }
                    }
                    sIndex = i + 1;
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

            char[] chrs = { '.', ')', '(', '[', ']', '>', '<', ':', ';', '\n', '\t' };
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
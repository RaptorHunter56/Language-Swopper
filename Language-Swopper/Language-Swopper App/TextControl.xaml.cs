using System;
using System.Collections.Generic;
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

        public Dictionary<string, Color> dictionary = new Dictionary<string, Color>();

        public TextControl()
        {
            InitializeComponent();
            this.paragraph = new Paragraph();
            MainRichTextBox.Document = new FlowDocument(paragraph);
            dictionary.Add("hi", new Color() { A = 250, R = 0, G = 0, B = 250 });
        }

        private List<Word> words = new List<Word>();
        private Paragraph paragraph;
        private bool stop = true;

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (stop)
            {
                stop = false;
                words.Clear();

                int moveTo = -2 - MainRichTextBox.CaretPosition.GetOffsetToPosition(MainRichTextBox.CaretPosition.DocumentStart);
                string largeText = "";

                foreach (var TextColor in paragraph.Inlines)
                {
                    foreach (var TextWord in new TextRange(TextColor.ContentStart, TextColor.ContentEnd).Text)
                    {
                        largeText += TextWord;
                    }
                }
                foreach (var item in largeText.Split(' '))
                {
                    words.Add(new Word() { Text = item + " ", Color = new Color() { A = 250, R = 0, G = 0, B = 0 } });
                }
                words[words.Count - 1].Text = words[words.Count - 1].Text.Substring(0, (words[words.Count - 1].Text.Length - 1));
                for (int i = 0; i < words.Count; i++)
                {
                    CheckWord(i);
                }

                paragraph.Inlines.Clear();
                foreach (var word in words)
                {
                    paragraph.Inlines.Add(new Run(word.Text)
                    {
                        Foreground = new SolidColorBrush(word.Color)
                    });
                }

                //if (moveTo != 0)
                //    MainRichTextBox.CaretPosition = ;
                //else
                //    MainRichTextBox.CaretPosition = MainRichTextBox.CaretPosition.DocumentEnd;
                ////var from = "user1";
                ////var text = "chat message goes here";
                ////paragraph.Inlines.Add(new Run(from + ": ")
                ////{
                ////    Foreground = Brushes.Red
                ////});
                ////paragraph.Inlines.Add(text);
                ////paragraph.Inlines.Add(new LineBreak());
                this.DataContext = this;
                stop = true;
            }
        }

        internal void CheckWord(int Position)
        {
            if (dictionary.ContainsKey(words[Position].Text))
                words[Position].Color = dictionary[words[Position].Text];
            else
                words[Position].Color = new Color() { A = 250, R = 0, G = 0, B = 0 };
        }

        private class Word
        {
            public string Text { get; set; }
            public Color Color { get; set; } = new Color() { A = 250, R = 0, G = 0, B = 0};
        }
    }
}

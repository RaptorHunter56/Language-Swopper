using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Language_Swopper_App
{
    static class Copy
    {
        public static void CopyProperties(object objSource, object objDestination)
        {
            #region Testing
            try {
                string SourceText = new TextRange(((RichTextBox)objSource).Document.ContentStart, ((RichTextBox)objSource).Document.ContentEnd).Text;
                string DestinationText = new TextRange(((RichTextBox)objDestination).Document.ContentStart, ((RichTextBox)objDestination).Document.ContentEnd).Text;
            } catch (Exception) { }
            #endregion
            
            ///get the list of all properties in the destination object
            ///var destProps = objDestination.GetType().GetProperties();
            ///get the list of all properties in the source object
            ///foreach (var sourceProp in objSource.GetType().GetProperties())
            ///{
            ///    foreach (var destProperty in destProps)
            ///    {
            ///        //if we find match between source & destination properties name, set
            ///        //the value to the destination property
            ///        if (destProperty.Name == sourceProp.Name &&
            ///                destProperty.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
            ///        {
            ///            destProperty.SetValue(destProps, sourceProp.GetValue(
            ///                sourceProp, new object[] { }), new object[] { });
            ///            break;
            ///        }
            ///    }
            ///}

            FlowDocument doc = ((RichTextBox)objSource).Document;
            ((RichTextBox)objSource).Document = new FlowDocument();
            ((RichTextBox)objDestination).Document = doc;


            #region Testing
            try {
                string SourceText = new TextRange(((RichTextBox)objSource).Document.ContentStart, ((RichTextBox)objSource).Document.ContentEnd).Text;
                string DestinationText = new TextRange(((RichTextBox)objDestination).Document.ContentStart, ((RichTextBox)objDestination).Document.ContentEnd).Text;
            } catch (Exception) { }
            #endregion
        }

    }
}

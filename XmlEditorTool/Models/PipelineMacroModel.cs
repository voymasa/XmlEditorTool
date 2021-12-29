using System.Collections.Generic;
using System.Collections.ObjectModel;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool.Models
{
    public class PipelineMacroModel
    {
        public string AttributeName { get; set; }
        public string DataType { get; set; }
        public List<ContentItemModel> ContentList { get; set; }

        public Dictionary<string,string> GetContentAttributePairs()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            /*
             * For each ContentItem in the list, get the content and pair it with the appropriate attribute name.
             * Attribute names for the pairing are basically the AttributeName variable for most single content/value
             * but some pipeline macros, such as Colour, have a unique Attribute name for each colour (Colour.R, Colour.G, etc)
             */

            return keyValuePairs;
        }

        public ObservableCollection<ContentItemViewModel> ContentCollection { get; set; }
        public Dictionary<string, string> GenerateContentAttributeValues()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();

            if (ContentCollection.Count > 1)
            {
                foreach (ContentItemViewModel vm in ContentCollection)
                {
                    KeyValuePair<string, string> kvp = vm.GetContentHeaderValuePair(AttributeName + ".", false);
                    if (kvp.Value != null)
                    {
                        content.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            else if (ContentCollection.Count == 1)
            {
                foreach (ContentItemViewModel vm in ContentCollection)
                {
                    KeyValuePair<string, string> kvp = vm.GetContentHeaderValuePair(AttributeName, true);
                    if (kvp.Value != null)
                    {
                        content.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return content;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Models
{
    class PipelineBasicStringModel
    {
        public string AttributeName { get; set; }
        public string ContentHeader { get; set; }
        public string ContentValue { get; set; }
        public string ViewModelDataType { get; set; }
        public string ContentDefault { get; set; }
        public bool IsDefaultSet { get; set; }
        public bool ContentValid { get; set; }
        public string ContentErrorMsg { get; set; }

        public Dictionary<string, string> BuildAttributeContentDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            KeyValuePair<string, string> kvp = BuildAttributeContentPair();
            dict.Add(kvp.Key, kvp.Value);

            return dict;
        }
        private KeyValuePair<string, string> BuildAttributeContentPair()
        {
            var validatedContent = ValidateContent();
            return new KeyValuePair<string, string>(AttributeName, validatedContent);
        }

        private string ValidateContent()
        {
            if (ContentValue.Equals(ContentDefault))
            {
                ContentValid = false;
                ContentErrorMsg = $"{ContentValue} is the default value";
            }
            else
            {
                ContentValid = true;
                ContentErrorMsg = "";
            }
            return ContentValue;
        }
    }
}

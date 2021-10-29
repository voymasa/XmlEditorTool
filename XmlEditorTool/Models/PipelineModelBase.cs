using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Models
{
    public class PipelineModelBase
    {
        public string AttributeName { get; set; }
        public string DataType { get; set; }
        public string ContentHeader { get; set; }
        public string ContentValue { get; set; }

        public string ContentDefaultValue { get; set; }
        public bool IsDefaultSet { get; set; }
        public bool ContentValid { get; set; }
        public string ContentErrorMsg { get; set; }

        public List<MacroContentBase> ModelContents { get; set; }

        //public PipelineModelBase()
        //{

        //}

        public virtual Dictionary<string, string> BuildAttributeContentDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (MacroContentBase mcb in ModelContents)
            {
                mcb.ValidateContent();
                if (mcb.IsContentValid)
                {
                    //KeyValuePair<string, string> kvp = BuildAttributeContentPair();
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(mcb.ContentHeader, mcb.ContentValue);
                    dict.Add(kvp.Key, kvp.Value);
                }
            }
            //KeyValuePair<string, string> kvp = BuildAttributeContentPair();
            //dict.Add(kvp.Key, kvp.Value);

            return dict;
        }
        //private KeyValuePair<string, string> BuildAttributeContentPair()
        //{
        //    foreach (MacroContentBase mcb in ModelContents)
        //    {
        //        mcb.ValidateContent();
        //    }
        //    var validatedContent = ValidateContent();
        //    return new KeyValuePair<string, string>(AttributeName, validatedContent);
        //}

        //private string ValidateContent()
        //{
        //    if (ContentValue.Equals(ContentDefaultValue))
        //    {
        //        ContentValid = false;
        //        ContentErrorMsg = $"{ContentValue} is the default value";
        //    }
        //    else
        //    {
        //        ContentValid = true;
        //        ContentErrorMsg = "";
        //    }
        //    return ContentValue;
        //}
    }
}

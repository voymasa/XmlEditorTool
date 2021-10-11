using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Models
{
    public class BasicStringModel : PipelineModelBase
    {
        public override Dictionary<string, string> BuildAttributeContentDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (MacroContentBase mcb in ModelContents)
            {
                mcb.ValidateContent();
                if (mcb.IsContentValid)
                {
                    //KeyValuePair<string, string> kvp = BuildAttributeContentPair();
                    dict.Add(AttributeName, mcb.ContentValue.Trim());
                }
            }
            //KeyValuePair<string, string> kvp = BuildAttributeContentPair();
            //dict.Add(kvp.Key, kvp.Value);

            return dict;
        }
    }
}

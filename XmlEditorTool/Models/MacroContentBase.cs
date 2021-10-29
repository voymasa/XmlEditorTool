using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Models
{
    public class MacroContentBase
    {
        public string ContentHeader { get; set; }
        public string ContentValue { get; set; }
        public string ContentDefault { get; set; }
        public bool IsDefaultContent { get; set; }
        public bool IsContentValid { get; set; }
        public string ContentMsg { get; set; }

        public string ValidateContent()
        {
            if (ContentValue.Equals(ContentDefault))
            {
                IsContentValid = false;
                ContentMsg = $"{ContentValue} is the default value";
            }
            else
            {
                IsContentValid = true;
                ContentMsg = "";
            }
            return ContentValue;
        }
    }
}

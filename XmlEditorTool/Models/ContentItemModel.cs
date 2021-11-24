using System.Collections.Generic;

namespace XmlEditorTool.Models
{
    public class ContentItemModel
    {
        public string ContentHeader { get; set; }
        public string ContentValue { get; set; }

        /// <summary>
        /// This method creates a kvp out of the header and value.
        /// If a string is passed in, it is intended to modify the header "key", or be the key if isKey is true
        /// </summary>
        /// <param name="stringToAddToHeader"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> GetContentHeaderValuePair(string? stringForHeader, bool isKey)
        {
            return new KeyValuePair<string, string>(isKey? stringForHeader : (stringForHeader + ContentHeader), ContentValue);
        }
    }
}
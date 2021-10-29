using System.Collections.Generic;

namespace XmlEditorTool.Models
{
    public class PipelineMacroModel
    {
        public string AttributeName { get; set; }
        public string DataType { get; set; }
        public List<ContentItemModel> ContentList { get; set; }
    }
}
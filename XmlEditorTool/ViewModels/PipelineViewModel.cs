using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineMacroModel Model { get; protected set; }

        public string AttributeName
        {
            get { return Model.AttributeName; }
            set
            {
                Model.AttributeName = value;
            }
        }

        public string DataType
        {
            get { return Model.DataType; }
            set { Model.DataType = value; }
        }

        public PipelineViewModel (PipelineMacroModel model)
        {
            Model = model; // the new one should be passed in to this
        }

        public Dictionary<string, string> GenerateAttributeValueForContent()
        {
            return Model.GenerateContentAttributeValues();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class PipelineBasicStringViewModel : ViewModelBase
    {
        public PipelineBasicStringModel Model { get; protected set; }
        public List<PipelineBasicStringModel> Models { get; private set; }
        public string AttributeName
        {
            get { return Model.AttributeName; }
            set
            {
                Model.AttributeName = value;
            }
        }
        public string ViewModelDataType
        {
            get { return Model.ViewModelDataType; }
            set { Model.ViewModelDataType = value; }
        }
        public string ContentHeader
        {
            get { return Model.ContentHeader; }
            set { Model.ContentHeader = value; }
        }
        public string ContentValue
        {
            get { return Model.ContentValue; }
            set
            {
                if (Model.ContentValue != value)
                {
                    Model.ContentValue = value;
                    RaisePropertyChanged(() => ContentValue);
                }
            }
        }
        public string ContentErrorMsg
        {
            get { return !Model.ContentValid ? Model.ContentErrorMsg : ""; }
        }

        public PipelineBasicStringViewModel(string macroName, 
            string attributeName, string dataType, string header, string content)
        {
            Model = new PipelineBasicStringModel();
            Models = new List<PipelineBasicStringModel>();
            MacroName = macroName;
            Model.AttributeName = attributeName;
            Model.ViewModelDataType = dataType;
            Model.ContentHeader = header;
            Model.ContentValue = content;
            Model.ContentDefault = content;
            Model.IsDefaultSet = true;
            Models.Add(Model);
        }

        override public Dictionary<string, string> CreateAttributeValueDictionaryFromData()
        {
            AttributeValueDictionary = Model.BuildAttributeContentDictionary();
            return AttributeValueDictionary;
        }
    }
}

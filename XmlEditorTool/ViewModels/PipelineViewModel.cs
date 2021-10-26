using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    public class PipelineViewModel<T> : ViewModelBase where T : PipelineModelBase
    {
        public T Model { get; protected set; }
        public List<T> Models { get; private set; }

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

        public PipelineViewModel (T model, string macroName, string attributeName, string dataType,
           string header, string content)
        {
            Model = model; // the new one should be passed in to this
            Models = new List<T>();
            MacroName = macroName;
            Model.AttributeName = attributeName;
            Model.DataType = dataType;
            Model.ContentHeader = header;
            Model.ContentValue = content;
            Model.ContentDefaultValue = content;
            Model.IsDefaultSet = true;
            Models.Add(Model);
        }

        public PipelineViewModel (T model, string macroName, string attributeName, string dataType,
            Dictionary<string, string> contents)
        {
            Model = model;
            Models = new List<T>();
            MacroName = macroName;
            Model.AttributeName = attributeName;
            Model.DataType = dataType;

            // The point of this section is to create a Content Object for each piece of content
            // the model will have (such as a Colour having 4 content values: rgbA).
            // TODO -- review and think this over and how it will come in (9/12/2021)
            foreach(string s in contents.Keys)
            {
                MacroContentBase mcb = new MacroContentBase();
                mcb.ContentHeader = contents["ContentHeader"];
                mcb.ContentValue = contents["ContentValue"];
                mcb.ContentDefault = contents["ContentValue"];
                mcb.ContentValue = "";

                Model.ModelContents.Add(mcb);
            }

            Models.Add(Model);
        }
    }
}

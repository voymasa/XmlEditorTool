using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class MacroContentItemViewModel : ViewModelBase
    {
        public MacroContentBase Content { get; protected set; }
        public List<MacroContentBase> Contents { get; private set; }

        public string ContentHeader
        {
            get { return Content.ContentHeader; }
            set
            {
                Content.ContentHeader = value;
            }
        }

        public string ContentValue
        {
            get { return Content.ContentValue; }
            set
            {
                if (Content.ContentValue != value)
                {
                    Content.ContentValue = value;
                    RaisePropertyChanged(() => ContentValue);
                }

                if (Content.ContentValue.Equals(Content.ContentDefault))
                {
                    Content.IsDefaultContent = true;
                }
                else
                {
                    Content.IsDefaultContent = false;
                }
            }
        }

        public MacroContentItemViewModel (MacroContentBase m, string header, string contentValue)
        {
            Content = m;
            Contents = new List<MacroContentBase>();
            Content.ContentHeader = header;
            Content.ContentValue = contentValue;
            Content.ContentDefault = contentValue;
            Content.IsDefaultContent = true;
            Contents.Add(Content);
        }
    }
}

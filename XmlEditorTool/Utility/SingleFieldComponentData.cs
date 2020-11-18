using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class SingleFieldComponentData : ComponentData
    {
        private object contentValue;

        public object ContentValue
        {
            get { return contentValue; }
            set
            {
                contentValue = value;
                OnPropertyChanged("ContentValue");
            }
        }
    }
}

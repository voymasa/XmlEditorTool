using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class DoubleFieldComponentData : ComponentData
    {
        private object contentValue1;
        private object contentValue2;

        public object ContentValue1
        {
            get { return contentValue1; }
            set
            {
                contentValue1 = value;
                OnPropertyChanged("ContentValue1");
            }
        }

        public object ContentValue2
        {
            get { return contentValue2; }
            set
            {
                contentValue2 = value;
                OnPropertyChanged("ContentValue2");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class ComponentData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string attributename;
        private string datatype;
        private object contentValue;

        public string AttributeName
        {
            get { return attributename; }
            set
            {
                attributename = value;
                OnPropertyChanged("AttributeName");
            }
        }

        public string Datatype
        {
            get { return datatype; }
            set
            {
                datatype = value;
                OnPropertyChanged("Datatype");
            }
        }

        public object ContentValue
        {
            get { return contentValue; }
            set
            {
                contentValue = value;
                OnPropertyChanged("ContentValue");
            }
        }

        public void OnPropertyChanged(string propertyname)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}

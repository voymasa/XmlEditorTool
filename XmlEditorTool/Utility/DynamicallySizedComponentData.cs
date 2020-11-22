using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class DynamicallySizedComponentData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string attributename;
        private string datatype;
        private string header1;
        private string header2;
        private string header3;
        private string header4;
        private object content1;
        private object content2;
        private object content3;
        private object content4;

        public DynamicallySizedComponentData()
        {
            header1 = "";
            header2 = "";
            header3 = "";
            header4 = "";
        }

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

        public string Header1
        {
            get { return header1; }
            set
            {
                header1 = value;
                OnPropertyChanged("Header1");
            }
        }
        public string Header2
        {
            get { return header2; }
            set
            {
                header2 = value;
                OnPropertyChanged("Header2");
            }
        }
        public string Header3
        {
            get { return header3; }
            set
            {
                header3 = value;
                OnPropertyChanged("Header3");
            }
        }

        public string Header4
        {
            get { return header4; }
            set
            {
                header4 = value;
                OnPropertyChanged("Header4");
            }
        }

        public object ContentValue1
        {
            get { return content1; }
            set
            {
                content1 = value;
                OnPropertyChanged("ContentValue1");
            }
        }

        public object ContentValue2
        {
            get { return content2; }
            set
            {
                content2 = value;
                OnPropertyChanged("ContentValue2");
            }
        }

        public object ContentValue3
        {
            get { return content3; }
            set
            {
                content3 = value;
                OnPropertyChanged("ContentValue3");
            }
        }

        public object ContentValue4
        {
            get { return content4; }
            set
            {
                content4 = value;
                OnPropertyChanged("ContentValue4");
            }
        }

        public void SetHeaderAndContent(int index, string header, object value)
        {
            switch (index)
            {
                case 0: Header1 = header; ContentValue1 = value; break;
                case 1: Header2 = header; ContentValue2 = value; break;
                case 2: Header3 = header; ContentValue3 = value; break;
                case 3: Header4 = header; ContentValue4 = value; break;
            }
        }

        public void SetContentValue(int index, object value)
        {
            switch(index)
            {
                case 0: ContentValue1 = value; break;
                case 1: ContentValue2 = value; break;
                case 2: ContentValue3 = value; break;
                case 3: ContentValue4 = value; break;
            }
        }

        public void SetHeader(int index, string header)
        {
            switch (index)
            {
                case 0: Header1 = header; break;
                case 1: Header2 = header; break;
                case 2: Header3 = header; break;
                case 3: Header4 = header; break;
            }
        }

        public string GetHeader(int index)
        {
            switch(index)
            {
                case 0: return Header1;
                case 1: return Header2;
                case 2: return Header3;
                case 3: return Header4;
                default: return Header1;
            }
        }

        public void OnPropertyChanged(string propertyname)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}

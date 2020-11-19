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

        protected class Content
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private string header;
            private string contentname;
            private object contentvalue;
            
            public Content(int index) : this(index, new object()) {}

            public Content(int index, object newContent)
            {
                header = "Header" + (index + 1);
                contentname = "ContentValue" + (index + 1);
                ContentValue = newContent;
            }

            public string Header
            {
                get { return header; }
            }

            public object ContentValue { 
                get { return contentvalue; }
                set { 
                    contentvalue = value;
                    OnPropertyChanged(contentname);
                }
            }

            private void OnPropertyChanged(string propertyname)
            {
                var handler = this.PropertyChanged;
                if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private List<Content> contentList { get; set; }

        public DynamicallySizedComponentData()
        {
            contentList = new List<Content>();
        }

        public string GetHeader(int index)
        {
            return contentList[index].Header;
        }

        public object GetContentValue(int index)
        {
            return contentList[index].ContentValue;
        }

        public void SetContentValue(int index, object value)
        {
            contentList[index].ContentValue = value;
            //OnPropertyChanged("ContentValue" + (index + 1));
        }

        public void AddContentValue(object value)
        {
            Content content = new Content(contentList.Count, value);
            contentList.Add(content);
            //OnPropertyChanged("ContentValue" + contentList.Count);
        }

        public void OnPropertyChanged(string propertyname)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}

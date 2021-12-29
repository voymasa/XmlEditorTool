using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlEditorTool.ViewModels
{
    public class XmlElementViewModel : ViewModelBase
    {
        private const string NAME = "Name";
        private const string LOC_NAME = "LocName";
        public XmlElement ElementModel { get; set; }
        //public XElement Element { get; set; }

        private ObservableCollection<XmlElementViewModel> _xmlViewModelCollection;
        public ObservableCollection<XmlElementViewModel> XmlViewModelCollection
        {
            get { return _xmlViewModelCollection; }
            set
            {
                if (_xmlViewModelCollection != value)
                {
                    _xmlViewModelCollection = value;
                    RaisePropertyChanged(() => XmlViewModelCollection);
                }
            }
        }

        public string ElementName
        {
            get
            {
                string name;
                if (ElementModel.HasAttribute(LOC_NAME))
                {
                    name = ElementModel.GetAttribute(LOC_NAME);
                }
                else if (ElementModel.HasAttribute(NAME))
                {
                    name = ElementModel.GetAttribute(NAME);
                }
                else // not optimal for display
                {
                    name = ElementModel.LocalName;
                }
                return name;
            }
            set
            {
                if (!ElementModel.LocalName.Equals(value))
                {
                    ElementModel.SetAttribute(ElementModel.LocalName, value);
                    RaisePropertyChanged(() => ElementName);
                }
            }
        }

        public string ElementComponentName
        {
            get { return ElementModel.LocalName; }
        }

        public string ElementInfo
        {
            get
            {
                var info = ElementModel.OuterXml;

                int openingTagIndex = info.IndexOf("<");
                int closingTagIndex = info.IndexOf(">");

                return info.Substring(openingTagIndex, closingTagIndex + 1); 
            }
            set
            {
                if (ElementModel.Value != value)
                {
                    ElementModel.Value = value;
                    RaisePropertyChanged(() => ElementInfo);
                }
            }
        }

        public void UpdateElementInfo(XmlAttribute attribute, string attrValue)
        {
            if (ElementModel.HasAttribute(attribute.LocalName))
            {
                ElementModel.SetAttribute(attribute.LocalName, attrValue);
            }
            else if (!ElementModel.HasAttribute(attribute.LocalName))
            {
                attribute.Value = attrValue;
                ElementModel.Attributes.Append(attribute);
            }
            //ElementInfo = ElementModel.Value;
            RaisePropertyChanged(() => ElementInfo);
        }

        public XmlElementViewModel(XmlElement e)
        {
            ElementModel = e;
        }
    }
}

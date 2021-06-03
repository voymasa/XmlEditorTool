using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlEditorTool.DataModels
{
    class XmlElementModel
    {
        public XmlElement ElementToModify { get; set; }
        public List<XmlAttribute> XmlAttributes { get; set; }
    }
}

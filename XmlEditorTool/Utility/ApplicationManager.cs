using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlEditorTool.Utility
{
    class ApplicationManager
    {
        private static ApplicationManager singleton;
        public static ApplicationManager GetInstance()
        {
            if (singleton == null)
            {
                singleton = new ApplicationManager();
            }
            return singleton;
        }

        private ApplicationManager()
        {
            XmlElements = new List<XmlElement>();
        }

        public XmlDocument XmlDocument { get; set; }
        public List<XmlElement> XmlElements { get; set; }
        public Dictionary<string,string> XmlMap { get; set; }
        public XmlElement selectedElement { get; set; }
        public string xmlFilePath { get; set; }
    }
}

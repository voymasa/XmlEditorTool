﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool.Models
{
    public class XmlModel
    {
        public string XmlFilePath { get; set; }
        public XmlDocument Document { get; private set; }
        public List<XmlElement> XmlElement { get; private set; }
        public List<XmlElementViewModel> ViewModels { get; protected set; }
        //public Dictionary<string, string> XmlMap { get; set; }
        public XmlElement SelectedElement { get; set; }
        public XmlElementViewModel SelectedElementViewModel { get; set; }
        
        /*
         * 1. set the document to the argument and store the filepath
         * 2. construct the list of all xml elements from the document
         */
        public XmlModel(string filepath)
        {
            XmlFilePath = filepath;
            Document = new XmlDocument();
            Document.Load(filepath);
            XmlElement = new List<XmlElement>();
            ViewModels = new List<XmlElementViewModel>();
        }
    }
}

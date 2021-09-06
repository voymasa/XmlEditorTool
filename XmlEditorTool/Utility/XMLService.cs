using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using XmlEditorTool.Utility;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool
{
    class XMLService
    {
        private const string NAME = "Name";
        private const string LOC_NAME = "LocName";

        public static void BuildTree(String filepath, System.Windows.Controls.TreeView treeView)
        {
            XmlDocument doc = new XmlDocument();
            ApplicationManager.GetInstance().xmlFilePath = filepath;
            doc.Load(filepath);
            ApplicationManager.GetInstance().XmlDocument = doc;
            ApplicationManager.GetInstance().XmlElements.Add(doc.DocumentElement);

            int openTagIndex = doc.DocumentElement.OuterXml.IndexOf("<");
            int closeTagIndex = doc.DocumentElement.OuterXml.IndexOf(">");

            TreeViewItem treeNode = new TreeViewItem
            {
                //Should be Root
                Header = doc.DocumentElement.OuterXml.Substring(openTagIndex, closeTagIndex + 1),
                Name = doc.DocumentElement.GetAttribute(NAME),
                Tag = doc.DocumentElement.LocalName,
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            //ApplicationManager.GetInstance().XmlMap.Add(treeNode.Header.ToString(), doc.DocumentElement.NamespaceURI);
            BuildNodes(treeNode, doc.DocumentElement);
        }

        private static void BuildNodes(TreeViewItem treeNode, XmlElement element)
        {
            foreach (XmlNode child in element.ChildNodes)
            {
                switch (child.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement childElement = child as XmlElement;
                        ApplicationManager.GetInstance().XmlElements.Add(childElement);

                        string specificNode = (childElement.GetAttribute(NAME) != null ? " \"" + childElement.GetAttribute(NAME) + "\"" : "");
                        if (specificNode.Trim().Equals(""))
                            specificNode = (childElement.GetAttribute(LOC_NAME) != null ? " \"" + childElement.GetAttribute(LOC_NAME) + "\"" : null);

                        int openingTagIndex = childElement.OuterXml.IndexOf("<");
                        int closingTagIndex = childElement.OuterXml.IndexOf(">");

                        TreeViewItem childTreeNode = new TreeViewItem
                        {
                            //Get First attribute where it is equal to value
                            Header = childElement.OuterXml.Substring(openingTagIndex, closingTagIndex + 1),//!specificNode.Trim().Equals("") ? (childElement.Name + specificNode) : childElement.Name,
                            Name = childElement.Name,
                            Tag = childElement.LocalName,
                            //Automatically expand elements
                            IsExpanded = true
                        };
                        treeNode.Items.Add(childTreeNode);
                        //ApplicationManager.GetInstance().XmlMap.Add(childTreeNode.Header.ToString(), childElement.NamespaceURI);
                        BuildNodes(childTreeNode, childElement);
                        break;
                    case XmlNodeType.Text:
                        XmlText childText = child as XmlText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value.Trim() != "" ? childText.Value : "", });
                        break;
                }
            }
        }

        public static XmlElement GetXmlElementByTagName(TreeViewItem treeViewItem)
        {
            XmlNodeList nodes = ApplicationManager.GetInstance().XmlDocument.GetElementsByTagName(treeViewItem.Name);
            // 1/16/2021 the nodelist at this point will have multiple items for those with the same "name"
            foreach (XmlNode n in nodes)
            {
                int openingTagIndex = n.OuterXml.IndexOf("<");
                int closingTagIndex = n.OuterXml.IndexOf(">");
                if (n.OuterXml.Substring(openingTagIndex, closingTagIndex + 1).Equals(treeViewItem.Header.ToString()))
                {
                    return n as XmlElement;
                }
            }
            return nodes[0] as XmlElement;
        }

        /// <summary>
        /// Updates the specified xml element with values from the specified tree view item
        /// </summary>
        /// <param name="treeViewItem">The tree view item that contains the values that were modified; this reflects a specific element attribute</param>
        /// <param name="element">The xml element whose attributes are being modified or added to</param>
        public static void UpdateXmlElement(TreeViewItem treeViewItem, XmlElement element)
        {
            // if there is no selected xml element, then immediately return
            if (ApplicationManager.GetInstance().selectedElement == null)
            {
                return;
            }
            // match the component from the datagrid to the xml document element
            // if the attribute exists, then modify it would the current value
            // else, create the attribute and add the value
            // TODO -- good Lord this is ugly, and wrong, and needs to be replaced with a method called on the viewmodel to construct
            // the string value for the attribute
            var tempData = (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue1;
            if (tempData != null)
            {
                UpdateWithData(element, treeViewItem.Header.ToString(),
                    (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue1.ToString().Trim());
            }
            tempData = (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue2;
            if (tempData != null)
            {
                UpdateWithData(element, treeViewItem.Header.ToString(),
                    (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue2.ToString().Trim());
            }
            tempData = (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue3;
            if (tempData != null)
            {
                UpdateWithData(element, treeViewItem.Header.ToString(),
                    (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue3.ToString().Trim());
            }
            tempData = (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue4;
            if (tempData != null)
            {
                UpdateWithData(element, treeViewItem.Header.ToString(),
                    (treeViewItem.DataContext as DynamicallySizedComponentData).ContentValue4.ToString().Trim());
            }
        }

        //public static void UpdateXmlElementUsingViewModel(TreeViewItem tvi, XmlElement element)
        //{
        //    if (ApplicationManager.GetInstance().selectedElement == null)
        //    {
        //        throw new ArgumentNullException("no xml element selected");
        //    }

        //    var viewModel = tvi.DataContext as ViewModelBase;
        //    Dictionary<string, string> kvp = GetAttributeDictionaryFromViewModel(viewModel);
        //}

        //private static Dictionary<string, string> GetAttributeDictionaryFromViewModel(ViewModelBase vm)
        //{
        //    Dictionary<string, string> tempDict = null;

        //    // TODO -- update this to handle every view model case
        //    switch (vm.DisplayName)
        //    {
        //        case "PipelineBasicStringViewModel":
        //            tempDict = (vm as PipelineBasicStringViewModel).AttributeDictionary;
        //            break;
        //        default:
        //            tempDict = vm.AttributeDictionary;
        //            break;
        //    }

        //    return tempDict;
        //}

        private static void UpdateWithData(XmlElement element, string attribute, string data)
        {
            if (element.HasAttribute(attribute))
            {
                // Consider creating a loop hear to iterate through each value of the tree item
                Console.WriteLine(element.GetAttribute(attribute));
                // TODO -- create a method that constructs the attribute and value depending on the attribute
                element.SetAttribute(attribute, data);
                Console.WriteLine(element.GetAttribute(attribute));
            }
            else
            {
                // TODO -- create a method that constructs the attribute and value depending on the attribute
                // TODO --rethink this
                XmlAttribute temp = ApplicationManager.GetInstance().XmlDocument.CreateAttribute(attribute);
                temp.Value = data;
                element.Attributes.Append(temp);
                Console.WriteLine(element.GetAttribute(attribute));
            }
        }

        /**
         * This method parses a file as text and adds any lines that contain the MacroPrefix setting
         * to a list of strings.
         * @param sourceFile a string of the filepath to the sourcefile containing the macros
         * returns List<string> containing the lines that are macros, or null if the MacroPrefix setting hasn't been set
         */
        public static List<string> ParseMacroList(string sourceFile)
        {
            List<string> macros = new List<string>();

            if (!sourceFile.Contains(".h")) // TODO -- figure out a better "null"check
                return macros;
            string filepath = Properties.Settings.Default.SourceFileDir + "/" + sourceFile;
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.MacroPrefix))
                return null;
            using (StreamReader reader = new StreamReader(filepath))
            {
                // read the file line by line and check if it contains the macro prefix
                string currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    if (currentLine != null && (currentLine.Trim().StartsWith("//") || currentLine.Trim().StartsWith("#")))
                    {
                        currentLine = reader.ReadLine();
                        continue;
                    }
                    // if it contains the macro prefix, add that line to the macros List<string> object                
                    else if (currentLine != null && currentLine.Contains(Properties.Settings.Default.MacroPrefix))
                    {
                        macros.Add(currentLine);
                    }
                    currentLine = reader.ReadLine();
                }
            }

            return macros;
        }

        /// <summary>
        /// This method exports the current Application Manager XML Document, to the selected location
        /// </summary>
        /// <param name="manager"> The application manager that owns the xml document being modified</param>
        /// <returns>true if the export succeeded, or false if it fails</returns>
        public static Boolean ExportChangesToXML(ApplicationManager manager)
        {
            Boolean succeeded = false;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "XmlDocument"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "eXtensible Markup Language (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                manager.XmlDocument.Save(filename);
            }

            return succeeded;
        }

        public static void SaveChangesToXML()
        {
            ApplicationManager.GetInstance().XmlDocument.Save(ApplicationManager.GetInstance().xmlFilePath);
        }
    }
}

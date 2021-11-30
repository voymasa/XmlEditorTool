using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using XmlEditorTool.Models;
using XmlEditorTool.Utility;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool
{
    class XMLService
    {
        private const string NAME = "Name";
        private const string LOC_NAME = "LocName";

        public static void BuildTree(XmlModel model, System.Windows.Controls.TreeView treeView)
        {
            //XmlDocument doc = new XmlDocument();
            //ApplicationManager.GetInstance().xmlFilePath = filepath;
            //doc.Load(filepath);
            //ApplicationManager.GetInstance().XmlDocument = doc;
            //ApplicationManager.GetInstance().XmlElements.Add(doc.DocumentElement);
            model.XmlElement.Add(model.Document.DocumentElement);

            int openTagIndex = model.Document.DocumentElement.OuterXml.IndexOf("<");
            int closeTagIndex = model.Document.DocumentElement.OuterXml.IndexOf(">");

            TreeViewItem treeNode = new TreeViewItem
            {
                //Should be Root
                Header = model.Document.DocumentElement.OuterXml.Substring(openTagIndex, closeTagIndex + 1),
                Name = model.Document.DocumentElement.GetAttribute(NAME),
                Tag = model.Document.DocumentElement.LocalName,
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            //ApplicationManager.GetInstance().XmlMap.Add(treeNode.Header.ToString(), doc.DocumentElement.NamespaceURI);
            BuildNodes(treeNode, model, model.Document.DocumentElement);
        }

        private static void BuildNodes(TreeViewItem treeNode, XmlModel model, XmlElement element)
        {
            foreach (XmlNode child in element.ChildNodes)
            {
                switch (child.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement childElement = child as XmlElement;
                        model.XmlElement.Add(childElement);

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
                        BuildNodes(childTreeNode, model, childElement);
                        break;
                    case XmlNodeType.Text:
                        XmlText childText = child as XmlText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value.Trim() != "" ? childText.Value : "", });
                        break;
                }
            }
        }

        public static XmlElement GetXmlElementByTagName(TreeViewItem treeViewItem, XmlModel model)
        {
            XmlNodeList nodes = model.Document.GetElementsByTagName(treeViewItem.Name);
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
        public static void UpdateXmlElement (TreeViewItem treeViewItem, XmlModel model, XmlElement element)
        {
            // if there is no selected xml element, then immediately return
            if (model.SelectedElement == null)
            {
                return;
            }
            
            /*
             * For each treeviewitem get the datacontext of the viewmodel
             * Get the attribute name/value pairs for the content in the viewmodel
             */
            PipelineMacroViewModel pmvm = treeViewItem.DataContext as PipelineMacroViewModel;
            Dictionary<string, string> content = pmvm.GetContentForUpdate();
            foreach (string k in content.Keys)
            {
                // Call the method that creates the information to update the element with
                Console.WriteLine(k);
                Console.WriteLine(content[k]);
                string AttributeToUpdateOrCreate = k;
                string DataForTheAttribute = content[k];
                UpdateWithData(model, element, AttributeToUpdateOrCreate, DataForTheAttribute);
            }
        }

        private static void UpdateWithData(XmlModel model, XmlElement element, string attribute, string data)
        {
            if (element.HasAttribute(attribute))
            {
                // Consider creating a loop hear to iterate through each value of the tree item
                // TODO -- create a method that constructs the attribute and value depending on the attribute
                element.SetAttribute(attribute, data);
            }
            else
            {
                // TODO -- create a method that constructs the attribute and value depending on the attribute
                // TODO --rethink this
                XmlAttribute temp = model.Document.CreateAttribute(attribute);
                temp.Value = data;
                element.Attributes.Append(temp);
            }
        }

        /**
         * This method parses a file as text and adds any lines that contain the MacroPrefix setting
         * to a list of strings.
         * @param sourceFile a string of the filepath to the sourcefile containing the macros
         * returns List<string> containing the lines that are macros, or null if the MacroPrefix setting hasn't been set
         */
        public static List<string> ParseMacroList(XmlModel model, string sourceFile)
        {
            List<string> macros = new List<string>();

            /*
             * @modified: 11/28/2021
             * @by: Aaron Voymas
             * Checking that a file is a .h file limits this tool too much, and if the mapper file has a non .h file which contains pipeline macros
             * there is no reason to prevent that from loading. Changing this to a simple null check;
             */
            if (string.IsNullOrWhiteSpace(sourceFile))
                return macros;
            //string filepath = Properties.Settings.Default.SourceFileDir + "/*/" + sourceFile;
            DirectoryInfo dirInfo = new DirectoryInfo(Properties.Settings.Default.SourceFileDir);
            DirectoryInfo[] subdirs = FileService.SetTopLevelDirs(dirInfo,
                new FileInfo(model.XmlFilePath));
            FileInfo[] fileInfo = null;
            foreach (DirectoryInfo d in subdirs)
            {
                fileInfo = d.GetFiles(sourceFile, SearchOption.AllDirectories);
                // this foreach loop is designed to bounce out at the first file that matches
                if (fileInfo.Length > 0)
                {
                    break;
                }
            }
            
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.MacroPrefix))
            {
                return null;
            }
            if (fileInfo.Length < 1)
            {
                return null;
            }
            using (StreamReader reader = new StreamReader(fileInfo[0].FullName))
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
        public static bool ExportChangesToXML(XmlModel model)
        {
            bool succeeded = false;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "XmlDocument"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "eXtensible Markup Language (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                model.Document.Save(filename);
            }

            return succeeded;
        }

        public static void SaveChangesToXML(XmlModel model)
        {
            model.Document.Save(model.XmlFilePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using XmlEditorTool.Utility;

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
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
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
                            //Automatically expand elements
                            IsExpanded = true
                        };
                        treeNode.Items.Add(childTreeNode);
                        BuildNodes(childTreeNode, childElement);
                        break;
                    case XmlNodeType.Text:
                        XmlText childText = child as XmlText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value.Trim() != "" ? childText.Value : "", });
                        break;
                }
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;

namespace XmlEditorTool
{
    class XMLService
    {
        private const string NAME = "Name";
        private const string LOC_NAME = "LocName";

        public static void BuildTree(String filepath, System.Windows.Controls.TreeView treeView)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            TreeViewItem treeNode = new TreeViewItem
            {
                //Should be Root
                Header = doc.DocumentElement.Name,
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

                        string specificNode = (childElement.GetAttribute(NAME) != null ? " \"" + childElement.GetAttribute(NAME) + "\"" : "");
                        if (specificNode.Trim().Equals(""))
                            specificNode = (childElement.GetAttribute(LOC_NAME) != null ? " \"" + childElement.GetAttribute(LOC_NAME) + "\"" : null);

                        TreeViewItem childTreeNode = new TreeViewItem
                        {
                            //Get First attribute where it is equal to value
                            Header = !specificNode.Trim().Equals("") ? (childElement.Name + specificNode) : childElement.Name,
                            Name = childElement.Name,
                            //Automatically expand elements
                            IsExpanded = true
                        };
                        treeNode.Items.Add(childTreeNode);
                        BuildNodes(childTreeNode, childElement);
                        break;
                    case XmlNodeType.Text:
                        XmlText childText = child as XmlText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value.Trim() != ""? childText.Value : "", });
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
                string currentLine;
                do
                {
                    currentLine = reader.ReadLine();
                    // if it contains the macro prefix, add that line to the macros List<string> object                
                    if (currentLine != null && currentLine.Contains(Properties.Settings.Default.MacroPrefix))
                    {
                        macros.Add(currentLine);
                    }
                } while (reader.ReadLine() != null) ;
            }

            return macros;
        }
    }
}

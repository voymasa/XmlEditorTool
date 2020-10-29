using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XmlEditorTool
{
    class XMLService
    {
        public static void LoadTreeViewFromXmlFile(string filename, System.Windows.Controls.TreeView view)
        {
            //Load the XML Document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            // Add the root node's children to the TreeView
            view.Items.Clear();
            System.Windows.Forms.TreeView treeView = new System.Windows.Forms.TreeView();
            AddTreeViewChildNodes(treeView.Nodes, xmlDoc.DocumentElement);
            view.DataContext = treeView;
        }

        private static void AddTreeViewChildNodes(
            TreeNodeCollection parentNodes, XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                // Make the new TreeView node
                TreeNode newNode = parentNodes.Add(childNode.Name);

                // Recursively make this node's descendants
                AddTreeViewChildNodes(newNode.Nodes, childNode);

                // If this is a leaf node, make sure it's visible
                if (newNode.Nodes.Count == 0) newNode.EnsureVisible();
            }
        }

        public static void LoadTreeViewItemsFromXmlFile(string filename, System.Windows.Controls.TreeView view)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            view.Items.Clear();
            AddTreeViewItemNodes(view.Items, xmlDoc.DocumentElement);
        }

        private static void AddTreeViewItemNodes(
            ItemCollection parentItems, XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                // Make the TreeViewItem
                XmlNode node = (XmlNode)parentItems[parentItems.Add(childNode.Name)];
                TreeViewItem newItem = new TreeViewItem();
                newItem.DataContext = node;

                // Make the descendants of the TreeViewItem
                AddTreeViewItemNodes(newItem.Items, childNode);

                // If this is a leaf item, make sure it's visible
                if (newItem.Items.Count < 1)
                {
                    newItem.Visibility = Visibility.Visible;
                }
            }
        }

        public static void BuildTree(String filepath, System.Windows.Controls.TreeView treeView)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            TreeViewItem treeNode = new TreeViewItem
            {
                //Should be Root
                Header = doc.DocumentElement.Name,
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
                        TreeViewItem childTreeNode = new TreeViewItem
                        {
                            //Get First attribute where it is equal to value
                            Header = childElement.Name,//.Attributes().First(s => s.Name == "value").Value,
                            //DataContext = childElement,
                            //Automatically expand elements
                            IsExpanded = true
                        };
                        treeNode.Items.Add(childTreeNode);
                        BuildNodes(childTreeNode, childElement);
                        break;
                    case XmlNodeType.Text:
                        XmlText childText = child as XmlText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value, });
                        break;
                }
            }
        }
    }
}

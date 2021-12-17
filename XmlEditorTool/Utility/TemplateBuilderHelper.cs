using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using XmlEditorTool.Models;
using XmlEditorTool.ViewModels;
using XmlEditorTool.Views;
using static XmlEditorTool.Utility.DynamicallySizedComponentData;

namespace XmlEditorTool.Utility
{
    class TemplateBuilderHelper
    {
        public static void BuildTreeViewFromPipelineMacros(TreeView treeView, List<string> pipelineMacroDataList, XmlModel model, string elementName)
        {
            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (string s in pipelineMacroDataList)
            {
                // figure out where the information of the macro begins; assume it's not an actual pipeline macro if there isn't (
                int openParIndex = s.IndexOf("(");
                int closeParIndex = s.IndexOf(")");
                if (openParIndex < 0)
                {
                    continue;
                }

                string macroName = s.Substring(0, openParIndex);
                string[] args = s.Substring(openParIndex, closeParIndex - openParIndex).Replace("(", "")
                    .Replace(")", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //TreeViewItem treeViewItem = PipelineMacroHelper.CreateMacroTreeViewItem(macroName.Trim(), args, elementName);
                TreeViewItem treeViewItem = PipelineMacroHelper.BuildTreeViewItem(model, macroName.Trim(), args, elementName);


                if (treeViewItem != null)
                {
                    treeViewItems.Add(treeViewItem);
                }
            }
            treeView.ItemsSource = treeViewItems;
            treeView.Visibility = Visibility.Visible;
        }
    }
}

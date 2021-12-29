using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            List<TreeView> treeViewItems = new List<TreeView>();
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
                //TreeViewItem treeViewItem = PipelineMacroHelper.BuildTreeViewItem(model, macroName.Trim(), args, elementName);


                //if (treeViewItem != null)
                //{
                //    treeViewItems.Add(treeViewItem);
                //}

                // Create a treeview to represent the macro and content
                // Create the pipeline tree item and add it to the tree
                // Create the content tree items and add them to the tree
                // Add the pipe treeview to the base treeview
                TreeView pipeTree = new TreeView
                {
                    Visibility = Visibility.Visible
                };

                TreeViewItem pipelineMacroItem = PipelineMacroHelper.CreatePipelineMacroItem(macroName.Trim(), args, elementName);
                if (pipelineMacroItem != null)
                {
                    pipeTree.Items.Add(pipelineMacroItem);
                }

                ObservableCollection<TreeViewItem> contentItems = PipelineMacroHelper.CreateContentItemList(model, macroName, args, elementName);
                foreach (TreeViewItem item in contentItems)
                {
                    if (item != null)
                    {
                        if (pipelineMacroItem != null)
                        {
                            var pipelineViewModel = pipelineMacroItem.DataContext as PipelineViewModel;
                            var contentViewModel = item.DataContext as ContentItemViewModel;
                            pipelineViewModel.Model.ContentCollection.Add(contentViewModel);
                        }
                        pipeTree.Items.Add(item);
                    }
                }

                treeViewItems.Add(pipeTree);
                //treeView.Items.Add(pipeTree);
            }
            treeView.ItemsSource = treeViewItems;
            treeView.Visibility = Visibility.Visible;
        }
    }
}

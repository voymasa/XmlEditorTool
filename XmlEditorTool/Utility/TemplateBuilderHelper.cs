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
        // TODO: refactor this to determine the data template to use for each item in the macro list, rather than trying to tell it to use the same data template for each
        public static void BuildComponentDataTreeView(DataTemplate template, TreeView treeView, List<string> dataList, string itemName)
        {
            List<TreeViewItem> tvItemList = new List<TreeViewItem>();
            
            // iterate through the list and compare the substring preceding the ( to the macro table/enums
            foreach (string s in dataList)
            {
                // TODO: this is where the data template component should be determined
                // TODO: create the specific data model/view model and fill in its data, then add it to the tree view as a tree view item
                List<DynamicallySizedComponentData> componentDataList = new List<DynamicallySizedComponentData>();

                // create the Data based upon the macro, and parse the values from the substring between the () and split by ,
                DynamicallySizedComponentData data = new DynamicallySizedComponentData();
                int openParIndex = s.IndexOf("(");
                int closeParIndex = s.IndexOf(")");
                if (openParIndex < 0)
                    continue;
                string macroName = s.Substring(0, openParIndex);
                string[] args = s.Substring(openParIndex, closeParIndex - openParIndex).Replace("(", "").Replace(")", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                data.AttributeName = args[0];
                // Add content and headers
                int argStartIdx = MacroMapperHelper.GetInstance().GetValuesStartIndex(macroName);
                for (int i = 0; i < MacroMapperHelper.GetInstance().GetNumDefaultValues(macroName); i++)
                {
                    data.SetHeaderAndContent(i, MacroMapperHelper.GetInstance().GetHeaders(macroName)[i], args[argStartIdx + i]);
                    // find the corresponding datatype of the macro content value, i.e. default value
                    data.Datatype = MacroMapperHelper.GetInstance().GetDatatype(macroName); // this will be determined by some mapper between the macro name and a list
                                                                                            // get the value of the attribute with the same name, and grab the value
                    XmlElement xmlElement = ApplicationManager.GetInstance().XmlElements.Find(x => x.Name.Equals(itemName) && x.HasAttribute(args[0]));
                    if (xmlElement != null)
                    {
                        string attributeValue = xmlElement.GetAttribute(args[0]);
                        data.SetContentValue(i, attributeValue);
                        //data.SetContentValue(0, attributeValue);
                    }
                }

                //DataGrid dataGrid = BuildDataGrid(data);
                TreeViewItem item = (TreeViewItem)template.LoadContent();
                item.Header = data.AttributeName;
                item.Name = data.AttributeName;
                item.IsExpanded = true;
                item.DataContext = data; // 8/15/2021: added to give access to the "viewModel" containing the data

                componentDataList.Add(data);
                (item.Items.GetItemAt(0) as DataGrid).ItemsSource = componentDataList;
                for (int i = 0; i < 4; i++)
                {
                    (item.Items.GetItemAt(0) as DataGrid).Columns[i + 2].Header = data.GetHeader(i);
                }
                tvItemList.Add(item);
            }

            treeView.ItemsSource = tvItemList;
            treeView.Visibility = Visibility.Visible;
        }

        public static void BuildTreeViewFromPipelineMacros(TreeView treeView, List<string> pipelineMacroDataList, string elementName)
        {
            //string TEST_MACRO_NAME = "PIPELINE_BASICSTRING"; // used for testing. eliminate once proven to work

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (string s in pipelineMacroDataList)
            {
                // figure out where the information of the macro begins; assume it's not an actual pipeline macro if there isn't (
                int openParIndex = s.IndexOf("(");
                int closeParIndex = s.IndexOf(")");
                if (openParIndex < 0)
                    continue;

                string macroName = s.Substring(0, openParIndex);
                string[] args = s.Substring(openParIndex, closeParIndex - openParIndex).Replace("(", "")
                    .Replace(")", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //TreeViewItem treeViewItem = PipelineMacroHelper.CreateMacroTreeViewItem(macroName.Trim(), args, elementName);
                TreeViewItem treeViewItem = PipelineMacroHelper.BuildTreeViewItem(macroName.Trim(), args, elementName);


                if (treeViewItem != null)
                {
                    treeViewItems.Add(treeViewItem);
                }
            }
            treeView.ItemsSource = treeViewItems;
            treeView.Visibility = Visibility.Visible;
        }

        public static void BuildTreeViewItemFromContent(TreeView treeView, string macroName, List<MacroContentBase> contents)
        {
            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (MacroContentBase mcb in contents)
            {
                TreeViewItem treeViewItem = CreateTreeViewItemFromContents(macroName, mcb);// Code to return TreeViewItem built with contents

                if (treeViewItem != null)
                {
                    treeViewItems.Add(treeViewItem);
                }
            }
            treeView.ItemsSource = treeViewItems;
            treeView.Visibility = Visibility.Visible;
        }

        public static TreeViewItem CreateTreeViewItemFromContents(string macroName, MacroContentBase m)
        {
            TreeViewItem item = (TreeViewItem)new MacroContentItemView().GetDataTemplate().LoadContent();
            MacroContentItemViewModel vm = new MacroContentItemViewModel(
                m,
                MacroMapperHelper.GetInstance().GetHeaders(macroName)[0],
                m.ContentValue
                );
            item.Header = vm.ContentHeader;
            item.Name = vm.ContentHeader;
            item.DataContext = vm;
            (item.Items.GetItemAt(0) as ItemsControl).ItemsSource = vm.Contents;
            item.IsExpanded = true;
            return item;
        }
    }
}

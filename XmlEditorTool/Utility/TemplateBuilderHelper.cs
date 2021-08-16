using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
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

        private static DataGrid BuildDataGrid(DynamicallySizedComponentData component)
        {
            DataGrid grid = new DataGrid();

            return grid;
        }

        public static void BuildTreeViewItemWithDataGrid(TreeView treeView, List<string> macroList)
        {

        }
    }
}

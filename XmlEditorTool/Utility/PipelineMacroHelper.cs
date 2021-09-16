using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XmlEditorTool.Views;
using XmlEditorTool.ViewModels;
using System.Xml;
using XmlEditorTool.Models;

namespace XmlEditorTool.Utility
{
    class PipelineMacroHelper
    {
        public static TreeViewItem CreateMacroTreeViewItem(string macroName, string[] arguments, string elementName)
        {
            TreeViewItem item;
            //DataTemplate dataTemplate;
            XmlElement xmlElement = ApplicationManager.GetInstance().XmlElements.Find(x => x.Name.Equals(elementName) && x.HasAttribute(arguments[0]));
            switch (macroName)
            {
                case "PIPELINE_BASICSTRING":
                    item = (TreeViewItem)new PipelineBasicStringView().GetDataTemplate().LoadContent();
                    //PipelineBasicStringViewModel vm = new PipelineBasicStringViewModel(macroName,
                    //    arguments[0], MacroMapperHelper.GetInstance().GetDatatype(macroName),
                    //    MacroMapperHelper.GetInstance().GetHeaders(macroName)[0],
                    //    xmlElement != null? xmlElement.GetAttribute(arguments[0]) : "");
                    PipelineViewModel<BasicStringModel> vm = new PipelineViewModel<BasicStringModel>(
                        new BasicStringModel(), macroName,
                        arguments[0], MacroMapperHelper.GetInstance().GetDatatype(macroName),
                        MacroMapperHelper.GetInstance().GetHeaders(macroName)[0],
                        xmlElement != null ? xmlElement.GetAttribute(arguments[0]) : "");
                    item.Header = vm.AttributeName;
                    item.Name = vm.AttributeName;
                    item.DataContext = vm;
                    (item.Items.GetItemAt(0) as DataGrid).ItemsSource = vm.Models;
                    break;
                default:
                    return null;
            }

            item.IsExpanded = true;
            return item;
        }
    }
}

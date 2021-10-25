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
                    //(item.Items.GetItemAt(0) as ItemsControl).ItemsSource = vm.Models;
                    break;
                default:
                    return null;
            }

            item.IsExpanded = true;
            return item;
        }

        public static TreeViewItem BuildTreeViewItem(string macroName, string[] args, string elementName)
        {
            TreeViewItem item = (TreeViewItem)new PipelineMacroView().GetPipelineTemplate().LoadContent();
            XmlElement xmlElement = ApplicationManager.GetInstance().XmlElements.Find(x => x.Name.Equals(elementName) && x.HasAttribute(args[0]));

            // create contentlist
            List<ContentItemModel> ContentItemList = new List<ContentItemModel>();
            /*
             * for each header in the macro csv file, create a contentitem model and assign the values
             */
            List<string> headerList = MacroMapperHelper.GetInstance().GetHeaders(macroName);
            for (int i = 0; i < headerList.Count; i++)
            {
                ContentItemModel contentModel = new ContentItemModel();
                contentModel.ContentHeader = headerList[i];
                contentModel.ContentValue = xmlElement != null ? xmlElement.GetAttribute(args[0]) : "";
                ContentItemList.Add(contentModel);
            }
            // create pipelinemodellist
            List<PipelineMacroModel> PipelineModelList = new List<PipelineMacroModel>();
            PipelineMacroModel pipelineModel = new PipelineMacroModel();
            pipelineModel.AttributeName = args[0];
            pipelineModel.DataType = MacroMapperHelper.GetInstance().GetDatatype(macroName);
            pipelineModel.ContentList = ContentItemList;
            PipelineModelList.Add(pipelineModel);
            // create component model
            ComponentViewModel cvm = new ComponentViewModel(PipelineModelList);
            // add component model to item data context
            item.Header = cvm.PipelineMacroViewModelCollection[0].AttributeName;
            item.Name = cvm.PipelineMacroViewModelCollection[0].AttributeName;
            item.DataContext = cvm;
            item.IsExpanded = true;
            return item;
        }

        /// <summary>
        /// Using the contents of the model, create the treeview items from the template and add it to the treeview
        /// </summary>
        /// <param name="treeView">the treeview for the contents of the view</param>
        /// <param name="model">the model that contains the contents to use for the building</param>
        private static void BuildTreeViewItemsFromContent(TreeView treeView, DataTemplate dataTemplate, PipelineModelBase model)
        {
            // TODO: will want to abstract the model better
        }
    }
}

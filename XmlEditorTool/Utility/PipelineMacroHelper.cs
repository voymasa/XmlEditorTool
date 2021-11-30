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
using System.Collections.ObjectModel;

namespace XmlEditorTool.Utility
{
    class PipelineMacroHelper
    {
        public static TreeViewItem BuildTreeViewItem(XmlModel model, string macroName, string[] args, string elementName)
        {
            PipelineMacroView pmv = new PipelineMacroView();
            TreeViewItem item = pmv.PipelineTreeItem;
            XmlElement xmlElement = model.XmlElement.Find(x => x.Name.Equals(elementName) && x.HasAttribute(args[0]));

            // create contentlist
            List<ContentItemModel> ContentItemList = new List<ContentItemModel>();
            /*
             * for each header in the macro csv file, create a contentitem model and assign the values
             */
            List<string> headerList = MacroMapperHelper.GetInstance().GetHeaders(macroName);
            for (int i = 0; i < headerList.Count; i++)
            {
                ContentItemModel contentModel = new ContentItemModel(args[i + 1]); // the plus 1 is to bypass the attribute name from the arg list
                contentModel.ContentHeader = headerList[i];
                // set to default value first, then to the value, if any, from the existing xml element
                if (xmlElement != null)
                {
                    contentModel.ContentValue = xmlElement.GetAttribute(args[0]);
                    contentModel.IsDefaultValue = false;
                }
                ContentItemList.Add(contentModel);
            }
            // create pipelinemodellist
            List<PipelineMacroModel> PipelineModelList = new List<PipelineMacroModel>();
            PipelineMacroModel pipelineModel = new PipelineMacroModel();
            pipelineModel.AttributeName = args[0];
            pipelineModel.DataType = MacroMapperHelper.GetInstance().GetDatatype(macroName);
            pipelineModel.ContentList = ContentItemList;
            PipelineModelList.Add(pipelineModel);
            ObservableCollection<PipelineMacroViewModel> pmvmCollection = new ObservableCollection<PipelineMacroViewModel>();
            //item.ItemsSource = pmvmCollection;
            foreach (PipelineMacroModel p in PipelineModelList)
            {
                pmvmCollection.Add(new PipelineMacroViewModel(p));
            }
            // create component model
            //ComponentViewModel cvm = new ComponentViewModel(PipelineModelList);
            // add component model to item data context
            pmv.ContentTreeView.ItemTemplate = pmv.GetContentTemplate();
            pmv.ContentTreeView.ItemsSource = pmvmCollection[0].ContentItemViewModelCollection;
            pmv.ContentTreeView.Header = "Content";
            //pmv.ContentTreeView.DataContext = pmvmCollection[0].ContentItemViewModelCollection;
            pmv.ContentTreeView.IsExpanded = true;
            item.Header = pmvmCollection[0].AttributeName;
            //item.Name = pmvmCollection[0].AttributeName;
            item.DataContext = pmvmCollection[0];
            item.IsExpanded = true;
            return item;
        }
    }
}

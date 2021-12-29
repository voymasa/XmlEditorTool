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
            if (!MacroMapperHelper.GetInstance().HasMacroInfo(macroName))
            {
                return null;
            }
            PipelineMacroView pmv = new PipelineMacroView();
            TreeViewItem item = pmv.PipelineTreeViewItem;
            XmlElement xmlElement = model.XmlElement.Find(x => x.Name.Equals(elementName) && x.HasAttribute(args[0]));

            // create contentlist
            List<ContentItemModel> ContentItemList = new List<ContentItemModel>();
            /*
             * for each header in the macro csv file, create a contentitem model and assign the values
             */
            List<string> headerList;
            try
            {
                headerList = MacroMapperHelper.GetInstance().GetHeaders(macroName);
            }
            catch (NullReferenceException nre)
            {
                return null;
            }
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
            ObservableCollection<TreeViewItem> contentItemCollection = new ObservableCollection<TreeViewItem>();
            foreach (ContentItemModel cModel in ContentItemList)
            {
                ContentItemViewModel cViewModel = new ContentItemViewModel(cModel);
                MacroContentItemView mView = new MacroContentItemView();
                TreeViewItem cItem = mView.ContentTreeItem;
                cItem.Header = cViewModel.ContentHeader;
                cItem.DataContext = cViewModel;
                cItem.IsExpanded = true;
                contentItemCollection.Add(cItem);
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
            //pmv.ContentTreeView.ItemTemplate = pmv.GetContentTemplate();
            pmv.ContentTreeView.ItemsSource = contentItemCollection;//pmvmCollection[0].ContentItemViewModelCollection;
            //pmv.ContentTreeView.DataContext = pmvmCollection[0].ContentItemViewModelCollection;
            pmv.ContentTreeView.IsExpanded = true;
            item.Header = pmvmCollection[0].AttributeName;
            //item.Name = pmvmCollection[0].AttributeName;
            item.DataContext = pmvmCollection[0];
            item.IsExpanded = true;
            return item;
        }

        public static TreeViewItem CreatePipelineMacroItem(string macroName, string[] macroInfo, string elementName)
        {
            if (!MacroMapperHelper.GetInstance().HasMacroInfo(macroName))
            {
                return null;
            }

            PipelineMacroView _pipelineView = new PipelineMacroView();
            TreeViewItem _pipelineItem = new TreeViewItem()
            {
                Header = elementName + ":" + macroInfo[0],
                ItemTemplate = _pipelineView.GetMacroDataTemplate(),
                IsExpanded = true,
                Visibility = Visibility.Visible
            };

            PipelineMacroModel _pipelineModel = CreatePipelineModel(macroInfo[0], MacroMapperHelper.GetInstance().GetDatatype(macroName));
            PipelineViewModel _pipelineViewModel = CreatePipelineViewModel(_pipelineModel);
            
            _pipelineItem.DataContext = _pipelineViewModel;
            _pipelineItem.ItemsSource = new ObservableCollection<PipelineViewModel> { _pipelineViewModel };

            return _pipelineItem;
        }

        private static PipelineMacroModel CreatePipelineModel(string attrName, string dataType)
        {
            PipelineMacroModel _model = new PipelineMacroModel()
            { 
                AttributeName = attrName,
                DataType = dataType,
                ContentCollection = new ObservableCollection<ContentItemViewModel>()
            };
            return _model;
        }

        private static PipelineViewModel CreatePipelineViewModel(PipelineMacroModel model)
        {
            return new PipelineViewModel(model);
        }

        public static ObservableCollection<TreeViewItem> CreateContentItemList(XmlModel xmlModel, string macroName, string[] macroInfo, string elementName)
        {
            if (!MacroMapperHelper.GetInstance().HasMacroInfo(macroName))
            {
                return null;
            }
            ObservableCollection<TreeViewItem> contentItems = new ObservableCollection<TreeViewItem>();
            List<string> headerList = MacroMapperHelper.GetInstance().GetHeaders(macroName);
            string attributeName = headerList[0];
            XmlElement xmlElement = xmlModel.XmlElement.Find(x => x.Name.Equals(elementName) && x.HasAttribute(macroInfo[0]));
            
            for (int i = 0; i < headerList.Count; i++)
            {
                ContentItemModel contentModel = CreateContentItemModel(macroInfo[0], headerList[i], macroInfo[i + 1], xmlElement);
                ContentItemViewModel contentViewModel = new ContentItemViewModel(contentModel);

                MacroContentItemView contentView = new MacroContentItemView();
                TreeViewItem contentItem = new TreeViewItem()
                {
                    ItemTemplate = contentView.GetDataTemplate(),
                    Visibility = Visibility.Visible,
                    IsExpanded = true,
                    DataContext = contentViewModel,
                    Header = macroInfo[0],
                    ItemsSource = new ObservableCollection<ContentItemViewModel> { contentViewModel}
                };
                contentItems.Add(contentItem);
            }

            return contentItems;
        }

        private static ContentItemModel CreateContentItemModel(string attribute, string header, string defaultValue, XmlElement element)
        {
            ContentItemModel contentModel = new ContentItemModel(defaultValue)
            {
                ContentHeader = header
            };
            if (element != null)
            {
                contentModel.ContentValue = element.GetAttribute(attribute);
                contentModel.IsDefaultValue = false;
            }

            return contentModel;
        }
    }
}

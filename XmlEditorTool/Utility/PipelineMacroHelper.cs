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
                contentModel.IsValueApplied = true;
            }

            return contentModel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class PipelineMacroViewModel : ViewModelBase
    {
        public PipelineMacroModel PipelineModel {get; protected set;}

        private ObservableCollection<ContentItemViewModel> _contentViewModelCollection;
        public ObservableCollection<ContentItemViewModel> ContentItemViewModelCollection
        {
            get { return _contentViewModelCollection; }
            set
            {
                if (_contentViewModelCollection != value)
                {
                    _contentViewModelCollection = value;
                    RaisePropertyChanged(() => ContentItemViewModelCollection);
                }
            }
        }

        public string AttributeName
        {
            get { return PipelineModel.AttributeName; }
            set
            {
                if (PipelineModel.AttributeName != value)
                {
                    PipelineModel.AttributeName = value;
                    RaisePropertyChanged(() => AttributeName);
                }
            }
        }

        public string DataType
        {
            get { return PipelineModel.DataType; }
            set
            {
                if (PipelineModel.DataType != value)
                {
                    PipelineModel.DataType = value;
                    RaisePropertyChanged(() => DataType);
                }
            }
        }

        public PipelineMacroViewModel(PipelineMacroModel Model)
        {
            this.PipelineModel = Model;
            ContentItemViewModelCollection = new ObservableCollection<ContentItemViewModel>();
            foreach (ContentItemModel cim in PipelineModel.ContentList)
            {
                ContentItemViewModelCollection.Add(new ContentItemViewModel(cim));
            }
        }

        public Dictionary<string,string> GetContentForUpdate()
        {
            return PipelineModel.GetContentAttributePairs();
        }
    }
}

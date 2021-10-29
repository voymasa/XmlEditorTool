using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class ComponentViewModel : ViewModelBase
    {
        private ObservableCollection<PipelineMacroViewModel> _pipelineMacroViewModelCollection;
        public ObservableCollection<PipelineMacroViewModel> PipelineMacroViewModelCollection
        {
            get { return _pipelineMacroViewModelCollection; }
            set
            {
                if (_pipelineMacroViewModelCollection != value)
                {
                    _pipelineMacroViewModelCollection = value;
                    RaisePropertyChanged(() => PipelineMacroViewModelCollection);
                }
            }
        }

        public ComponentViewModel(List<PipelineMacroModel> PcvmList)
        {
            PipelineMacroViewModelCollection = new ObservableCollection<PipelineMacroViewModel>();
            foreach(PipelineMacroModel p in PcvmList)
            {
                PipelineMacroViewModelCollection.Add(new PipelineMacroViewModel(p));
            }
        }
    }
}

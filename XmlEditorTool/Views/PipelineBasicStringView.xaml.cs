using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool.Views
{
    /// <summary>
    /// Interaction logic for PipelineBasicStringView.xaml
    /// </summary>
    public partial class PipelineBasicStringView : UserControl
    {
        public PipelineBasicStringView()
        {
            InitializeComponent();
            //DataContext = new PipelineBasicStringViewModel("PipelineBasicStringViewModel", null);
        }

        public DataTemplate GetDataTemplate()
        {
            return this.Resources["PipelineBasicStringTemplate"] as DataTemplate;
        }

        //PipelineBasicStringViewModel ViewModel => this.DataContext as PipelineBasicStringViewModel;
    }
}

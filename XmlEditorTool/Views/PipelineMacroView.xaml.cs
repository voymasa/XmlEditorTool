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

namespace XmlEditorTool.Views
{
    /// <summary>
    /// Interaction logic for PipelineMacroView.xaml
    /// </summary>
    public partial class PipelineMacroView : UserControl
    {
        public PipelineMacroView()
        {
            InitializeComponent();
        }

        public DataTemplate GetContentTemplate()
        {
            return this.Resources["ContentItemTemplate"] as DataTemplate;
        }

        public HierarchicalDataTemplate GetPipelineTemplate()
        {
            return this.Resources["PipelineMacroTemplate"] as HierarchicalDataTemplate;
        }
    }
}

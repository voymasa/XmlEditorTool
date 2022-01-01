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
    /// Interaction logic for MacroContentItemView.xaml
    /// </summary>
    public partial class MacroContentItemView : UserControl
    {
        public MacroContentItemView()
        {
            InitializeComponent();
        }

        public DataTemplate GetDataTemplate()
        {
            return this.Resources["ContentTemplate"] as DataTemplate;
        }

        private void ContentValueTextChanged(object sender, TextChangedEventArgs args)
        {
            var panel = (sender as TextBox).Parent as StackPanel;
            var viewModel = panel.DataContext as ContentItemViewModel;
            viewModel.IsValueApplied = false;
        }
    }
}

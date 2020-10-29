using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlEditorTool
{
    /// <summary>
    /// Interaction logic for XmlEditorToolHomeView.xaml
    /// </summary>
    public partial class XmlEditorToolHomeView : Page
    {
        public XmlEditorToolHomeView()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Browse file system
            //FileDialog fileDialog = new FileDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        { }
    }
}

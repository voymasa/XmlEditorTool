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
using XmlEditorTool.Utility;

namespace XmlEditorTool.Views
{
    /// <summary>
    /// Interaction logic for StartPageView.xaml
    /// </summary>
    public partial class StartPageView : Page
    {
        public StartPageView()
        {
            InitializeComponent();
        }
        private void UploadDrop(object sender, DragEventArgs e)
        {
            // Make sure the file is an xml file
            string[] format = e.Data.GetFormats(false);
            bool validFormat = false;
            foreach (string s in format)
            {
                if (s.Equals("FileName"))
                {
                    string[] str = e.Data.GetData(s) as string[];
                    if (str[0].Contains(".xml"))
                        validFormat = true;
                }
            }

            // only load the information if it is an xml and there is data present
            if (validFormat && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                /*
                 * @modified: 11/28/2021
                 * @by: Aaron Voymas
                 * the xml editor window should be constructed and the build tree should be called within that window. must be able to pass the filename
                 * to the editor window
                 */
                XMLEditorViewWindow editorWindow = new XMLEditorViewWindow();
                XMLEditorView editorView = new XMLEditorView(files[0]);
                editorWindow.NavigationService.Navigate(editorView);
                editorWindow.Show();
                //XMLService.BuildTree(files[0], treeView);
            }
        }

        /**
         * This method opens the file selected from the Open Dialogue and displays its
         * contents in the left window panel
         */
        private void UploadFileClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            dlg.Filter = "eXtensible Markup Language (*.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                /*
                 * @modified: 11/28/2021
                 * @by: Aaron Voymas
                 * the xml editor window should be constructed and the build tree should be called within that window. must be able to pass the filename
                 * to the editor window
                 */
                XMLEditorViewWindow editorWindow = new XMLEditorViewWindow();
                XMLEditorView editorView = new XMLEditorView(filename);
                editorWindow.NavigationService.Navigate(editorView);
                editorWindow.Show();
                //XMLService.BuildTree(filename, treeView);
            }
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            // Create object of the settings window object type
            // Call the show method on that window
            SettingsWindowView settings = new SettingsWindowView();
            settings.Show();
        }

        private void ExportXml(object sender, RoutedEventArgs e)
        {
            XMLService.ExportChangesToXML(ApplicationManager.GetInstance());
        }

        private void SaveXml(object sender, RoutedEventArgs e)
        {
            XMLService.SaveChangesToXML();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

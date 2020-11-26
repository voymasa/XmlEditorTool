using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml;
using XmlEditorTool.Utility;

namespace XmlEditorTool
{
    /// <summary>
    /// Interaction logic for XMLEditorView.xaml
    /// </summary>
    public partial class XMLEditorView : Page
    {
        public XMLEditorView()
        {
            InitializeComponent();
        }

        private void UploadDrop(object sender, DragEventArgs e)
        {
            if (XmlTreeView.IsVisible)
            {
                XmlTreeView.Visibility = Visibility.Hidden;
            }
            // Make sure the file is an xml file
            String[] format = e.Data.GetFormats(false);
            bool validFormat = false;
            foreach (String s in format)
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
                XMLService.BuildTree(files[0], XmlTreeView);
                XmlTreeView.Visibility = Visibility.Visible;
            }
        }

        /**
         * This method opens the file selected from the Open Dialogue and displays its
         * contents in the left window panel
         */
        private void UploadFileClick(object sender, RoutedEventArgs e)
        {
            if (XmlTreeView.IsVisible)
            {
                XmlTreeView.Visibility = Visibility.Hidden;
            }
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
                XMLService.BuildTree(filename, XmlTreeView);
                XmlTreeView.Visibility = Visibility.Visible;
            }
        }

        private void XmlTreeViewItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DgGrid.Visibility = Visibility.Hidden;
            string itemName = (e.NewValue as TreeViewItem).Name;
            string fileToUse = ComponentMapperManager.GetInstance().GetSourceFile(itemName);

            FileNameLabel.Content = fileToUse;

            // read through the file and store in a List<string> each line that contains the Macro Prefix setting
            List<string> macroList = XMLService.ParseMacroList(fileToUse);

            TemplateBuilderHelper.BuildComponentDataTreeView(MacroTreeView.Resources["DynamicComponentTemplate"] as DataTemplate, MacroTreeView, macroList, itemName);
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

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            // iterate through each tree item
            for (int i = 0; i < MacroTreeView.Items.Count; i++)
            {
                // match the component from the datagrid to the xml document element
                // if the attribute exists, then modify it would the current value
                // else, create the attribute and add the value
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

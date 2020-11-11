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
            // create/load the datalist here
            var dataList = new List<ComponentData> { 
                new ComponentData { AttributeName = "First", Datatype = "String", ContentValue = "hello" },
                new ComponentData { AttributeName = "Second", Datatype = "Enum", ContentValue = new List<object> { "a","b","ab" } },
                new ComponentData { AttributeName = "Third", Datatype = "Int", ContentValue = 42},
                new ComponentData { AttributeName = "Fourth", Datatype = "Boolean", ContentValue= false}
            };
            DgGrid.ItemsSource = dataList;
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

            List<ComponentData> macroList = new List<ComponentData>();
            // read through the file and store in a List<string> each line that contains the Macro Prefix setting
            // iterate through the list and compare the substring preceding the ( to the macro table/enums
            // create the Data based upon the macro, and parse the values from the substring between the () and split by ,
            // add the Data object to the List<Data>

            // final step is to set the List<Data> as the itemsource for the datagrid
            DgGrid.ItemsSource = macroList;
            DgGrid.Visibility = Visibility.Visible;
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            // Create object of the settings window object type
            // Call the show method on that window
            SettingsWindowView settings = new SettingsWindowView();
            settings.Show();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

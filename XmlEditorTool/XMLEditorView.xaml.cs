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

            List<ComponentData> componentDataList = new List<ComponentData>();

            // read through the file and store in a List<string> each line that contains the Macro Prefix setting
            List<string> macroList = XMLService.ParseMacroList(fileToUse);
            // iterate through the list and compare the substring preceding the ( to the macro table/enums
            foreach (string s in macroList)
            {
                // create the Data based upon the macro, and parse the values from the substring between the () and split by ,
                ComponentData data = new ComponentData();
                int openParIndex = s.IndexOf("(");
                int closeParIndex = s.IndexOf(")");
                if (openParIndex < 0)
                    continue;
                string macroName = s.Substring(0, openParIndex);
                string[] args = s.Substring(openParIndex, closeParIndex - openParIndex).Replace("(","").Replace(")","").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                data.AttributeName = args[0];
                data.ContentValue = args[1];
                // find the corresponding datatype of the macro content value, i.e. default value
                data.Datatype = MacroMapperHelper.GetInstance().GetDatatype(macroName); // this will be determined by some mapper between the macro name and a list
                // get the value of the attribute with the same name, and grab the value
                XmlElement xmlElement = ApplicationManager.GetInstance().XmlElements.Find(x => x.Name.Equals(itemName) && x.HasAttribute(args[0]));
                if (xmlElement != null)
                {
                    string attributeValue = xmlElement.GetAttribute(args[0]);
                    data.ContentValue = attributeValue;
                    //data.SetContentValue(0, attributeValue);
                }
                // add the Data object to the List<Data>
                componentDataList.Add(data);
            }

            // final step is to set the List<Data> as the itemsource for the datagrid
            DgGrid.ItemsSource = componentDataList;
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

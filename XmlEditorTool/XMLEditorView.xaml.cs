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
using XmlEditorTool.ViewModels;

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
            //DgGrid.Visibility = Visibility.Hidden;
            string itemName = (e.NewValue as TreeViewItem).Name;
            string fileToUse = ComponentMapperManager.GetInstance().GetSourceFile(itemName);

            FileNameLabel.Content = fileToUse;

            // set the selected xml element via the treeviewitem index
            ApplicationManager.GetInstance().selectedElement = XMLService.GetXmlElementByTagName(e.NewValue as TreeViewItem);

            // read through the file and store in a List<string> each line that contains the Macro Prefix setting
            List<string> macroList = XMLService.ParseMacroList(fileToUse);

            // TODO: update this after refactoring the method
            //TemplateBuilderHelper.BuildComponentDataTreeView(MacroTreeView.Resources["DynamicComponentTemplate"] as DataTemplate, MacroTreeView, macroList, itemName);
            TemplateBuilderHelper.BuildTreeViewFromPipelineMacros(MacroTreeView, macroList, itemName);
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
                XMLService.UpdateXmlElement(MacroTreeView.Items.GetItemAt(i) as TreeViewItem, ApplicationManager.GetInstance().selectedElement);
            }
        }

        private void ExpandAllProperties(object sender, RoutedEventArgs e)
        {
            for(int i=0; i < MacroTreeView.Items.Count; i++)
            {
                (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = true;
            }
        }

        private void ExpandModifiedProperties(object sender, RoutedEventArgs e)
        {
            CollapseAllProperties();

            for(int i=0; i < MacroTreeView.Items.Count; i++)
            {
                (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = false;

                var dc = (MacroTreeView.Items[i] as TreeViewItem).DataContext as PipelineMacroViewModel;
                
                foreach (ContentItemViewModel c in dc.ContentItemViewModelCollection)
                {
                    if(!c.ContentModel.IsDefaultValue)
                    {
                        (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = true;
                        break;
                    }
                }
            }
        }

        private void CollapseAllProperties(object sender, RoutedEventArgs e)
        {
            CollapseAllProperties();
        }

        private void CollapseAllProperties()
        {
            for (int i = 0; i < MacroTreeView.Items.Count; i++)
            {
                (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = false;
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

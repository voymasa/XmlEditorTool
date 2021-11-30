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

        public XMLEditorView(string xmlFile)
        {
            InitializeComponent();
            XMLService.BuildTree(xmlFile, XmlTreeView);
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

        // TODO -- these should be modified so that the variables and values are local to this window rather than global to the app
        private void ExportXml(object sender, RoutedEventArgs e)
        {
            XMLService.ExportChangesToXML(ApplicationManager.GetInstance());
        }

        private void SaveXml(object sender, RoutedEventArgs e)
        {
            XMLService.SaveChangesToXML();
        }

        private void CloseThisWindow(object sender, RoutedEventArgs e)
        {
            System.Windows.Window.GetWindow(this).Close();
        }
    }
}

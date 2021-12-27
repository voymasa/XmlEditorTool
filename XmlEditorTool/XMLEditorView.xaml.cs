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
using XmlEditorTool.Models;
using XmlEditorTool.Utility;
using XmlEditorTool.ViewModels;

namespace XmlEditorTool
{
    /// <summary>
    /// Interaction logic for XMLEditorView.xaml
    /// </summary>
    public partial class XMLEditorView : Page
    {
        public XmlModel Model { get; protected set; }
        public XMLEditorView()
        {
            InitializeComponent();
        }

        public XMLEditorView(string xmlFile)
        {
            InitializeComponent();
            Model = new XmlModel(xmlFile);
            XMLService.BuildTree(Model, XmlTreeView);
        }

        private void XmlTreeViewItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string itemName = ((e.NewValue as TreeViewItem).DataContext as XmlElementViewModel).ElementComponentName;
            string fileToUse = ComponentMapperManager.GetInstance().GetSourceFile(itemName);

            FileNameLabel.Content = fileToUse;
            
            if (string.IsNullOrWhiteSpace(fileToUse)) // at this point the label will display there is not source file found but the rest of this method will null exception
            {
                return;
            }
            // set the selected xml element via the treeviewitem index
            XmlElementViewModel xmlData = (e.NewValue as TreeViewItem).DataContext as XmlElementViewModel;
            Model.SelectedElement = XMLService.GetXmlElementByTagName(xmlData, Model);

            // read through the file and store in a List<string> each line that contains the Macro Prefix setting
            List<string> macroList = XMLService.ParseMacroList(Model, fileToUse);

            TemplateBuilderHelper.BuildTreeViewFromPipelineMacros(MacroTreeView, macroList, Model, itemName);
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            // iterate through each tree item
            for (int i = 0; i < MacroTreeView.Items.Count; i++)
            {
                XMLService.UpdateXmlElement(MacroTreeView.Items.GetItemAt(i) as TreeViewItem, Model, Model.SelectedElement);
            }
        }

        private void ExpandAllProperties(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MacroTreeView.Items.Count; i++)
            {
                (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = true;
            }
        }

        private void ExpandModifiedProperties(object sender, RoutedEventArgs e)
        {
            CollapseAllProperties();

            for (int i = 0; i < MacroTreeView.Items.Count; i++)
            {
                (MacroTreeView.Items[i] as TreeViewItem).IsExpanded = false;

                PipelineMacroViewModel dc = (MacroTreeView.Items[i] as TreeViewItem).DataContext as PipelineMacroViewModel;
                
                foreach (ContentItemViewModel c in dc.ContentItemViewModelCollection)
                {
                    if (!c.ContentModel.IsDefaultValue)
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

        private void ExportXml(object sender, RoutedEventArgs e)
        {
            XMLService.ExportChangesToXML(Model);
        }

        private void SaveXml(object sender, RoutedEventArgs e)
        {
            XMLService.SaveChangesToXML(Model);
        }

        private void CloseThisWindow(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

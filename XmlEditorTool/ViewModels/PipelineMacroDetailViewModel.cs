using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class PipelineMacroDetailViewModel : ViewModelBase
    {
        private ObservableCollection<XmlElementDataGridTreeViewViewModel> _xmlElementCollection;
        public ObservableCollection<XmlElementDataGridTreeViewViewModel> XmlElementCollection
        {
            get { return _xmlElementCollection; }
            set
            {
                if (_xmlElementCollection != value)
                {
                    _xmlElementCollection = value;
                    RaisePropertyChanged(() => XmlElementCollection);
                }
            }
        }

        public PipelineMacroDetailViewModel()
        {
            XmlElementCollection = new ObservableCollection<XmlElementDataGridTreeViewViewModel>();
            List<XmlElementDataGridTreeViewModel> elementList = GetElementList();
            foreach (XmlElementDataGridTreeViewModel m in elementList)
            {
                XmlElementCollection.Add(new XmlElementDataGridTreeViewViewModel(m));
            }
        }

        #region Methods
        List<XmlAttributeDataGridTreeItemModel> GetAttributeList()
        {
            List<XmlAttributeDataGridTreeItemModel> list = new List<XmlAttributeDataGridTreeItemModel>();
            list.Add(new XmlAttributeDataGridTreeItemModel { 
                TreeItemID = 1,
                AttributeName = "FileName",
                ContentType = "string",
                DataGrid = new System.Windows.Controls.DataGrid(),
                //DataGridAttributeSource = new System.Xml.XmlAttribute(
                //    "Prefix1",
                //    "LocalName1",
                //    "NameSpaceUri1",
                //    new System.Xml.XmlDocument()
                //)
            });
            return list;
        }

        List<XmlElementDataGridTreeViewModel> GetElementList()
        {
            //List<XmlAttributeDataGridTreeItemModel>
            List<XmlElementDataGridTreeViewModel> list = new List<XmlElementDataGridTreeViewModel>();


            return list;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static XmlEditorTool.XMLEditorView;

namespace XmlEditorTool.Utility
{
    class DataGridCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EnumTemplate { get; set; }
        public DataTemplate IntegerTemplate { get; set; }
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BooleanTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
            System.Windows.DependencyObject container)
        {
            if (item is ComponentData)
            {
                if ((item as ComponentData).ContentValue is Int32)
                    return IntegerTemplate;
                else if ((item as ComponentData).ContentValue is Boolean)
                    return BooleanTemplate;
                else if ((item as ComponentData).ContentValue is string)
                    return StringTemplate;
                else if ((item as ComponentData).ContentValue is IList<object>)
                    return EnumTemplate;
            }
            return null;
        }
    }
}

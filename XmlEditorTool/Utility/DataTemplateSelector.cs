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
        public DataTemplate DynamicComponentTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
            System.Windows.DependencyObject container)
        {
            if (item is DynamicallySizedComponentData)
            {
                return DynamicComponentTemplate;
            }
            else if (item is ComponentData)
            {
                if ((item as ComponentData).Datatype.Equals("int"))
                    return IntegerTemplate;
                else if ((item as ComponentData).Datatype.Equals("boolean"))
                    return BooleanTemplate;
                else if ((item as ComponentData).Datatype.Equals("string"))
                    return StringTemplate;
                else if ((item as ComponentData).Datatype.Equals("enum"))
                    return EnumTemplate;
            }
            return null;
        }
    }
}

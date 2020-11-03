using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Properties;

namespace XmlEditorTool.Utility
{
    class ComponentMapperManager
    {
        private static ComponentMapperManager singleton;
        public static ComponentMapperManager GetInstance()
        {
            if (singleton == null)
            {
                singleton = new ComponentMapperManager();
            }
            return singleton;
        }

        private ComponentMapperManager()
        {
            ReloadMapping();
        }

        private Dictionary<string,string> ComponentSourceFileMap;
        public Dictionary<string,string> ComponentSourceFileMapProperty { get => ComponentSourceFileMap; set => ComponentSourceFileMap = value; }

        private void ReloadMapping()
        {
            ComponentSourceFileMapProperty = null;
            CreateComponentSourceMapping();
        }

        private void CreateComponentSourceMapping()
        {
            /*
             * load the csv file
             * parse the contents with a csv parser, into the dictionary
             */
            // TODO -- change the const string reference to use a settings value
            using (var stream = new StreamReader(Settings.Default.MapperFile))
            using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    var dt = new DataTable();
                    dt.Columns.Add("Component", typeof(string));
                    dt.Columns.Add("SourceFile", typeof(string));

                    dt.Load(dr);

                    ComponentSourceFileMapProperty = dt.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                        row => row.Field<string>(1));
                }
            }

        }

        private string SearchPropertyMapByKey(string key)
        {
            string msg = "No associated source file mapped";
            try
            {
                return ComponentSourceFileMapProperty[key];
            }
            catch (KeyNotFoundException)
            {
                return msg;
            }
        }

        public string GetSourceFile(string component)
        {
            return SearchPropertyMapByKey(component);
        }
    }
}

using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class MacroMapperHelper
    {
        private static MacroMapperHelper singleton;
        public static MacroMapperHelper GetInstance()
        {
            if (singleton == null)
            {
                singleton = new MacroMapperHelper();
            }
            return singleton;
        }

        private MacroMapperHelper()
        {
            ReloadMapping();
        }

        public Dictionary<string, string> MacroDatatypeMapperProperty { get; set; }

        private void ReloadMapping()
        {
            MacroDatatypeMapperProperty = null;
            CreateDatatypeMapping();
        }

        private void CreateDatatypeMapping()
        {
            /*
             * load the csv file
             * parse the contents with a csv parser, into the dictionary
             */
            // TODO -- change the const string reference to use a settings value
            using (var stream = new StreamReader(Properties.Settings.Default.DatatypeMapFile))
            using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    var dt = new DataTable();
                    dt.Columns.Add("Macro", typeof(string));
                    dt.Columns.Add("Datatype", typeof(string));

                    dt.Load(dr);

                    MacroDatatypeMapperProperty = dt.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                        row => row.Field<string>(1));
                }
            }

        }

        private string SearchPropertyMapByKey(string key)
        {
            string msg = "No associated source file mapped";
            try
            {
                return MacroDatatypeMapperProperty[key];
            }
            catch (KeyNotFoundException)
            {
                return msg;
            }
        }

        public string GetDatatype(string macroArg)
        {
            return SearchPropertyMapByKey(macroArg);
        }
    }
}

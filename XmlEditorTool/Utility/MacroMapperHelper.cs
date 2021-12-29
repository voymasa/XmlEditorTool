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

        public List<DataRow> MacroDatatypeMapperProperty { get; set; }

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
                    csv.Configuration.MissingFieldFound = null;

                    var dt = new DataTable();
                    dt.Columns.Add("Macro", typeof(string));
                    dt.Columns.Add("Datatype", typeof(string));
                    dt.Columns.Add("NumDefaultArgs", typeof(int));
                    dt.Columns.Add("StartIndex", typeof(int));
                    dt.Columns.Add("Header1", typeof(string));
                    dt.Columns.Add("Header2", typeof(string));
                    dt.Columns.Add("Header3", typeof(string));
                    dt.Columns.Add("Header4", typeof(string));

                    dt.Load(dr);

                    MacroDatatypeMapperProperty = dt.AsEnumerable().ToList();
                }
            }
        }

        /// <summary>
        /// This method looks through the list of macros for one that matches the macro name
        /// </summary>
        /// <param name="macro">the name of the macro</param>
        /// <returns>the datarow containing the information about that macro, or null if it isn't in the list</returns>
        private DataRow SearchPropertyListByMacro(string macro)
        {
            return MacroDatatypeMapperProperty.Find(x => x.ItemArray[0].Equals(macro.Trim()));
        }

        public bool HasMacroInfo(string macro)
        {
            return MacroDatatypeMapperProperty.Find(x => x.ItemArray[0].Equals(macro.Trim())) != null;
        }

        /// <summary>
        /// This method gets the data type stored in the macro data file
        /// </summary>
        /// <param name="macroArg">the name of the macro</param>
        /// <returns>a string representation of the datatype</returns>
        public string GetDatatype(string macroArg)
        {
            return (string)SearchPropertyListByMacro(macroArg).ItemArray[1];
        }

        /// <summary>
        /// This method gets the number of default values that the macro has
        /// </summary>
        /// <param name="macroArg">the name of the macro</param>
        /// <returns>an integer representation of the number of arguments in the macro are default values</returns>
        public int GetNumDefaultValues(string macroArg)
        {
            return (int)SearchPropertyListByMacro(macroArg).ItemArray[2];
        }

        /// <summary>
        /// This method returns an integer representing the argument of the macro where the default values
        /// being, as a zero-based index
        /// </summary>
        /// <param name="macroArg">the name of the macro</param>
        /// <returns>an integer representing the zero-based index of the first default value of
        /// the macro</returns>
        public int GetValuesStartIndex(string macroArg)
        {
            return (int)SearchPropertyListByMacro(macroArg).ItemArray[3];
        }

        /// <summary>
        /// This method returns a list of the headers for each of the values that the macro
        /// will expect the user to fill in
        /// </summary>
        /// <param name="macroArg">the name of the macro</param>
        /// <returns>a List containing string represenations of the header names</returns>
        public List<string> GetHeaders(string macroArg)
        {
            List<string> headerList = new List<string>();
            DataRow data = SearchPropertyListByMacro(macroArg);
            if (data == null)
            {
                throw new NullReferenceException("No associated macro mapped in the csv.");
            }
            int numHeaders = GetNumDefaultValues(macroArg);

            for (int i = 0; i < numHeaders; i++)
            {
                headerList.Add((string)data.ItemArray[4 + i]);
            }

            return headerList;
        }
    }
}

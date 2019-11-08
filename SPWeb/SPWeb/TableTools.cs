using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;

using System.Web.Services;

namespace SPWeb
{

 

    public static class TableTools
    {
        public static Exception Exception;
        public static void Merge( DataTable dt, DataTable destiny)
        {
            destiny.Clear();
            destiny.Merge(dt);
            destiny.AcceptChanges();
            dt.Dispose();
        }
        public static void GetTableFromWebAndStoreFile(Uri rtxUri, string currentRatesFile)
        {
            string curtxsresult = getTableFromWeb(rtxUri);
            if (!string.IsNullOrEmpty(curtxsresult))
            {
                storeTable(curtxsresult, currentRatesFile);
            }
        }

        public static DataTable ImportRows(DataTable schemaToClone, DataRow[] toImport)
        {
            schemaToClone = schemaToClone.Clone();
            foreach (var item in toImport)
            {
                schemaToClone.ImportRow(item);
            }
            schemaToClone.AcceptChanges();
            return schemaToClone;
        }

        public static DataTable LoadTableFromFile(string destiny)
        {
            System.Data.DataTable dt = null;
            Exception = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXml(destiny);

                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Exception = ex;
            }

            return dt;
        }

        public static void SerializeTableAs( DataTable output, ref DataTable input, bool disposeInput = true)
        {
            output.Clear();
            output.Load(input.CreateDataReader());
            output.AcceptChanges();
            if (disposeInput)   input.Dispose();
        }

        public static DataTable SortBy(string fieldSort, DataTable aTable)
        {
            DataView dv = aTable.DefaultView;
            dv.Sort = fieldSort + " desc";
            DataTable rawTable = dv.ToTable();
            SerializeTableAs( aTable, ref rawTable);

            return aTable;
        }

        private static string getTableFromWeb(Uri uri)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string result = string.Empty;

            Exception = null;
            try
            {
                result = client.DownloadString(uri);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }

            client.Dispose();

            return result;
        }
        private static void storeTable(string tableText, string destiny)
        {
            Exception = null;
            if (string.IsNullOrEmpty(destiny)) return;

            try
            {
                System.IO.File.WriteAllText(destiny, tableText);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
    }
}
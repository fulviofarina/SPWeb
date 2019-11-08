using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;

using System.Web.Services;

namespace SPWeb
{

    public static class dsUtil
    {
        private static string pathPreFix = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string currentRatesFile = pathPreFix + "\\currentRtx.xml";
        private static string currentTxFile = pathPreFix + "\\currentTxs.xml";

        private static Uri rtxUri = new Uri("http://quiet-stone-2094.herokuapp.com/rates.xml");
        private static Uri txUri = new Uri("http://quiet-stone-2094.herokuapp.com/transactions.xml");

        private static string separator = "*";
        private static string to = " in ";
       

        public static DataTable LoadTransactions(string desiredCurrency, string desiredSKU, string justTotals, string orderByColumn, string refreshFromWeb)
        {
         
            //should refresh from web?
            bool refreshWeb = string.IsNullOrEmpty(refreshFromWeb);
            ds.TransactionsDataTable tdt;
            loadTransactionsFromWeb(out tdt, refreshWeb);

            //filter by SKU
            desiredSKU = desiredSKU.Trim();
            if (!string.IsNullOrEmpty(desiredSKU))
            {
                var arr = tdt.WhereSKU(desiredSKU);
                tdt = TableTools.ImportRows(tdt, arr) as ds.TransactionsDataTable;
            }

            //order
            if (string.IsNullOrEmpty(orderByColumn.Trim())) orderByColumn = "sku, currency";
            tdt = TableTools.SortBy(orderByColumn.Trim(), tdt) as ds.TransactionsDataTable;


            desiredCurrency = desiredCurrency.Trim();
            bool fillTotals = !string.IsNullOrEmpty(justTotals);
            ds.RatesDataTable rdt = LoadRates(string.Empty, string.Empty, string.Empty, "n") as ds.RatesDataTable;

            DataTable dtResult = convertTransactionsRates(desiredCurrency, ref tdt, ref rdt, fillTotals);


            return dtResult;
        }


        public static DataTable LoadRates(string currencyFrom, string currencyTo, string orderBy, string refreshFromWeb)
        {
         
            //should refresh from web?
            bool refreshWeb = string.IsNullOrEmpty(refreshFromWeb);
            ds.RatesDataTable rdt;
            loadRatesFromWeb(out rdt, refreshWeb);

            //filter by from
            if (!string.IsNullOrEmpty(currencyFrom))
            {
                var arr = rdt.WhereFrom(currencyFrom);
                rdt = TableTools.ImportRows(rdt, arr) as ds.RatesDataTable;
            }
            //filter by to
            if (!string.IsNullOrEmpty(currencyTo))
            {
                var arr = rdt.WhereTo(currencyTo);
                rdt = TableTools.ImportRows(rdt, arr) as ds.RatesDataTable;
            }

            //order by
            if (string.IsNullOrEmpty(orderBy)) orderBy = "from";
            rdt = TableTools.SortBy(orderBy.Trim(), rdt) as ds.RatesDataTable;

        
            return rdt;
        }

        private static DataTable convertTransactionsRates(string desiredCurrency, ref ds.TransactionsDataTable tdt, ref ds.RatesDataTable rdt, bool fillTotals)
        {


            ds.TransactionsDataTable tdtTot = new ds.TransactionsDataTable();
            var distinctSku = tdt.SelectDistinctSKU();
            bool conversionNeeded = !string.IsNullOrEmpty(desiredCurrency);

            foreach (var sku in distinctSku)
            {
                //transacciones de 1 sku
                IEnumerable<ds.TransactionsRow> txsBySKU = tdt.WhereSKU(sku);
                //distintos tipos de monedas (distintas a desiredCurrency) para ese sku
                string[] distinctCurrencies = txsBySKU.Select(o => o.currency).Distinct().ToArray();

                //computar el total en el  desiredCurrency
                foreach (string thisCurrency in distinctCurrencies)
                {
                    //la tasa
                    decimal rate = 1;
                    if (conversionNeeded)
                    {
                         var r =  rdt.First(thisCurrency, desiredCurrency);
                         if (r != null) rate = r.rate;
                    }
                    decimal currencyTotal = 0;
                    currencyTotal = convertAndGetTotalAmount(ref txsBySKU, rate, thisCurrency);

                    if (fillTotals)
                    {
                        string label = string.Empty;
                        label = thisCurrency + to + thisCurrency;

                        if (conversionNeeded)
                        {
                            label = thisCurrency + to + desiredCurrency;
                        }
                        addTxsTotal(ref tdtTot, sku, label, currencyTotal);
                    }
                }

                if (conversionNeeded)
                {
                    decimal sum = tdtTot.Where(o => o.sku.CompareTo(sku) == 0).Sum(o => o.amount);
                    string label = "TOTAL" + to + desiredCurrency;
                    addTxsTotal(ref tdtTot, sku, label, sum);
                }

            }

            if (fillTotals)
            {
                tdtTot.AcceptChanges();
                return tdtTot;
            }
            else
            {

                cleanTxsConversion(desiredCurrency, ref tdt);
                tdt.AcceptChanges();
                return tdt;
            }


        }

        private static void loadRatesFromWeb(out ds.RatesDataTable rdt, bool refreshFromWeb)
        {

            if (refreshFromWeb)
            {
                TableTools.GetTableFromWebAndStoreFile(rtxUri, currentRatesFile);
            }

            DataTable ratesRaw = TableTools.LoadTableFromFile(currentRatesFile);
            rdt = new ds.RatesDataTable();
            TableTools.SerializeTableAs(rdt, ref ratesRaw);

            if (refreshFromWeb)
            {
                fillMissingRates(ref rdt);
                rdt.AcceptChanges();
                rdt.WriteXml(currentRatesFile, XmlWriteMode.IgnoreSchema);
            }

        }

        private static void loadTransactionsFromWeb(out ds.TransactionsDataTable tdt, bool refreshFromWeb)
        {
            if (refreshFromWeb)
            {
                TableTools.GetTableFromWebAndStoreFile(txUri, currentTxFile);
            }

            DataTable txsRaw = TableTools.LoadTableFromFile(currentTxFile);
            tdt = new ds.TransactionsDataTable();
            TableTools.SerializeTableAs(tdt, ref txsRaw);
          
        }

        private static void addTxsTotal(ref ds.TransactionsDataTable tdtTot, string sku, string label, decimal currencyTotal)
        {
            ds.TransactionsRow t1 = tdtTot.NewTransactionsRow();
            t1.sku = sku;
            t1.amount = Decimal.Round(currencyTotal, 2);
            t1.currency = label;

            tdtTot.AddTransactionsRow(t1);
        }

        private static void cleanTxsConversion(string desiredCurrency, ref ds.TransactionsDataTable tdt)
        {
            foreach (var item in tdt)
            {
                if (string.IsNullOrEmpty(desiredCurrency))
                {
                    item.currency = item.currency.Replace(separator, null);
                }
                else item.currency = desiredCurrency;
            }
        }

        private static decimal convertAndGetTotalAmount(ref IEnumerable<ds.TransactionsRow> skuTxs, decimal rate, string aCurrency)
        {
            IEnumerable<ds.TransactionsRow> enume = skuTxs.Where(o => o.currency.Trim().CompareTo(aCurrency) == 0).ToArray();

            decimal dTot = 0;
            foreach (ds.TransactionsRow item in enume)
            {
                item.amount = Decimal.Round(item.amount * rate, 2);
                item.currency = separator + item.currency;
                dTot += item.amount;
            }

            return dTot;
        }

        private static void fillMissingRates(ref ds.RatesDataTable rdt)
        {
            IEnumerable<string> distinct = rdt.Select(o => o.from).Distinct().ToArray();

            foreach (var item in distinct)
            {
                foreach (var item2 in distinct)
                {
                    if (item.CompareTo(item2) != 0)
                    {
                        ds.RatesRow r = rdt.FirstOrDefault(o => o.from.CompareTo(item) == 0 && o.to.CompareTo(item2) == 0);

                        if (r == null)
                        {
                            ds.RatesRow refe = rdt.FirstOrDefault(o => o.from.CompareTo(item2) == 0 && o.to.CompareTo(item) == 0);

                            r = rdt.NewRatesRow();
                            r.from = item;
                            r.to = item2;

                            if (refe == null)
                            {
                                IEnumerable<ds.RatesRow> refesFrom = rdt.Where(o => o.from.CompareTo(item) == 0).ToArray();

                                foreach (var refeFrom in refesFrom)
                                {
                                    ds.RatesRow refeTo = rdt.FirstOrDefault(o => o.from.CompareTo(refeFrom.to) == 0 && o.to.CompareTo(item2) == 0);

                                    if (refeTo == null) continue;

                                    r.rate = Decimal.Round(refeFrom.rate * refeTo.rate, 4);
                                    break;
                                }
                            }
                            else
                            {
                                r.rate = Decimal.Round(1 / refe.rate, 4);
                            }

                            rdt.AddRatesRow(r);
                        }
                    }
                }
            }
        }
    }

  
}
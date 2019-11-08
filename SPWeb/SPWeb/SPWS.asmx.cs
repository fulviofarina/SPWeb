using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;

namespace SPWeb
{
   


    /// <summary>
    /// Summary description for SPWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/SPWeb/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the
    // following line. [System.Web.Script.Services.ScriptService]
    public class SPWS : System.Web.Services.WebService
    {
   
        [WebMethod]
        public DataTable LoadRates(string currencyFrom, string currencyTo, string orderBy, string refreshFromWeb)
        {
            return dsUtil.LoadRates(currencyFrom, currencyTo, orderBy, refreshFromWeb);
        }

        /// <summary>
        /// Refreshed the Transactions Table with the following arguments
        /// </summary>
        /// <param name="desiredCurrency"></param>
        /// <param name="desiredSKU"></param>
        /// <param name="justTotals">Obtains a total's per currency Table</param>
        /// <param name="orderByColumn"></param>
        /// <param name="refreshFromWeb"></param>
        /// <returns></returns>
        [WebMethod]
        public DataTable LoadTransactions(string desiredCurrency, string desiredSKU, string justTotals, string orderByColumn, string refreshFromWeb)
        {
           return  dsUtil.LoadTransactions(desiredCurrency, desiredSKU, justTotals, orderByColumn, refreshFromWeb);

        }




    }
}
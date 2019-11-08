using System.Linq;

namespace SPWeb
{


    public partial class ds
    {
        public partial class TransactionsDataTable
        {
            
            public TransactionsRow[] WhereSKU(string desiredSKU)
            {
                var arr = this.Where(o => o.sku.CompareTo(desiredSKU.Trim()) == 0).ToArray();

                return arr;
            }
            public string[] SelectDistinctSKU()
            {
                return this.Select(o => o.sku).Distinct().ToArray();
            }
            public string[] SelectDistinctCurrencies()
            {
                return this.Select(o => o.currency).Distinct().ToArray();
            }
        }
        public partial class RatesDataTable
        {
            public RatesRow First(string from, string to)
            {
                var r = this.FirstOrDefault(o => o.from.CompareTo(from) == 0 && o.to.CompareTo(to) == 0);
                return r;
            }
            public string[] SelectDistinctFrom()
            {
                return this.Select(o => o.from).Distinct().ToArray();
            }
            public string[] SelectDistinctTo()
            {
                return this.Select(o => o.to).Distinct().ToArray();
            }
            public RatesRow[] WhereFrom(string currencyFrom)
            {
                var arr = this.Where(o => o.from.CompareTo(currencyFrom.Trim()) == 0).ToArray();

                return arr;
            }
            public RatesRow[] WhereTo(string currencyTo)
            {
                var arr = this.Where(o => o.to.CompareTo(currencyTo.Trim()) == 0).ToArray();

                return arr;
            }
        }

    }
}

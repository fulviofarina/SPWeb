using System;
using System.Data;
using System.Web.UI.WebControls;

namespace SPWeb
{
    public partial class WebForm : System.Web.UI.Page
    {
        private localhost.SPWS client = null;
        private ds dataset = null;
        protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] currencyNodes = null;
            string[] skus = null;

            string from = string.Empty;
            string desiredSKU = string.Empty;
            string all = "*";

            string refreshfromWebRts = string.Empty;
            string refreshfromWebTxs = string.Empty;

            if (IsPostBack)
            {
                refreshfromWebRts = "n";
                refreshfromWebTxs = "n";
                from = this.ListBox2.SelectedItem.Text;
                if (from.CompareTo(all) == 0)
                {
                    from = string.Empty;
                    refreshfromWebRts = string.Empty;
                }
                desiredSKU = this.ListBox3.SelectedItem.Text;
                if (desiredSKU.CompareTo(all) == 0)
                {
                    desiredSKU = string.Empty;
                    refreshfromWebTxs = string.Empty;
                }
            }

            dataset = new ds();
            client = new localhost.SPWS();

            DataTable dt = client.LoadRates(from, string.Empty, string.Empty, refreshfromWebRts);
            DataTable destiny = dataset.Rates;
            TableTools.Merge(dt, destiny);

            DataTable txs = client.LoadTransactions(from, desiredSKU, string.Empty, string.Empty, refreshfromWebTxs);
            destiny = dataset.Transactions;
            TableTools.Merge(txs, destiny);

            //load totals
            txs = client.LoadTransactions(from, desiredSKU, "y", string.Empty, "n");

            destiny = dataset.TransactionsTotals;
            TableTools.Merge(txs, destiny);

            object binding = dataset.Rates;
            GridView theGrid = this.GridView1;
            bind(ref binding, ref theGrid);

            binding = dataset.TransactionsTotals;
            theGrid = this.GridView3;
            bind(ref binding, ref theGrid);

            binding = dataset.Transactions;
            theGrid = this.GridView2;
            bind(ref binding, ref theGrid);

            if (!IsPostBack)
            {
                currencyNodes = dataset.Rates.SelectDistinctFrom();
                skus = dataset.Transactions.SelectDistinctSKU();
            }

            if (!IsPostBack)
            {
                ListBox lbox = this.ListBox2;
                fillBox(currencyNodes, from, all, ref lbox);

                fillBox(skus, desiredSKU, all, ref lbox);
            }
        }

        private static void bind(ref object binding, ref GridView theGrid)
        {
            theGrid.DataSource = binding;
            // this.GridView1.DataMember = dataset.Rates.TableName;
            theGrid.DataBind();
            theGrid.Visible = true;
        }

        private static void fillBox(string[] nodes, string selectTag, string allTag, ref ListBox lbox)
        {
            lbox.Items.Clear();
            lbox.Items.Add(allTag);

            foreach (var item in nodes)
            {
                lbox.Items.Add(item);
            }
            if (string.IsNullOrEmpty(selectTag))
            {
                lbox.Items.FindByText(allTag).Selected = true;
            }
            else
            {
                lbox.Items.FindByText(selectTag).Selected = true;
            }
        }
    }
}
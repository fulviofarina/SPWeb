using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPWeb
{
    public partial class WebForm : System.Web.UI.Page
    {

      


       
        private  ds dataset = null;
        private  localhost.SPWS client = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] currencyNodes = null;
            string[] skus = null;

            string from = string.Empty;
            string desiredSKU = string.Empty;

            string all = "*";

            string  refreshfromWebRts = string.Empty;

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

            if (!IsPostBack)
            {
                currencyNodes = dataset.Rates.SelectDistinctFrom();
            }



            DataTable txs = client.LoadTransactions(from, desiredSKU, string.Empty, string.Empty, refreshfromWebTxs);
                 destiny = dataset.Transactions;
                TableTools.Merge(txs, destiny);

            if (!IsPostBack)
            {
                skus = dataset.Transactions.SelectDistinctSKU();
            }
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
                this.ListBox2.Items.Clear();
                this.ListBox2.Items.Add(all);

                foreach (var item in currencyNodes)
                {
                    this.ListBox2.Items.Add(item);
                }
                if (string.IsNullOrEmpty(from))
                {
                    this.ListBox2.Items.FindByText(all).Selected = true;
                }
                else
                {
                    this.ListBox2.Items.FindByText(from).Selected = true;
                }

                this.ListBox3.Items.Clear();
                this.ListBox3.Items.Add(all);

                foreach (var item in skus)
                {
                    this.ListBox3.Items.Add(item);
                }
                if (string.IsNullOrEmpty(desiredSKU))
                {
                    this.ListBox3.Items.FindByText(all).Selected = true;
                }
                else
                {
                    this.ListBox3.Items.FindByText(desiredSKU).Selected = true;
                }


            }


        }

       

        /*
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox ls = sender as ListBox;

            DataTable dt = client.LoadRates(ls.SelectedItem.Text, string.Empty, string.Empty, "n");

            DataTable destiny = dataset.Rates;
            TableTools.Merge(dt, destiny);

            object binding = destiny;
            GridView theGrid = this.GridView1;
            bind(ref binding, ref theGrid);
        }
        */

        private static void bind(ref object binding, ref GridView theGrid)
        {
            theGrid.DataSource = binding;
            //  this.GridView1.DataMember = dataset.Rates.TableName;
            theGrid.DataBind();
            theGrid.Visible = true;
        }

        protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
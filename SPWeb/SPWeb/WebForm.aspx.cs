﻿using System;
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

            //clean listBox selections
            if (IsPostBack)
            {
                refreshfromWebRts = "n";
                refreshfromWebTxs = "n";
                from = this.ratesListBox.SelectedItem.Text;
                if (from.CompareTo(all) == 0)
                {
                    from = string.Empty;
                    refreshfromWebRts = string.Empty;
                }
                desiredSKU = this.skuListBox.SelectedItem.Text;
                if (desiredSKU.CompareTo(all) == 0)
                {
                    desiredSKU = string.Empty;
                    refreshfromWebTxs = string.Empty;
                }
            }

            loadTables(from, desiredSKU, refreshfromWebRts, refreshfromWebTxs);
            bindGrids();

            if (!IsPostBack)
            {
                currencyNodes = dataset.Rates.SelectDistinctFrom();
                skus = dataset.Transactions.SelectDistinctSKU();
          
                ListBox lbox = this.ratesListBox;
                fillListBox(currencyNodes, from, all, ref lbox);

                 lbox = this.skuListBox;
                fillListBox(skus, desiredSKU, all, ref lbox);
            }
        }

        private void bindGrids()
        {
            object binding = dataset.Rates;
            GridView theGrid = this.GridView1;
            bindAGrid(ref binding, ref theGrid);

            binding = dataset.TransactionsTotals;
            theGrid = this.GridView3;
            bindAGrid(ref binding, ref theGrid);

            binding = dataset.Transactions;
            theGrid = this.GridView2;
            bindAGrid(ref binding, ref theGrid);
        }

        private void loadTables(string from, string desiredSKU, string refreshfromWebRts, string refreshfromWebTxs)
        {
            dataset = new ds();
            client = new localhost.SPWS();

            //load rates
            DataTable dt = client.LoadRates(from, string.Empty, string.Empty, refreshfromWebRts);
            DataTable destiny = dataset.Rates;
            TableTools.Merge(dt, destiny);

            //load txs
            DataTable txs = client.LoadTransactions(from, desiredSKU, string.Empty, string.Empty, refreshfromWebTxs);
            destiny = dataset.Transactions;
            TableTools.Merge(txs, destiny);

            //load totals
            txs = client.LoadTransactions(from, desiredSKU, "y", string.Empty, "n");
            destiny = dataset.TransactionsTotals;
            TableTools.Merge(txs, destiny);
        }

        private static void bindAGrid(ref object binding, ref GridView theGrid)
        {
            theGrid.DataSource = binding;
            // this.GridView1.DataMember = dataset.Rates.TableName;
            theGrid.DataBind();
            theGrid.Visible = true;
        }

        private static void fillListBox(string[] nodes, string selectTag, string allTag, ref ListBox lbox)
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
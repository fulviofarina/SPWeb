<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="SPWeb.WebForm" EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
            width: 148px;
        }

        .auto-style3 {
            margin-top: 0px;
        }

        .auto-style4 {
            width: 262px;
        }
        .auto-style6 {
            width: 131px;
        }
        .auto-style8 {
            margin-left: 0;
        }
        .auto-style9 {
            width: 205px;
        }
        .auto-style10 {
            width: 148px;
        }
        .auto-style11 {
            margin-top: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>

        <table style="width: 100%" aria-orientation="vertical">
            <caption style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">HOMEWORK FULVIO FARINA</caption>
            <tr>
                <th class="auto-style10" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">SKUs</th>
                <th class="auto-style6" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">Currencies</th>
                <th class="auto-style9" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">Rates</th>
               <th class="auto-style4" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">Nothing</th>
            </tr>
            <tr>
                <td class="auto-style10">
                    <asp:ListBox ID="skuListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" CssClass="auto-style11" style="padding:0; margin-left: 0; margin-right: 0; margin-bottom: 0;" Height="279px" Width="103px"></asp:ListBox>
                </td>
                <td class="auto-style6">
                    <asp:ListBox ID="ratesListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" style="padding:0; margin-right: 0; margin-top: 0; margin-bottom: 0;" CssClass="auto-style8" Height="284px" Width="110px"></asp:ListBox>
                </td>
                <td class="auto-style9">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CssClass="auto-style3" style="width:100%;height:100%;margin:0;padding:0;" AllowSorting="True" ForeColor="Black" GridLines="Vertical" >
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" Font-Names="Calibri" />
                        <PagerStyle ForeColor="Black" HorizontalAlign="Center" BackColor="#999999" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                </td>
                <td class="auto-style4">Empty Space</td>
            </tr>

            <tr>
                <th class="auto-style1" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">Transactions Total</th>
            </tr>
            <tr>

                <td class="auto-style10">
                    <asp:GridView ID="GridView3" runat="server" EnableModelValidation="True" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" ShowFooter="True" HorizontalAlign="Justify" style="width:100%;height:100%;margin:0;padding:0;">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#7C6F57" HorizontalAlign="Justify" VerticalAlign="Middle" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" HorizontalAlign="Justify" Font-Names="Calibri" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <th class="auto-style1" style="font-size: large; font-family: Calibri; background-color: #000000; color: #FFFFFF; font-weight: bold; table-layout: auto">Transactions</th>
            </tr>
            <tr>
                <td class="auto-style10">
                    <asp:GridView ID="GridView2" runat="server" EnableModelValidation="True" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" ShowFooter="True" HorizontalAlign="Justify" style="width:100%;height:100%;margin:0;padding:0;" AllowSorting="True">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <EditRowStyle HorizontalAlign="Justify" VerticalAlign="Middle" />
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Justify" Font-Names="Calibri" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SweetsIncSept13.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <div id="header">
                <!--logo image goes here it has an id="logo" -->
                <a href="~/index.aspx" runat="server" id="mlink">
                    <asp:Image ID="logo" runat="server" ImageUrl="~/images/Logo.jpg" />
                </a>
                <!--hyperlink with id of cartLink2 is wrapped around the image for the cart - it has the id cartIcon -->

                <hr />
                <asp:Label ID="lblMasterMessage" runat="server"
                    EnableViewState="False" SkinID="msgLabel"></asp:Label>
            </div>
            <div id="main">

                <div id="col1">

                    <asp:Repeater ID="rptCat" runat="server">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <a href='<%= Request.Url.LocalPath.ToString() + "?id="%><%#Eval("id")%>'>
                                    <%#Eval("name")%></a>

                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div id="col2">
                    <asp:Repeater ID="rptProducts" runat="server">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div>
                                    <img src='<%#Eval("name") %>' />
                                    <span>
                                        <a href='<%= Request.Url.LocalPath.ToString() + "?id="%><%#Eval("id")%>'>
                                            <%#Eval("id")%></a> <span><%#Eval("ProductName")%> <% %><%#((decimal)Eval("productPrice")).ToString("c")%></span>
                                    </span>
                                </div>

                                <h3>Product Featured?</h3>
                                <input type="checkbox" <%# Convert.ToBoolean(Eval("featured")) ? "checked" : string.Empty %> />
                                <p>
                                    <b>Brief Description:</b> <%#Eval("briefDescription") %>
                                </p>
                                <p>
                                    <b>Full Description:</b> <%#Eval("fullDescription") %>
                                </p>
                                <hr />
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div id="col3">
                    <h3>column 3</h3>
                </div>

                <footer>
                    <hr />
                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                    <br />
                    Copyright &copy; <%= DateTime.Now.Year %> SweetsInc Candy Company
              <br />
                    <!--eventually a link to admin login page will appear here -->
                </footer>
            </div>
        </div>
    </form>
</body>
</html>

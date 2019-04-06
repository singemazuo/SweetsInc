<%@ Page Title="" Language="C#" MasterPageFile="~/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SweetsIncSept13.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">

        <div id="col1">

            <asp:Repeater ID="rptCat" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <a href='<%= Request.Url.LocalPath.ToString() + "?catid="%><%#Eval("id")%>'>
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
                            <img src='<%#Eval("imageName") %>' />
                            <span>
                                <a href='<%= Request.Url.LocalPath.ToString() + "?pid="%><%#Eval("id")%>'>
                                    <%#Eval("id")%></a> <span><%#Eval("ProductName")%> <% %><%#((decimal)Eval("productPrice")).ToString("c")%></span>
                            </span>
                        </div>

                        <asp:Panel Visible="false" ID="prodDetails" runat="server">
                            <h3>Product Featured?</h3>
                            <input type="checkbox" <%# Convert.ToBoolean(Eval("featured")) ? "checked" : string.Empty %> />
                            <p>
                                <b>Brief Description:</b> <%#Eval("briefDescription") %>
                            </p>
                            <p>
                                <b>Full Description:</b> <%#Eval("fullDescription") %>
                            </p>
                        </asp:Panel>
                        
                        <hr />
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                    
                    
                </FooterTemplate>
            </asp:Repeater>
            <asp:Panel Visible="false" ID="prodAdd" runat="server">
                <asp:Button ID="btnAddToCart" runat="server" Text="Add To Cart" OnClick="btnAddToCart_Click" />
            </asp:Panel>
        </div>
        <div id="col3">
            <h3>column 3</h3>
        </div>
        </div>
</asp:Content>

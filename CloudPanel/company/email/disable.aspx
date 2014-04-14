<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="disable.aspx.cs" Inherits="CloudPanel.company.email.disable" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
        <div class="pageheader">
        <h2><i class="fa fa-envelope-o"></i><%= Resources.LocalizedText.ExchangeDisable_Title %></h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <br />

                        <p style="text-align: center">
                            <%= Resources.LocalizedText.ExchangeDisable_Warning %>
                        </p>

                        <br />
                        <br />

                        <p style="text-align: center">
                            <b><%= Resources.LocalizedText.ExchangeDisable_RandomCharacters %></b> <asp:Label ID="lbRandomCharacters" runat="server" Text="3H206AA0"></asp:Label>
                        </p>

                        <div style="text-align: center">
                            <asp:TextBox ID="txtRandomCharacters" runat="server"></asp:TextBox>

                            <br /><br />

                            <asp:Button ID="btnDisableExchange" runat="server" Text='<%$ Resources:LocalizedText, ExchangeDisable_ButtonDisable %>' CssClass="btn btn-danger" OnClick="btnDisableExchange_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

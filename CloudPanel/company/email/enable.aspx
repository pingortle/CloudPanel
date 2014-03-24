<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="enable.aspx.cs" Inherits="CloudPanel.company.email.enable" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-envelope-o"></i><%= Resources.LocalizedText.ExchangeEnable_Title %></h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <br />

                        <p style="text-align: center">
                            <%= Resources.LocalizedText.ExchangeEnable_Warning %>
                        </p>

                        <br />

                        <div style="text-align: center">
                            <asp:Button ID="btnEnableExchange" runat="server" Text='<%$ Resources:LocalizedText, ExchangeEnable_ButtonEnable %>' CssClass="btn btn-primary" OnClick="btnEnableExchange_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

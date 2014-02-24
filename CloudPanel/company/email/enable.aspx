<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="enable.aspx.cs" Inherits="CloudPanel.company.email.enable" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-envelope-o"></i>Enable E-mail</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <br />

                        <p style="text-align: center">
                            Your company has not been enabled for E-mail. Before you can create mailboxes, distribution groups, contacts, and other Microsoft Exchange objects you must first enable the company.
                        </p>

                        <br />

                        <div style="text-align: center">
                            <asp:Button ID="btnEnableExchange" runat="server" Text="Enable E-mail" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="disable.aspx.cs" Inherits="CloudPanel.company.email.disable" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
        <div class="pageheader">
        <h2><i class="fa fa-envelope-o"></i>Disable E-mail</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <br />

                        <p style="text-align: center">
                            Disabling E-mail will delete all emails, mailboxes, groups, contacts, and any other objects that were created for your company. This action is not reversable and please make sure this is what you are wanting to do before continuing.
                        </p>

                        <br />
                        <br />

                        <p style="text-align: center">
                            <b>To disable E-mail please type the following characters in the textbox below:</b> <asp:Label ID="lbRandomCharacters" runat="server" Text="3H206AA0"></asp:Label>
                        </p>

                        <div style="text-align: center">
                            <asp:TextBox ID="txtRandomCharacters" runat="server"></asp:TextBox>

                            <br /><br />

                            <asp:Button ID="btnDisableExchange" runat="server" Text="Disable E-mail" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

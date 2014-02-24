<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="contacts.aspx.cs" Inherits="CloudPanel.company.email.contacts" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
        <div class="pageheader">
        <h2><i class="fa fa-user"></i>Contacts</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelContactList" runat="server" CssClass="row">

            <div style="float: right; margin: 10px">
                <asp:Button ID="btnAddContact" runat="server" Text="Add New Contact" CssClass="btn btn-success"/>
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th>Display Name</th>
                                <th>Email</th>
                                <th>Hidden</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Jacob's LIVE account</td>
                                <td>jacobdixon@live.com</td>
                                <th>Yes</th>
                                <td>
                                    <div class="btn-group">
                                        <span class="btn btn-xs btn-primary">Modify</span>
                                        <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("DistinguishedName") %>'>Edit</asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                            <li>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Disable" CommandArgument='<%# Eval("DistinguishedName") %>'>Disable</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- btn-group -->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- table-responsive -->
            </div>
            <!-- col-md-6 -->

        </asp:Panel>

        <asp:Panel ID="panelEditContact" runat="server" CssClass="row">
            <div class="form-horizontal">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Add/Edit Contact</h4>
                        </div>

                        <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Display Name <span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="hfContactDistinguishedName" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 control-label">E-mail <span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 control-label">&nbsp;</label>
                                    <div class="col-sm-4">
                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbHidden" runat="server" />
                                            <label for='<%= cbHidden.ClientID %>'>Is contact hidden from global address list?</label>
                                        </div>
                                    </div>
                                </div>
                        </div>
                        <!-- panel-body -->

                        <div class="panel-footer" style="text-align: right">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" />
                        </div>
                        <!-- panel-footer -->
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

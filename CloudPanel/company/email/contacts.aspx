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
                <asp:Button ID="btnAddContact" runat="server" Text='<%$ Resources:LocalizedText, ExchangeContact_AddNew %>' CssClass="btn btn-success" OnClick="btnAddContact_Click"/>
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th><%= Resources.LocalizedText.ExchangeContact_DisplayName %></th>
                                <th><%= Resources.LocalizedText.ExchangeContact_Email %></th>
                                <th><%= Resources.LocalizedText.ExchangeContact_Hidden %></th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeaterContactList" runat="server" OnItemCommand="repeaterContactList_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("DisplayName") %></td>
                                        <td><%# Eval("Email") %></td>
                                        <td><%# Eval("Hidden").ToString() %></td>
                                        <td>
                                            <div class="btn-group">
                                                <asp:Button ID="btnModify" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Edit" CommandArgument='<%# Eval("DistinguishedName") %>' Text='<%$ Resources:LocalizedText, ExchangeContact_Modify %>' />
                                                <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                                    <span class="caret"></span>
                                                    <span class="sr-only">Toggle Dropdown</span>
                                                </button>
                                                <ul class="dropdown-menu" role="menu">
                                                    <li>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("DistinguishedName") %>'><%= Resources.LocalizedText.ExchangeContact_Delete %></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                            <!-- btn-group -->
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <!-- table-responsive -->
            </div>
            <!-- col-md-6 -->

        </asp:Panel>

        <asp:Panel ID="panelEditContact" runat="server" CssClass="row" Visible="false">
            <div class="form-horizontal">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Add/Edit Contact</h4>
                        </div>

                        <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ExchangeContact_DisplayName %> <span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="hfContactDistinguishedName" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ExchangeContact_Email %>  <span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 control-label">&nbsp;</label>
                                    <div class="col-sm-4">
                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbHidden" runat="server" />
                                            <label for='<%= cbHidden.ClientID %>'><%= Resources.LocalizedText.ExchangeContact_HideContact %> </label>
                                        </div>
                                    </div>
                                </div>
                        </div>
                        <!-- panel-body -->

                        <div class="panel-footer" style="text-align: right">
                            <asp:Button ID="btnCancel" runat="server" Text='<%$ Resources:LocalizedText, ExchangeContact_Cancel %>' CssClass="btn btn-default" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text='<%$ Resources:LocalizedText, ExchangeContact_Submit %>' CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
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

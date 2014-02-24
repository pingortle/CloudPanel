<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="domains.aspx.cs" Inherits="CloudPanel.company.email.domains" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-globe"></i>Accepted Domains</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelDomainList" runat="server" CssClass="row">

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th>Domain Name</th>
                                <th>Type</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>compsysar.com</td>
                                <td>Authoritative</td>
                                <td>
                                    <div class="btn-group">
                                        <span class="btn btn-xs btn-primary">Modify</span>
                                        <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("DomainName") %>'>Edit</asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                            <li>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Disable" CommandArgument='<%# Eval("DomainName") %>'>Disable</asp:LinkButton>
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

        <asp:Panel ID="panelEditAcceptedDomain" runat="server" CssClass="row">
            <div class="form-horizontal">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Edit Accepted Domain</h4>
                        </div>

                        <div class="panel-body">

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Domain Name <span class="asterisk">*</span></label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtDomainName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:HiddenField ID="hfDomainID" runat="server" />
                                </div>
                            </div>

                            <br />
                            <br />

                            <div class="rdio rdio-success">
                                <asp:RadioButton ID="cbAuthoritative" runat="server" GroupName="AcceptedDomain" Checked="true" />
                                <label for="<%= cbAuthoritative.ClientID %>">Authoritative</label>
                            </div>

                            <div class="rdio rdio-success">
                                <asp:RadioButton ID="cbInternalRelay" runat="server" GroupName="AcceptedDomain" />
                                <label for="<%= cbInternalRelay.ClientID %>">Internal Relay</label>
                            </div>

                            <div class="rdio rdio-success">
                                <asp:RadioButton ID="cbExternalRelay" runat="server" GroupName="AcceptedDomain" />
                                <label for="<%= cbExternalRelay.ClientID %>">Internal Relay</label>
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

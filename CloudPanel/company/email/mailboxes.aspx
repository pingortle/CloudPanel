<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="mailboxes.aspx.cs" Inherits="CloudPanel.company.email.mailboxes" %>

<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-envelope-o"></i>Mailboxes</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <!-- Current Mailboxes -->
        <asp:Panel ID="panelMailboxList" runat="server" CssClass="row">

            <div style="float: right; margin: 10px">
                <asp:Button ID="btnAddMailboxes" runat="server" Text="Enable New Mailbox" CssClass="btn btn-success" />
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th>Display Name</th>
                                <th>Email</th>
                                <th>Department</th>
                                <th>Current Size</th>
                                <th>Maximum Size</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Jacob Dixon</td>
                                <td>jdixon@compsysar.com</td>
                                <td></td>
                                <td>2.24 GB</td>
                                <td>4.00 GB</td>
                                <td>
                                    <div class="btn-group">
                                        <span class="btn btn-xs btn-primary">Modify</span>
                                        <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("UserPrincipalName") %>'>Edit</asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                            <li>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Disable" CommandArgument='<%# Eval("UserPrincipalName") %>'>Disable</asp:LinkButton>
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

        <!-- Enable Mailboxes -->
        <asp:Panel ID="panelEnableMailboxes" runat="server" CssClass="row">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Choose Users to Enable</h3>
                </div>
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <table class="table table-striped mb30" id="nonMailboxUsers">
                                <thead>
                                    <tr>
                                        <th>
                                            <input type="checkbox" id="title-checkbox" name="title-checkbox" /></th>
                                        <th>Display Name</th>
                                        <th>First Name</th>
                                        <th>Last Name</th>
                                        <th>Login Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="repeaterNonMailboxes" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbUserCheckbox" runat="server" CssClass="tableRowCheckbox" /></td>
                                                <td><%# Eval("DisplayName") %></td>
                                                <td><%# Eval("FirstName") %></td>
                                                <td><%# Eval("LastName") %></td>
                                                <td><%# Eval("UserPrincipalName") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <!-- table-responsive -->
                    </div>
                    <!-- col-md-12 -->
                </div>
            </div>

            <div class="panel panel-default">
                <div class="form-horizontal">
                    <div class="panel-heading">
                        <h3 class="panel-title">Settings</h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Domain Name <span class="asterisk">*</span></label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtDomainName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:HiddenField ID="hfDomainID" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>


    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.datatables.min.js") %>'></script>
    <script type="text/javascript">
        var nonMailboxDT = null;

        jQuery(document).ready(function () {

            nonMailboxDT = jQuery('#nonMailboxUsers').dataTable({
                "sPaginationType": "full_numbers",
                "bSort": false
            });

            jQuery("select").chosen({
                'min-width': '100px',
                'white-space': 'nowrap',
                disable_search_threshold: 10
            });

            $('#title-checkbox').click(function () {
                $(':checkbox').not(this).attr('checked', this.checked);
            });

        });
    </script>
</asp:Content>

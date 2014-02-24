<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="CloudPanel.company.users" %>

<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-users"></i>Users</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelUserList" runat="server" CssClass="row">

            <div style="float: right; margin: 10px">
                <asp:Button ID="btnAddUser" runat="server" Text="Add New User" CssClass="btn btn-success" OnClick="btnAddUser_Click" />
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th>Display Name</th>
                                <th>Login Name</th>
                                <th>Department</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Jacob Dixon</td>
                                <td>jdixon@compsysar.com</td>
                                <td></td>
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
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserPrincipalName") %>'>Delete</asp:LinkButton>
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

        <asp:Panel ID="panelEditCreateUser" runat="server" Visible="false">

            <!-- User Information -->
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">User Information</h4>
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">First Name<span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="hfEditUserPrincipalName" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Middle Name</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Last Name</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Display Name<span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Department</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Login Details and User Rights -->
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Login Details</h4>
                        </div>

                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label">Login Name</label>
                                        <asp:TextBox ID="txtLoginName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-6 -->

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label">Domain</label>
                                        <asp:DropDownList ID="ddlLoginDomain" runat="server" CssClass="form-control mb15">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- col-sm-6 -->
                            </div>

                            <div class="row">

                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label">Password</label>
                                        <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-12 -->

                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label">Retype Password</label>
                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-12 -->
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label"></label>
                                        <asp:CheckBox ID="cbPasswordNeverExpires" runat="server" />
                                        Password never expires
                                    </div>
                                </div>
                            </div>
                            <!-- col-sm-12 -->
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">User Rights</h4>
                            </div>

                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbCompanyAdmin" runat="server" />
                                            <label for='<%= cbCompanyAdmin.ClientID %>'>Is Company Admin?</label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                    <div class="ckbox ckbox-primary">
                                        <asp:CheckBox ID="cbResellerAdmin" runat="server" />
                                        <label for='<%= cbResellerAdmin.ClientID %>'>Is Reseller Admin?</label>
                                    </div>
                                    </div>
                                </div>

                                <br />
                                <hr />
                                <br />

                                <div class="row" id="companyAdminRights" style="visibility: hidden">
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbEnableExchange" runat="server" />
                                            <label for='<%= cbEnableExchange.ClientID %>'>Enable Exchange</label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbAddDomain" runat="server" />
                                            <label for='<%= cbAddDomain.ClientID %>'>Add Domain</label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbEnableAcceptedDomain" runat="server" />
                                            <label for='<%= cbEnableAcceptedDomain.ClientID %>'>Enable Accepted Domain</label>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDisableExchange" runat="server" />
                                            <label for='<%= cbDisableExchange.ClientID %>'>Disable Exchange</label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDeleteDomain" runat="server" />
                                            <label for='<%= cbDeleteDomain.ClientID %>'>Delete Domain</label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDisableAcceptedDomain" runat="server" />
                                            <label for='<%= cbDisableAcceptedDomain.ClientID %>'>Disable Accepted Domain</label>
                                        </div>
                                    </div>
                                </div>

                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Exchange -->
            <div class="row">
                <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">E-Mail</h4>
                            </div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbEnableMailbox" runat="server" />
                                            <label for='<%= cbEnableMailbox.ClientID %>'>Enable a mailbox for this user?</label>
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <br />

                                <div class="row">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label class="control-label">E-mail Address</label>
                                            <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label class="control-label"></label>
                                            <asp:DropDownList ID="ddlAcceptedDomains" runat="server" CssClass="form-control mb15">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->
                                </div>

                                <br />
                                <br />

                                <div class="row">

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label class="control-label">Mailbox Plan</label>
                                            <asp:DropDownList ID="ddlMailboxPlan" runat="server" CssClass="form-control mb15">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                </div>

                                <br />
                                <br />

                                <div class="row">

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">Mailbox Size - <asp:Label ID="lbMailboxSizeInMB" runat="server" Text=""></asp:Label></label>
                                            <div id="mailbox-size-slider"></div>
                                            <asp:HiddenField ID="hfSelectedMailboxSize" runat="server" />
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                </div>
                            </div>
                        </div>
                    </div>
            </div>

            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText, Resellers_Cancel %>" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedText, Resellers_Submit %>" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
            <!-- panel-footer -->

        </asp:Panel>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/jquery-ui-1.10.3.min.js") %>'></script>
    <script type="text/javascript">

        jQuery(document).ready(function () {

            // Range Slider Maximum
            jQuery('#mailbox-size-slider').slider({
                range: 'max',
                min: 1024,
                max: 8192,
                value: 0,
                step: 256,
                slide: function (event, ui) {
                    $("#<%= lbMailboxSizeInMB.ClientID %>").text(ui.value + "MB (" + ui.value / 1024 + "GB)");
                }
            });

            $("#<%= cbCompanyAdmin.ClientID %>").change(function () {
                if (this.checked) {
                    $("#companyAdminRights").css('visibility', 'visible');
                } else {
                    $("#companyAdminRights").css('visibility', 'hidden');
                }
            });
        });

    </script>
</asp:Content>

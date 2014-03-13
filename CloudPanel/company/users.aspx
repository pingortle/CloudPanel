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
                <asp:Button ID="btnAddUser" runat="server" Text='<%$ Resources:LocalizedText, Users_AddNew %>' CssClass="btn btn-success" OnClick="btnAddUser_Click" />
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th><%= Resources.LocalizedText.Users_DisplayName %></th>
                                <th><%= Resources.LocalizedText.Users_LoginName %></th>
                                <th><%= Resources.LocalizedText.Users_SamAccountName %></th>
                                <th><%= Resources.LocalizedText.Users_Department %></th>
                                <th>&nbsp;</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeaterUsers" runat="server" OnItemCommand="repeaterUsers_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("DisplayName") %></td>
                                        <td><%# Eval("UserPrincipalName") %></td>
                                        <td><%# Eval("sAMAccountName") %></td>
                                        <td><%# Eval("Department") %></td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="" CssClass="fa fa-comment-o tooltips" data-placement="top" data-toggle="tooltip" title='<%$ Resources:LocalizedText, Users_IsLyncEnabled %>' Visible='<%# (int)Eval("LyncPlan") > 0 ? true : false %>'></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Text="" CssClass="fa fa-envelope-o tooltips" data-placement="top" data-toggle="tooltip" title='<%$ Resources:LocalizedText, Users_IsEmailEnabled %>' Visible='<%# (int)Eval("MailboxPlan") > 0 ? true : false %>'></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="" CssClass="fa fa-suitcase tooltips" data-placement="top" data-toggle="tooltip" title='<%$ Resources:LocalizedText, Users_ResellerAdmin %>' Visible='<%# (bool)Eval("IsResellerAdmin") %>'></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Text="" CssClass="fa fa-wrench tooltips" data-placement="top" data-toggle="tooltip" title='<%$ Resources:LocalizedText, Users_CompanyAdmin %>' Visible='<%# (bool)Eval("IsCompanyAdmin") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <div class="btn-group">
                                                <asp:Button ID="btnModify" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Edit" CommandArgument='<%# Eval("UserPrincipalName") %>' Text='<%$ Resources:LocalizedText, Users_Modify %>' />
                                                <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                                    <span class="caret"></span>
                                                    <span class="sr-only">Toggle Dropdown</span>
                                                </button>
                                                <ul class="dropdown-menu" role="menu">
                                                    <li>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserPrincipalName") %>'><%= Resources.LocalizedText.Users_Delete %></asp:LinkButton>
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
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_FirstName %><span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="hfEditUserPrincipalName" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_MiddleName %></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_LastName %></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtLastname" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_DisplayName %><span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_Department %></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control"></asp:TextBox>
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
                            <h4 class="panel-title"><%= Resources.LocalizedText.Users_LoginDetails %></h4>
                        </div>

                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_LoginName %></label>
                                        <asp:TextBox ID="txtLoginName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-6 -->

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_Domain %></label>
                                        <asp:DropDownList ID="ddlLoginDomain" runat="server" CssClass="form-control mb15" DataTextField="DomainName" DataValueField="DomainID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- col-sm-6 -->
                            </div>

                            <div class="row">

                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_Password %></label>
                                        <asp:TextBox ID="txtPassword1" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-12 -->

                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_RetypePassword %></label>
                                        <asp:TextBox ID="txtPassword2" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-12 -->
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbPasswordNeverExpires" runat="server" />
                                            <label for='<%= cbPasswordNeverExpires.ClientID %>'><%= Resources.LocalizedText.Users_PasswordNeverExpires %></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- col-sm-12 -->
                        </div>
                    </div>
                </div>

                <asp:Panel ID="panelUserRights" runat="server" CssClass="col-md-6">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title"><%= Resources.LocalizedText.Users_UserRights %></h4>
                            </div>

                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbCompanyAdmin" runat="server" />
                                            <label for='<%= cbCompanyAdmin.ClientID %>'><%= Resources.LocalizedText.Users_IsCompanyAdmin %></label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbResellerAdmin" runat="server" />
                                            <label for='<%= cbResellerAdmin.ClientID %>'><%= Resources.LocalizedText.Users_IsResellerAdmin %></label>
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
                                            <label for='<%= cbEnableExchange.ClientID %>'><%= Resources.LocalizedText.Users_EnableExchange %></label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbAddDomain" runat="server" />
                                            <label for='<%= cbAddDomain.ClientID %>'><%= Resources.LocalizedText.Users_AddDomain %></label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbEnableAcceptedDomain" runat="server" />
                                            <label for='<%= cbEnableAcceptedDomain.ClientID %>'><%= Resources.LocalizedText.Users_EnableAcceptedDomains %></label>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDisableExchange" runat="server" />
                                            <label for='<%= cbDisableExchange.ClientID %>'><%= Resources.LocalizedText.Users_DisableExchange %></label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDeleteDomain" runat="server" />
                                            <label for='<%= cbDeleteDomain.ClientID %>'><%= Resources.LocalizedText.Users_DeleteDomain %></label>
                                        </div>

                                        <div class="ckbox ckbox-success">
                                            <asp:CheckBox ID="cbDisableAcceptedDomain" runat="server" />
                                            <label for='<%= cbDisableAcceptedDomain.ClientID %>'><%= Resources.LocalizedText.Users_DisableAcceptedDomains %></label>
                                        </div>
                                    </div>
                                </div>

                                <br />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <!-- Exchange -->
            <asp:Panel ID="panelEmail" runat="server" CssClass="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title"><%= Resources.LocalizedText.Users_Email %></h4>
                            </div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbEnableMailbox" runat="server" />
                                            <label for='<%= cbEnableMailbox.ClientID %>'><%= Resources.LocalizedText.Users_EnableMailbox %></label>
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <br />

                                <div class="row">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label class="control-label"><%= Resources.LocalizedText.Users_EmailAddress %></label>
                                            <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label class="control-label"></label>
                                            <asp:DropDownList ID="ddlAcceptedDomains" runat="server" CssClass="form-control mb15" DataTextField="DomainName" DataValueField="DomainID">
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
                                            <label class="control-label"><%= Resources.LocalizedText.Users_MailboxPlan %></label>
                                            <asp:DropDownList ID="ddlMailboxPlan" runat="server" CssClass="form-control mb15">
                                            </asp:DropDownList>
                                            <span id="PlanDescription"> </span>
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                </div>

                                <br />
                                <br />

                                <div class="row">

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label"><%= Resources.LocalizedText.Users_MailboxSize %>:&nbsp;
                                                <asp:Label ID="lbMailboxSizeInMB" runat="server" Text=""></asp:Label>
                                            </label>
                                            <div id="mailbox-size-slider"></div>
                                            <asp:HiddenField ID="hfSelectedMailboxSize" runat="server" />
                                        </div>
                                    </div>
                                    <!-- col-sm-3 -->

                                </div>
                            </div>
                        </div>
                    </div>
            </asp:Panel>

            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText, Users_Cancel %>" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedText, Users_Submit %>" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
            <!-- panel-footer -->

        </asp:Panel>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <% if (!panelUserList.Visible) { %>
        <!-- Including this file while the user list if visible will mess up the tooltips for the icons that the user has -->
        <script src='<%= this.ResolveClientUrl("~/js/jquery-ui-1.10.3.min.js") %>'></script>
    <% } %>

    <script type="text/javascript">

        var selected = "";

        jQuery(document).ready(function () {

            if ($("#<%= cbCompanyAdmin.ClientID %>").is(':checked'))
                $("#companyAdminRights").css('visibility', 'visible');
            else
                $("#companyAdminRights").css('visibility', 'hidden');

            $("#<%= cbCompanyAdmin.ClientID %>").change(function () {
                if (this.checked) {
                    $("#companyAdminRights").css('visibility', 'visible');
                } else {
                    $("#companyAdminRights").css('visibility', 'hidden');
                }
            });

            <% if (panelEmail.Visible)
               { %>

                $("#<%= ddlMailboxPlan.ClientID %>").change(function () {
                    $("#<%= ddlMailboxPlan.ClientID %> option:selected").each(function () {
                        Calculate($(this).attr("Description"), $(this).attr("Price"), $(this).attr("Extra"), $(this).attr("Min"), $(this).attr("Max"));
                    });
                });

                $loadSelected = $("#<%= ddlMailboxPlan.ClientID %> option:selected");
                Calculate($loadSelected.attr("Description"), $loadSelected.attr("Price"), $loadSelected.attr("Extra"), $loadSelected.attr("Min"), $loadSelected.attr("Max"));
            <% } %>
        });

        function Calculate(description, price, extra, min, max) {

            selected = description;

            var price = parseFloat(price);
            var extra = parseFloat(extra);
            var minRange = parseInt(min);
            var maxRange = parseInt(max);

            // Store original value in hidden field for post back
            $("#<%= hfSelectedMailboxSize.ClientID %>").val(minRange);

            jQuery('#mailbox-size-slider').slider({
                range: 'max',
                min: minRange,
                max: maxRange,
                value: minRange,
                step: 256,
                slide: function (event, ui) {
                    $("#<%= lbMailboxSizeInMB.ClientID %>").text(ui.value / 1024 + "GB");
                    
                    $("#<%= hfSelectedMailboxSize.ClientID %>").val(ui.value);
                }


            });

            $("#<%= lbMailboxSizeInMB.ClientID %>").text( (minRange / 1024).toString() + "GB");
            $("#PlanDescription").text(selected);
        }

    </script>
</asp:Content>

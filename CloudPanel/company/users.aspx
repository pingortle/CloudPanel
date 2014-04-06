<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="CloudPanel.company.users" EnableEventValidation="false"%>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">

    <div class="pageheader">
        <h2><i class="fa fa-users"></i>Users</h2>
    </div>

    <div class="contentpanel">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
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
                                            <asp:Label ID="Label5" runat="server" Text="" CssClass="fa fa-warning tooltips" data-placement="top" data-toggle="tooltip" title='<%$ Resources:LocalizedText, Users_IsDisabled %>' Visible='<%# !(bool)Eval("IsEnabled") %>'></asp:Label>
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
                                                        <a href="#" data-toggle="modal" data-target=".resetpwd-modal" onclick="SetResetPwd('<%#Eval("UserPrincipalName") %>')">Reset Password</a>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserPrincipalName") %>' OnClientClick="return DeleteConfirm();"><%= Resources.LocalizedText.Users_Delete %></asp:LinkButton>
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

            
            <div class="modal fade resetpwd-modal" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" style="display: none;">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
                            <h4 class="modal-title"><%= Resources.LocalizedText.Users_ResetPassword %></h4>
                            <p><asp:Label ID="lbResetPasswordUPN" runat="server" Text=""></asp:Label></p>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_Password %></label>
                                        <asp:TextBox ID="txtResetPwd1" runat="server" CssClass="form-control" TextMode="Password" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hfResetPwdHiddenField" runat="server" />
                                    </div>
                                </div>
                                <!-- col-sm-12 -->

                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="control-label"><%= Resources.LocalizedText.Users_RetypePassword %></label>
                                        <asp:TextBox ID="txtResetPwd2" runat="server" CssClass="form-control" TextMode="Password" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- col-sm-12 -->
                            </div>

                            <asp:Button ID="btnResetPwd" runat="server" Text='<%$ Resources:LocalizedText, Users_ResetPassword %>' CssClass="btn btn-primary btn-block mt10" OnClick="btnResetPwd_Click"/>
                        </div>
                    </div>
                </div>
            </div>
            <!-- RESET PASSWORD MODAL -->

        </asp:Panel>

        <asp:Panel ID="panelCreateUser" runat="server" Visible="false">

            <!-- User Information -->
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title"><%= Resources.LocalizedText.Users_UserInformation %></h4>
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_FirstName %><span class="asterisk">*</span></label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
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
            
            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText, Users_Cancel %>" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedText, Users_Submit %>" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
            <!-- panel-footer -->

        </asp:Panel>

        <asp:Panel ID="panelEditUser" runat="server" Visible="false">

            <div id="errorContainer" class="error"></div>

            <div class="row">
                <div class="col-sm-10">
                    <div class="profile-header">
                        <h2 class="profile-name"><asp:Label ID="lbProfileDisplayName" runat="server" Text=""></asp:Label></h2>
                        <div class="profile-position"><i class="fa fa-lock"></i><asp:Label ID="lbProfileUsername" runat="server" Text=""></asp:Label></div>
                        <div class="profile-position"><i class="fa fa-lock"></i><asp:Label ID="lbProfileSamAccountName" runat="server" Text=""></asp:Label></div>
                        <div class="mb20"></div>
                    </div>
                    <!-- profile-header -->
                </div>
                <div class="col-sm-2">
                    <asp:Image ID="imgUserPhoto" runat="server" ImageUrl="~/images/photos/unknown-user.png" CssClass="thumbnail img-responsive pull-right" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#User" data-toggle="tab"><strong><%= Resources.LocalizedText.Users_User %></strong></a></li>
                        <% if (_isExchangeEnabled != null && _isExchangeEnabled)
                           { %>
                        <li class="dropdown">
                            <a href="#" data-toggle="dropdown" class="dropdown-toggle"><%= Resources.LocalizedText.Users_MailboxSettings %><span class="caret"></span>
                            </a>
                            <ul role="menu" class="dropdown-menu pull-left">
                                <li><a href="#Mailbox" data-toggle="tab"><%= Resources.LocalizedText.Users_MailboxSettings %></a></li>
                                <li><a href="#Archiving" data-toggle="tab"><%= Resources.LocalizedText.Users_Archiving %></a></li>
                                <li><a href="#LitigationHold" data-toggle="tab"><%= Resources.LocalizedText.Users_LitigationHold %></a></li>
                            </ul>
                        </li>
                        <% } %>
                    </ul>

                    <div class="tab-content" id="validateForm">

                        <!-- User -->
                        <div class="tab-pane active" id="User">
                            <div class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <p class="lead"><%= Resources.LocalizedText.Users_General %></p>

                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_FirstName %><span class="asterisk">*</span></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:HiddenField ID="hfEditUserPrincipalName" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_MiddleName %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditMiddleName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_LastName %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditLastname" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_DisplayName %><span class="asterisk">*</span></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_Department %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditDepartment" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <hr />
                                        <p class="lead"><%= Resources.LocalizedText.Users_UserStatus %></p>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditEnableUser" runat="server" />
                                                    <label for='<%= cbEditEnableUser.ClientID %>'><%= Resources.LocalizedText.Users_EnableUser %></label>
                                                </div>
                                            </div>
                                        </div>

                                        <% if (CloudPanel.WebSessionHandler.IsSuperAdmin || CloudPanel.WebSessionHandler.IsResellerAdmin) {  %>
                                        <hr />
                                        <p class="lead"><%= Resources.LocalizedText.Users_Permissions %></p>
                                        <% if (CloudPanel.WebSessionHandler.IsSuperAdmin || CloudPanel.WebSessionHandler.IsResellerAdmin)
                                           { %>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditIsResellerAdmin" runat="server" />
                                                    <label for='<%= cbEditIsResellerAdmin.ClientID %>'><%= Resources.LocalizedText.Users_IsResellerAdmin %></label>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-2">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditIsCompanyAdmin" runat="server" />
                                                    <label for='<%= cbEditIsCompanyAdmin.ClientID %>'><%= Resources.LocalizedText.Users_IsCompanyAdmin %></label>
                                                </div>
                                            </div>
                                            <div class="row" id="editCompanyAdminRights" style="visibility: hidden">
                                                <div class="col-md-2">
                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditEnableExchange" runat="server" />
                                                        <label for='<%= cbEditEnableExchange.ClientID %>'><%= Resources.LocalizedText.Users_EnableExchange %></label>
                                                    </div>

                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditAddDomain" runat="server" />
                                                        <label for='<%= cbEditAddDomain.ClientID %>'><%= Resources.LocalizedText.Users_AddDomain %></label>
                                                    </div>

                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditEnableAcceptedDomain" runat="server" />
                                                        <label for='<%= cbEditEnableAcceptedDomain.ClientID %>'><%= Resources.LocalizedText.Users_EnableAcceptedDomains %></label>
                                                    </div>
                                                </div>

                                                <div class="col-md-2">
                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditDisableExchange" runat="server" />
                                                        <label for='<%= cbEditDisableExchange.ClientID %>'><%= Resources.LocalizedText.Users_DisableExchange %></label>
                                                    </div>

                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditDeleteDomain" runat="server" />
                                                        <label for='<%= cbEditDeleteDomain.ClientID %>'><%= Resources.LocalizedText.Users_DeleteDomain %></label>
                                                    </div>

                                                    <div class="ckbox ckbox-success">
                                                        <asp:CheckBox ID="cbEditDisableAcceptedDomain" runat="server" />
                                                        <label for='<%= cbEditDisableAcceptedDomain.ClientID %>'><%= Resources.LocalizedText.Users_DisableAcceptedDomains %></label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                         
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Mailbox -->
                        <% if (_isExchangeEnabled != null && _isExchangeEnabled) { %>

                        <div class="tab-pane" id="Mailbox">
                            <div class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <p class="lead"><%= Resources.LocalizedText.Users_General %></p>

                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditIsMailboxEnabled" runat="server" />
                                                    <label for='<%= cbEditIsMailboxEnabled.ClientID %>'><%= Resources.LocalizedText.Users_EnableMailbox %></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_Email %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditMailboxEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxDomain" runat="server" CssClass="form-control chosen-select" DataTextField="DomainName" DataValueField="DomainID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_ActiveSyncPlan %></label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxASPlan" runat="server" CssClass="form-control chosen-select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_MailboxPlan %></label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxPlan" runat="server" CssClass="form-control chosen-select" DataTextField="MailboxPlanName" DataValueField="MailboxPlanID">
                                                </asp:DropDownList>

                                                <%= Resources.LocalizedText.Users_Description %>: <span id="EditPlanDescription"></span>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_MailboxSize %> </label>
                                            <div class="col-sm-4">
                                                <div id="edit-mailbox-size-slider"></div>
                                                <asp:HiddenField ID="hfEditSelectedMailboxSize" runat="server" />
                                            </div>
                                            <div class="col-sm-2 control-label" style="text-align: left">
                                                <asp:Label ID="lbEditMailboxSizeInMB" runat="server" Text=""></asp:Label>
                                            </div>                                            
                                        </div>
                                        
                                        <hr />
                                        <p class="lead"><%= Resources.LocalizedText.Users_EmailAliases %></p>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-8">
                                                <asp:UpdatePanel ID="updatePanelEmailAlias" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gridEmailAliases" runat="server" OnPreRender="gridEmailAliases_PreRender" CssClass="table table-bordered table-striped"
                                                            OnRowCommand="gridEmailAliases_RowCommand" AutoGenerateColumns="false" OnRowDeleting="gridEmailAliases_RowDeleting">
                                                            <Columns>
                                                                <asp:BoundField HeaderText='<%$ Resources:LocalizedText, Users_Alias %>' DataField="Email" />
                                                                <asp:CommandField DeleteImageUrl="~/images/minus@2x.png" ButtonType="Image" ShowDeleteButton="true" HeaderStyle-Width="32px" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                        <asp:Label ID="lbAddAliasError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnEditInsertEmailAlias" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtEditAddEmailAlias" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlEditAddEmailAliasDomain" runat="server"  DataTextField="DomainName" DataValueField="DomainID" CssClass="form-control chosen-select"></asp:DropDownList> 
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Button ID="btnEditInsertEmailAlias" runat="server" Text='<%$ Resources:LocalizedText, Buttons_Add %>' CssClass="btn btn-info" OnClick="btnEditInsertEmailAlias_Click" />
                                            </div>
                                        </div>

                                        <hr />
                                        <p class="lead"><%= Resources.LocalizedText.Users_Forwarding %></p>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_ForwardTo %></label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxForwardTo" runat="server" CssClass="form-control chosen-select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">&nbsp;</label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditMailboxForwardBoth" runat="server" />
                                                    <label for='<%= cbEditMailboxForwardBoth.ClientID %>'><%= Resources.LocalizedText.Users_DeliverAndForward %></label>
                                                </div>
                                            </div>
                                        </div>

                                        <hr />
                                        <p class="lead"><%= Resources.LocalizedText.Users_MailboxPermissions %></p>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_FullAccess %></label>
                                            <div class="col-sm-10">
                                                <asp:ListBox ID="ddlEditMailboxFullAccess" runat="server" CssClass="chosen-select" multiple DataTextField="DisplayName" DataValueField="sAMAccountName" Autocomplete="off" SelectionMode="Multiple"></asp:ListBox>
                                                <asp:HiddenField ID="hfEditMailboxFullAccessOriginal" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_SendAs %></label>
                                            <div class="col-sm-10">
                                                <asp:ListBox ID="ddlEditMailboxSendAs" runat="server" CssClass="chosen-select" multiple DataTextField="DisplayName" DataValueField="sAMAccountName" Autocomplete="off" SelectionMode="Multiple"></asp:ListBox>
                                                <asp:HiddenField ID="hfEditMailboxSendAsOriginal" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_SendOnBehalf %></label>
                                            <div class="col-sm-10">
                                                <asp:ListBox ID="ddlEditMailboxSendOnBehalf" runat="server" CssClass="chosen-select" multiple DataTextField="DisplayName" DataValueField="sAMAccountName" Autocomplete="off" SelectionMode="Multiple"></asp:ListBox>
                                                <asp:HiddenField ID="hfEditMailboxSendOnBehalfOriginal" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="tab-pane" id="Archiving">
                            <div class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"></label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditMailboxEnableArchiving" runat="server" />
                                                    <label for='<%= cbEditMailboxEnableArchiving.ClientID %>'><%= Resources.LocalizedText.Users_EnableArchiving %></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_ArchiveName %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditMailboxArchiveName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_ArchivePlan %></label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxArchivePlan" runat="server" CssClass="form-control chosen-select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_ArchiveDatabase %></label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlEditMailboxArchiveDatabase" runat="server" CssClass="form-control chosen-select">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="tab-pane" id="LitigationHold">
                            <div class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"></label>
                                            <div class="col-sm-4">
                                                <div class="ckbox ckbox-primary">
                                                    <asp:CheckBox ID="cbEditMailboxEnableLitigationHold" runat="server" />
                                                    <label for='<%= cbEditMailboxEnableLitigationHold.ClientID %>'><%= Resources.LocalizedText.Users_EnableLitigationHold %></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_Duration %></label>
                                            <div class="col-sm-4">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtEditMailboxLitigationHoldDuration" runat="server" CssClass="form-control" placeholder="mm/dd/yyyy"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_DescriptionURL %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditMailboxLitigationHoldURL" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Users_Comments %></label>
                                            <div class="col-sm-4">
                                                <asp:TextBox ID="txtEditMailboxLitigationHoldComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <% } %>

                    </div>
                </div>
            </div>

            <br />
            <br />

            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnEditCancel" runat="server" Text="<%$ Resources:LocalizedText, Users_Cancel %>" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                <asp:Button ID="btnEditSave" runat="server" Text="<%$ Resources:LocalizedText, Users_Submit %>" CssClass="btn btn-primary" OnClick="btnEditSave_Click" />
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
        var currentSize = "<%= _currentMailboxSize %>";

        function DeleteConfirm() {
            return confirm('<%= Resources.LocalizedText.Global_ConfirmDeletePlan %>');
        }

        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });

            // Date Picker
            jQuery('#<%= txtEditMailboxLitigationHoldDuration.ClientID %>').datepicker();

            // New User permissions DIV
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

            // Edit user permissions DIV
            if ($("#<%= cbEditIsCompanyAdmin.ClientID %>").is(':checked'))
                $("#editCompanyAdminRights").css('visibility', 'visible');
            else
                $("#editCompanyAdminRights").css('visibility', 'hidden');

            $("#<%= cbEditIsCompanyAdmin.ClientID %>").change(function () {
                if (this.checked) {
                    $("#editCompanyAdminRights").css('visibility', 'visible');
                } else {
                    $("#editCompanyAdminRights").css('visibility', 'hidden');
                }
            });

            $("#<%= ddlEditMailboxPlan.ClientID %>").change(function () {
                $("#<%= ddlEditMailboxPlan.ClientID %> option:selected").each(function () {
                    Calculate($(this).attr("Description"), $(this).attr("Price"), $(this).attr("Extra"), $(this).attr("Min"), $(this).attr("Max"));
                });
            });

            $loadSelected = $("#<%= ddlEditMailboxPlan.ClientID %> option:selected");
            Calculate($loadSelected.attr("Description"), $loadSelected.attr("Price"), $loadSelected.attr("Extra"), $loadSelected.attr("Min"), $loadSelected.attr("Max"));
        });

        function Calculate(description, price, extra, min, max) {

            selected = description;

            var price = parseFloat(price);
            var extra = parseFloat(extra);
            var minRange = parseInt(min);
            var maxRange = parseInt(max);

            if (currentSize == null || currentSize <= 0)
                currentSize = minRange;

            // Store original value in hidden field for post back
            $("#<%= hfEditSelectedMailboxSize.ClientID %>").val(currentSize);

            jQuery('#edit-mailbox-size-slider').slider({
                range: 'max',
                min: minRange,
                max: maxRange,
                value: currentSize,
                step: 256,
                slide: function (event, ui) {
                    $("#<%= lbEditMailboxSizeInMB.ClientID %>").text(ui.value / 1024 + "GB");
                    
                    $("#<%= hfEditSelectedMailboxSize.ClientID %>").val(ui.value);
                }
            });

            $("#<%= lbEditMailboxSizeInMB.ClientID %>").text( (currentSize / 1024).toString() + "GB");
            $("#EditPlanDescription").text(selected);
        }

        function SetResetPwd(upn)
        {
            $("#<%=hfResetPwdHiddenField.ClientID %>").val(upn);
            $("#<%= lbResetPasswordUPN.ClientID %>").html(upn);
        }

    </script>

</asp:Content>

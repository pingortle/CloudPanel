<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="CloudPanel.settings" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-gear"></i><%= Resources.LocalizedText.Settings_Settings %></h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" id="alertmessage" />

        <div id="errorContainer" class="error">
            
        </div>

        <div class="row">
            <div class="col-md-12">

                <ul class="nav nav-tabs nav-justified">
                    <li class="active"><a href="#general" data-toggle="tab"><strong>General</strong></a></li>
                    <li><a href="#ActiveDirectory" data-toggle="tab"><strong>Active Directory</strong></a></li>
                    <li><a href="#SecurityGroups" data-toggle="tab"><strong>Security Groups</strong></a></li>
                    <li><a href="#Billing" data-toggle="tab"><strong>Billing</strong></a></li>
                    <li><a href="#Exchange" data-toggle="tab"><strong>Exchange</strong></a></li>
                    <li><a href="#Modules" data-toggle="tab"><strong>Modules</strong></a></li>
                    <li><a href="#Notifications" data-toggle="tab"><strong>Notifications</strong></a></li>
                    <li><a href="#Other" data-toggle="tab"><strong>Other</strong></a></li>
                </ul>

                <div class="tab-content" id="validateForm">

                    <!-- General -->
                    <div class="tab-pane active" id="general">
                        <div class="form-horizontal">
                            <div class="panel panel-default">

                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Company Name <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableResellers" runat="server" />
                                                <label for='<%= cbEnableResellers.ClientID %>'>Enable Reseller Accounts</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- Active Directory -->
                    <div class="tab-pane" id="ActiveDirectory">
                        <div class="form-horizontal">
                            <div class="panel panel-default">

                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Hosting OU <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtHostingOU" runat="server" CssClass="form-control" placeholder="Example: OU=Hosting,DC=domain,DC=local"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Users OU</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtUsersOU" runat="server" CssClass="form-control" placeholder="Example: Users"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Domain Controller <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtDomainController" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Username <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Example: DOMAIN\Administrator"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Password <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- Security Groups -->
                    <div class="tab-pane" id="SecurityGroups">
                        <div class="form-horizontal">
                            <div class="panel panel-default">

                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Super Admins <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtSuperAdmins" runat="server" CssClass="form-control" placeholder="Example: Domain Admins,Enterprise Admins"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Billing Admins </label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtBillingAdmins" runat="server" CssClass="form-control" placeholder="Example: Domain Admins,Enterprise Admins"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- Billing -->
                    <div class="tab-pane" id="Billing">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Default Currency <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlCurrencySymbol" runat="server" CssClass="form-control mb15" >
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Exchange -->
                    <div class="tab-pane" id="Exchange">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Connection Type <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlExchConnectionType" runat="server" CssClass="form-control mb15">
                                                <asp:ListItem Text="Basic Authentication" Value="Basic"></asp:ListItem>
                                                <asp:ListItem Text="Kerberos Authentcation" Value="Kerberos"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Version<span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlExchVersion" runat="server" CssClass="form-control mb15">
                                                <asp:ListItem Text="Exchange 2010 SP2 or Later" Value="2010"></asp:ListItem>
                                                <asp:ListItem Text="Exchange 2013 CU1 or later" Value="2013"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Server FQDN <span class="asterisk">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtExchServer" runat="server" CssClass="form-control" placeholder="cas01.domain.local"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Public Folder Server</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtExchPFServer" runat="server" CssClass="form-control" placeholder="mbx01.domain.local"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Databases</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtExchDatabases" runat="server" CssClass="form-control" placeholder="DB01,DB02,CustomerADB"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbExchPFEnabled" runat="server" />
                                                <label for='<%= cbExchPFEnabled.ClientID %>'>Enable Public Folders?</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbExchSSLEnabled" runat="server" />
                                                <label for='<%= cbExchSSLEnabled.ClientID %>'>Exchange SSL Enabled?</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Modules -->
                    <div class="tab-pane" id="Modules">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableExchange" runat="server" />
                                                <label for='<%= cbEnableExchange.ClientID %>'>Enable Exchange Hosting</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableCitrix" runat="server" />
                                                <label for='<%= cbEnableCitrix.ClientID %>'>Enable Citrix Hosting</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableLync" runat="server" Enabled="false" />
                                                <label for='<%= cbEnableLync.ClientID %>'>Enable Lync Hosting</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableWebsite" runat="server" />
                                                <label for='<%= cbEnableWebsite.ClientID %>'>Enable Website Hosting</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-success">
                                                <asp:CheckBox ID="cbEnableSharepoint" runat="server" />
                                                <label for='<%= cbEnableSharepoint.ClientID %>'>Enable Sharepoint Hosting</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-top: 25px;">
            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedText, Resellers_Submit %>" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.validate.min.js") %>'></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });

            // Validation
            jQuery("#form1").validate({
                ignore: "",
                rules: {
                    <%= txtCompanyName.UniqueID %>: { required: true },
                    <%= txtHostingOU.UniqueID %>: { required: true },
                    <%= txtDomainController.UniqueID %>: { required: true },
                    <%= txtUsername.UniqueID %>: { required: true },
                    <%= txtPassword.UniqueID %>: { required: true },
                    <%= txtSuperAdmins.UniqueID %>: { required: true },
                    <%= txtBillingAdmins.UniqueID %>: { required: true },
                    <%= txtExchServer.UniqueID %>: { required: true }
                },
                messages: {
                    <%= txtCompanyName.UniqueID %>: '[General] Your company name is required!',
                    <%= txtHostingOU.UniqueID %>: '[Active Directory] You must provide the organizational unit you want to place resellers/companies in!',
                    <%= txtDomainController.UniqueID %>: '[Active Directory] You must provide the domain controller for CloudPanel to communicate with!',
                    <%= txtUsername.UniqueID %>: '[Active Directory] You must provide the DOMAIN\Username of the account to use when CloudPanel communicates with your environment!',
                    <%= txtPassword.UniqueID %>: '[Active Directory] You must provide the password!',
                    <%= txtSuperAdmins.UniqueID %>: '[Security Groups] You must provide a comma seperated value for the Super Administrators!',
                    <%= txtBillingAdmins.UniqueID %>: '[Security Groups] You must provide a commas seperated value for the Billing Administrators!',
                    <%= txtExchServer.UniqueID %>: '[Exchange] You must provide the FQDN of the Exchange server!'
                },
                errorLabelContainer: jQuery("#errorContainer")
            });

        });
    </script>
</asp:Content>

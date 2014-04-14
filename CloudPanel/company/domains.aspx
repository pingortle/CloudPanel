<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="domains.aspx.cs" Inherits="CloudPanel.company.domains" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-globe"></i>Company Domains</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelDomainList" runat="server" CssClass="row">

            <div style="float: right; margin: 10px">
                <asp:Button ID="btnAddDomain" runat="server" Text='<%$ Resources:LocalizedText, Domains_AddNewDomain %>' CssClass="btn btn-success" OnClick="btnAddDomain_Click" />
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <asp:Repeater ID="repeaterDomains" runat="server" OnItemCommand="repeaterDomains_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped mb30">
                                <thead>
                                    <tr>
                                        <th><%= Resources.LocalizedText.Domains_DomainName %></th>
                                        <th><%= Resources.LocalizedText.Domains_IsDefault %></th>
                                        <th><%= Resources.LocalizedText.Domains_DomainType %></th>
                                        <th style="width: 10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("DomainName") %></td>
                                <td><%# ((bool)Eval("IsDefault")).ToString() %></td>
                                <td><%# Eval("TypeOfDomain").ToString() %></td>
                                <td>
                                    <div class="btn-group">
                                        <asp:Button ID="btnModify" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Edit" CommandArgument='<%# Eval("DomainID") %>' Text='<%$ Resources:LocalizedText, Buttons_Modify %>' />
                                        <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("DomainName") %>' OnClientClick="return DeleteConfirm();"><%= Resources.LocalizedText.Buttons_Delete %></asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- btn-group -->
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                    </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <!-- table-responsive -->
            </div>
            <!-- col-md-6 -->
        </asp:Panel>

        <asp:Panel ID="panelEditCreateDomain" runat="server" CssClass="row" Visible="false">

            <!-- Domain -->
            <div class="form-horizontal">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.Domains_AddEditDomain %></h4>
                    </div>

                    <div class="panel-body">

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.Domains_DomainName %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtDomainName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hfDomainID" runat="server" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"></label>
                            <div class="col-sm-4">
                                <div class="ckbox ckbox-primary">
                                    <asp:CheckBox ID="cbIsDefaultDomain" runat="server" />
                                    <label for='<%= cbIsDefaultDomain.ClientID %>'><%= Resources.LocalizedText.Domains_IsDefault %></label>
                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->
                </div>
            </div>

            <!-- Exchange Domain -->
            <div class="form-horizontal" id="divExchangeDomain" runat="server">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.Domains_ExchangeDomain %></h4>
                    </div>

                    <div class="panel-body">

                        <div class="form-group">
                            <label class="col-sm-2 control-label"></label>
                            <div class="col-sm-4">
                                <div class="ckbox ckbox-primary">
                                    <asp:CheckBox ID="cbIsAcceptedDomain" runat="server" />
                                    <label for='<%= cbIsAcceptedDomain.ClientID %>'><%= Resources.LocalizedText.Domains_EnableAcceptedDomain %></label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"></label>
                            <div class="col-sm-4">
                                <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbAuthoritative" runat="server" GroupName="DomainType" Checked="true" />
                                        <label for="<%= cbAuthoritative.ClientID %>"><%= Resources.LocalizedText.Domains_Authoritative %></label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbInternalRelay" runat="server" GroupName="DomainType" />
                                        <label for="<%= cbInternalRelay.ClientID %>"><%= Resources.LocalizedText.Domains_InternalRelay %></label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbExternalRelay" runat="server" GroupName="DomainType" />
                                        <label for="<%= cbExternalRelay.ClientID %>"><%= Resources.LocalizedText.Domains_ExternalRelay %></label>
                                    </div>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->
                </div>
            </div>

            <br />
            
            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnCancel" runat="server" Text='<%$ Resources:LocalizedText, buttons_Cancel %>' CssClass="btn btn-default" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text='<%$ Resources:LocalizedText, buttons_Submit %>' CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
            <!-- panel-footer -->
        </asp:Panel>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/jquery.validate.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.validate.valDomain.js") %>'></script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            
            $("#<%= btnSubmit.ClientID %>").click(function() {
                $("#form1").validate({
                    rules: {
                        <%= txtDomainName.UniqueID %>: { valDomain: true, required: true }
                    },
                    errorPlacement: function() { return false; },
                    highlight: function (element, errorClass, validClass) {
                        $(element).parents('.form-group').removeClass('has-success');
                        $(element).parents('.form-group').addClass('has-error');
                    },
                    unhighlight: function (element, errorClass, validClass) {
                        $(element).parents('.form-group').removeClass('has-error');
                        $(element).parents('.form-group').addClass('has-success');
                    }
                });
            });
        });

        function DeleteConfirm() {
            return confirm('<%= Resources.LocalizedText.Global_ConfirmDelete %>');
        }
    </script>
</asp:Content>

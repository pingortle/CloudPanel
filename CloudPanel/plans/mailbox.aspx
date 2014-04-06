<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="mailbox.aspx.cs" Inherits="CloudPanel.plans.mailbox" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href='<%= this.ResolveClientUrl("~/css/toggles.css") %>' />
    <style type="text/css">
        .popovers
        {
            cursor: pointer;
        }
        .control-label
        {
            text-align: right;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-envelope"></i>Mailbox Plans</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelPlan" runat="server" CssClass="row">

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.PlanMailbox_PlanSettings %></h4>
                    </div>

                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_Plan %></label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlMailboxPlans" runat="server" CssClass="form-control chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlMailboxPlans_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="Label8" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_PlanInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_SpecificCompany %></label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCompanies" runat="server" CssClass="form-control chosen-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="Label7" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_CompanyCodeInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_DisplayName %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="Label9" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_DisplayNameInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_Description %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" required></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="Label10" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_DescriptionInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_MaxRecipients %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxRecipients" runat="server" CssClass="form-control spinner" required></asp:TextBox>
                                <asp:Label ID="Label5" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_MaxRecipientsInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_KeepDeletedItems %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxKeepDeletedItemsInDays" runat="server" CssClass="form-control spinner" required></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_KeepDeletedItemsInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_MailboxSize %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMinMailboxSize" runat="server" CssClass="form-control spinner-256step" required></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_MinMailboxSizeInfo %>"></asp:Label>
                            </div>
                        </div>

                         <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_MaxMailboxSize %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxMailboxSize" runat="server" CssClass="form-control spinner-256step" required></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_MaxMailboxSizeInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_MaxSendSize %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxSendSize" runat="server" CssClass="form-control spinner" required></asp:TextBox>
                                <asp:Label ID="Label4" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_MaxSendSizeInfo %>"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_MaxReceiveSize %> <span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxReceiveSize" runat="server" CssClass="form-control spinner" required></asp:TextBox>
                                <asp:Label ID="Label6" runat="server" Text="" CssClass="fa fa-question-circle popovers" data-placement="right" data-container="body" data-content="<%$ Resources:LocalizedText, PlanMailbox_MaxReceiveSizeInfo %>"></asp:Label>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->

                </div>
                <!-- panel-primary -->

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.PlanMailbox_Features %></h4>
                    </div>

                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnablePOP3 %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary pop3"></div>
                                <asp:CheckBox ID="cbEnablePOP3" runat="server" style="display: none" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnableIMAP %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary imap"></div>
                                <asp:CheckBox ID="cbEnableIMAP" runat="server" style="display: none" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnableOWA %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary owa"></div>
                                <asp:CheckBox ID="cbEnableOWA" runat="server" style="display: none" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnableOutlook %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary outlook"></div>
                                <asp:CheckBox ID="cbEnableMAPI" runat="server" style="display: none" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnableActivesync %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary activesync"></div>
                                <asp:CheckBox ID="cbEnableAS" runat="server" style="display: none" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_EnableECP %></label>
                            <div class="col-sm-4 control-label">
                                <div class="toggle toggle-primary ecp"></div>
                                <asp:CheckBox ID="cbEnableECP" runat="server" style="display: none" />
                            </div>
                        </div>
                    </div>
                    <!-- panel-body -->

                </div>
                <!-- panel-primary -->

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.PlanMailbox_Pricing %></h4>
                    </div>

                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_CostForHoster %></label>
                            <div class="col-sm-4 control-label">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon"><%= CloudPanel.Modules.Common.Settings.StaticSettings.CurrencySymbol %></span>
                                    <asp:TextBox ID="txtCostPerMailbox" runat="server" placeholder="0.00" CssClass="form-control maskMoney" required></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_PriceForCompany %></label>
                            <div class="col-sm-4 control-label">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon"><%= CloudPanel.Modules.Common.Settings.StaticSettings.CurrencySymbol %></span>
                                    <asp:TextBox ID="txtPricePerMailbox" runat="server" placeholder="0.00" CssClass="form-control maskMoney" required></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanMailbox_PricePerExtraGB %></label>
                            <div class="col-sm-4 control-label">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon"><%= CloudPanel.Modules.Common.Settings.StaticSettings.CurrencySymbol %></span>
                                    <asp:TextBox ID="txtPricePerGB" runat="server" placeholder="0.00" CssClass="form-control maskMoney" required></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->

                </div>
                <!-- panel-primary -->

            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:LocalizedText, Buttons_Delete %>" CssClass="btn btn-danger" OnClick="btnDelete_Click" OnClientClick="return DeleteConfirm();" />
                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedText, Buttons_Save %>" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
            <!-- panel-footer -->

        </asp:Panel>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery-ui-1.10.3.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/toggles.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.validate.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.maskMoney.js") %>'></script>
    <script type="text/javascript">

        function DeleteConfirm()
        {
            return confirm('<%= Resources.LocalizedText.Global_ConfirmDeletePlan %>');
        }

        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });

            jQuery('.pop3').toggles({ checkbox: $('#<%= cbEnablePOP3.ClientID %>'), on: <%= cbEnablePOP3.Checked.ToString().ToLower() %> });
            jQuery('.imap').toggles({ checkbox: $('#<%= cbEnableIMAP.ClientID %>'), on: <%= cbEnableIMAP.Checked.ToString().ToLower() %> });
            jQuery('.owa').toggles({ checkbox: $('#<%= cbEnableOWA.ClientID %>'), on: <%= cbEnableOWA.Checked.ToString().ToLower() %> });
            jQuery('.outlook').toggles({ checkbox: $('#<%= cbEnableMAPI.ClientID %>'), on: <%= cbEnableMAPI.Checked.ToString().ToLower() %> });
            jQuery('.activesync').toggles({ checkbox: $('#<%= cbEnableAS.ClientID %>'), on: <%= cbEnableAS.Checked.ToString().ToLower() %> });
            jQuery('.ecp').toggles({ checkbox: $('#<%= cbEnableECP.ClientID %>'), on: <%= cbEnableECP.Checked.ToString().ToLower() %> });

            jQuery('.maskMoney').maskMoney({allowZero: true, defaultZero: true });

            // Must be greater than one spinner
            $(".spinner-256step").spinner(
                {
                    min: 256,
                    step: 256,
                    change: function (event, ui) {
                        if (!$.isNumeric($(this).spinner('value')))
                            $(this).spinner('value', 0);
                    }
                });

            // Rest of spinners
            $(".spinner").spinner(
                {
                    min: 0,
                    change: function (event, ui) {
                        if (!$.isNumeric($(this).spinner('value')))
                            $(this).spinner('value', 0);
                    }
                });

            $("#<%= btnSave.ClientID %>").click(function() {
                $("#form1").validate({
                    rules: {
                        <%= txtMaxRecipients.UniqueID %>: { number: true }
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

    </script>
</asp:Content>

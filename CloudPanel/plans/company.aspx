<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CloudPanel.plans.company" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-pencil-square"></i>Companies</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />


    <asp:Panel ID="panelPlan" runat="server" CssClass="row">
            <div class="form-horizontal">

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.PlanCompany_Details %></h4>
                    </div>

                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_Plan %></label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCompanyPlans" runat="server" CssClass="form-control chosen-select" 
                                    OnSelectedIndexChanged="ddlCompanyPlans_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfSelectedPlanID" runat="server" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_DisplayName %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxUsers %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxUsers" runat="server" CssClass="form-control spinner-users"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxDomains %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxDomains" runat="server" CssClass="form-control spinner-greaterthanone"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->

                </div>
                <!-- panel-primary -->

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%= Resources.LocalizedText.PlanCompany_Exchange %></h4>
                    </div>

                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxMailboxes %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxMailboxes" runat="server" CssClass="form-control spinner-lessthanusers"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxContacts %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxContacts" runat="server" CssClass="form-control spinner"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxGroups %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxGroups" runat="server" CssClass="form-control spinner"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxResourceMailboxes %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxResourceMailboxes" runat="server" CssClass="form-control spinner"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%= Resources.LocalizedText.PlanCompany_MaxMailPublicFolders %><span class="asterisk">*</span></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMaxMailPublicFolders" runat="server" CssClass="form-control spinner"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- panel-body -->

                </div>
                <!-- panel-primary -->

            </div>

            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:LocalizedText, PlanCompany_Delete %>" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedText, PlanCompany_Save %>" CssClass="btn btn-primary" OnClick="btnSave_Click" />
             </div>
                    <!-- panel-footer -->

        </asp:Panel>

        </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery-ui-1.10.3.min.js") %>'></script>
    <script type="text/javascript">
        var usersSpinner = null;

        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });

            // Users spinner
            usersSpinner = $(".spinner-users").spinner(
                {
                    min: 1,
                    change: function (event, ui) {
                        if (isNaN($(this).spinner('value')) == true || $(this).spinner('value') < 1)
                            $(this).spinner('value', 1);
                    }
                });

            // Must be greater than one spinner
            $(".spinner-greaterthanone").spinner(
                {
                    min: 1,
                    change: function (event, ui) {
                        if (!$.isNumeric(currentMaxUsers) || $(this).spinner('value') < 0)
                            $(this).spinner('value', 0);
                    }
                });

            // Must be less than the users spinner
            $(".spinner-lessthanusers").spinner(
                {
                    min: 0,
                    change: function (event, ui) {

                        var currentMaxUsers = usersSpinner.spinner('value');

                        if (!$.isNumeric(currentMaxUsers))
                            $(this).spinner('value', 0);
                        else {
                            if ($(this).spinner('value') > currentMaxUsers) {
                                alert('You cannot put in a value that is greater than the max users.');
                                $(this).spinner('value', currentMaxUsers);
                            }
                        }
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
        });

    </script>
</asp:Content>

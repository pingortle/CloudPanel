<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="horizontal-menu.ascx.cs" Inherits="CloudPanel.cpcontrols.horizontal_menu" %>

<ul class="nav nav-horizontal">
    <li><asp:HyperLink ID="hlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"><span><%= Resources.SharedResources.MENU_Dashboard %></span></asp:HyperLink></li>

    <asp:PlaceHolder ID="PlaceHolderResellersMenu" runat="server" Visible="false">
        <li><asp:HyperLink ID="hlResellers" runat="server" NavigateUrl="~/Resellers.aspx"><span><%= Resources.SharedResources.MENU_Resellers %></span></asp:HyperLink></li>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="PlaceHolderCompaniesMenu" runat="server" Visible="false">
        <li><asp:HyperLink ID="hlCompanies" runat="server" NavigateUrl="~/Companies.aspx"><span><%= Resources.SharedResources.MENU_Companies %></span></asp:HyperLink></li>
    </asp:PlaceHolder>

    <li class="nav-parent"><a href="#" class="dropdown-toggle" data-toggle="dropdown"><%= Resources.SharedResources.MENU_Plans %> <span class="caret"></span></a>
        <ul class="dropdown-menu children">
            <li><asp:HyperLink ID="hlPlansCompany" runat="server" NavigateUrl="~/plans/company.aspx"><%= Resources.SharedResources.MENU_CompanyPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanMailbox" runat="server" NavigateUrl="~/plans/mailbox.aspx"><%= Resources.SharedResources.MENU_MailboxPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanActiveSync" runat="server" NavigateUrl="~/plans/activesync.aspx"><%= Resources.SharedResources.MENU_ActivesyncPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanCitrix" runat="server" NavigateUrl="~/plans/company.aspx"><%= Resources.SharedResources.MENU_CitrixPlans %></asp:HyperLink></li>
        </ul>
    </li>
    <li><asp:HyperLink ID="hlReports" runat="server" NavigateUrl="#"><span><%= Resources.SharedResources.MENU_Reports %></span></asp:HyperLink></li>
    <li><a class="dropdown-toggle" data-toggle="dropdown" href="#"><i class="glyphicon glyphicon-search"></i></a>
        <div class="dropdown-menu">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search here..." onkeypress="return EnterEvent(event)"></asp:TextBox>
        </div>
        <asp:Button ID="btnSearch" runat="server" Text="Search" Style="display: none" OnClick="btnSearch_Click" />
    </li>
</ul>

<asp:PlaceHolder ID="PlaceHolderSelectedCompanyMenu" runat="server" Visible="false">
    <ul class="nav nav-horizontal">
        <li><asp:HyperLink ID="hlOverview" runat="server" NavigateUrl="~/company/overview.aspx"><span><%= Resources.SharedResources.MENU_Overview %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlDomains" runat="server" NavigateUrl="~/company/domains.aspx"><span><%= Resources.SharedResources.MENU_Domains %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlUsers" runat="server" NavigateUrl="~/company/users.aspx"><span><%= Resources.SharedResources.MENU_Users %></span></asp:HyperLink></li>
        <li class="nav-parent"><a href="#" class="dropdown-toggle" data-toggle="dropdown"><%= Resources.SharedResources.MENU_Email %>  <span class="caret"></span></a>
            <ul class="dropdown-menu children">
                <asp:PlaceHolder ID="PlaceHolderExchangeDisabled" runat="server" Visible="false">
                    <li><asp:HyperLink ID="hlEnableEmail" runat="server" NavigateUrl="~/company/email/enable.aspx"><%= Resources.SharedResources.MENU_EnableEmail %></asp:HyperLink></li>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolderExchangeEnabled" runat="server" Visible="false">
                    <li><asp:HyperLink ID="hlDisableEmail" runat="server" NavigateUrl="~/company/email/disable.aspx"><%= Resources.SharedResources.MENU_DisableEmail %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlContacts" runat="server" NavigateUrl="~/company/email/contacts.aspx"><%= Resources.SharedResources.MENU_Contacts %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlDistributionGroups" runat="server" NavigateUrl="~/company/email/groups.aspx"><%= Resources.SharedResources.MENU_Groups %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlResourceMailboxes" runat="server" NavigateUrl="~/company/email/resourcemailboxes.aspx"><%= Resources.SharedResources.MENU_ResourceMailboxes %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlPublicFolders" runat="server" NavigateUrl="~/company/email/publicfolders.aspx"><%= Resources.SharedResources.MENU_PublicFolders %></asp:HyperLink></li>
                </asp:PlaceHolder>
            </ul>
        </li>
        <li><asp:HyperLink ID="hlCitrix" runat="server" NavigateUrl="~/company/citrix/applications.aspx"><span><%= Resources.SharedResources.MENU_Citrix %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlBilling" runat="server" NavigateUrl="~/company/billing/customprices.aspx"><span><%= Resources.SharedResources.MENU_Billing %></span></asp:HyperLink></li>
    </ul>
</asp:PlaceHolder>

<script type="text/javascript">
    function EnterEvent(e) {
        if (e.keyCode == 13) {
            __doPostBack('<%= btnSearch.UniqueID %>', "");
                }
    }
</script>
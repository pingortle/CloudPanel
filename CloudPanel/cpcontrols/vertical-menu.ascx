<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vertical-menu.ascx.cs" Inherits="CloudPanel.cpcontrols.horizontal_menu" %>

<h5 class="sidebartitle">Navigation</h5>
<ul class="nav nav-pills nav-stacked nav-bracket">
    <li><asp:HyperLink ID="hlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"><i class="fa fa-home"></i><span><%= Resources.SharedResources.MENU_Dashboard %></span></asp:HyperLink></li>

    <asp:PlaceHolder ID="PlaceHolderResellersMenu" runat="server" Visible="false">
        <li><asp:HyperLink ID="hlResellers" runat="server" NavigateUrl="~/Resellers.aspx"><i class="fa fa-user"></i><span><%= Resources.SharedResources.MENU_Resellers %></span></asp:HyperLink></li>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="PlaceHolderCompaniesMenu" runat="server" Visible="false">
        <li><asp:HyperLink ID="hlCompanies" runat="server" NavigateUrl="~/Companies.aspx"><i class="fa fa-building-o"></i><span><%= Resources.SharedResources.MENU_Companies %></span></asp:HyperLink></li>
    </asp:PlaceHolder>

    <li class="nav-parent"><a href="#"><i class="fa fa-edit"></i><span><%= Resources.SharedResources.MENU_Plans %></span></a>
        <ul class="children">
            <li><asp:HyperLink ID="hlPlansCompany" runat="server" NavigateUrl="~/plans/company.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_CompanyPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanMailbox" runat="server" NavigateUrl="~/plans/mailbox.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_MailboxPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanActiveSync" runat="server" NavigateUrl="~/plans/activesync.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_ActivesyncPlans %></asp:HyperLink></li>
            <li><asp:HyperLink ID="hlPlanCitrix" runat="server" NavigateUrl="~/plans/company.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_CitrixPlans %></asp:HyperLink></li>
        </ul>
    </li>
    <li><asp:HyperLink ID="hlReports" runat="server" NavigateUrl="#"><i class="fa fa-file-o"></i><span><%= Resources.SharedResources.MENU_Reports %></span></asp:HyperLink></li>
</ul>

<hr />

<asp:PlaceHolder ID="PlaceHolderSelectedCompanyMenu" runat="server" Visible="false">
    <h5 class="sidebartitle"><%= CloudPanel.WebSessionHandler.SelectedCompanyName %></h5>
    <ul class="nav nav-pills nav-stacked nav-bracket">
        <li><asp:HyperLink ID="hlOverview" runat="server" NavigateUrl="~/company/overview.aspx"><i class="fa fa-info"></i><span><%= Resources.SharedResources.MENU_Overview %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlDomains" runat="server" NavigateUrl="~/company/domains.aspx"><i class="fa fa-globe"></i><span><%= Resources.SharedResources.MENU_Domains %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlUsers" runat="server" NavigateUrl="~/company/users.aspx"><i class="fa fa-users"></i><span><%= Resources.SharedResources.MENU_Users %></span></asp:HyperLink></li>
        <li class="nav-parent"><a href="#"><i class="fa fa-envelope-o"></i><span><%= Resources.SharedResources.MENU_Email %></span></a>
            <ul class="children">
                <asp:PlaceHolder ID="PlaceHolderExchangeDisabled" runat="server" Visible="false">
                    <li><asp:HyperLink ID="hlEnableEmail" runat="server" NavigateUrl="~/company/email/enable.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_EnableEmail %></asp:HyperLink></li>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolderExchangeEnabled" runat="server" Visible="false">
                    <li><asp:HyperLink ID="hlDisableEmail" runat="server" NavigateUrl="~/company/email/disable.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_DisableEmail %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlContacts" runat="server" NavigateUrl="~/company/email/contacts.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_Contacts %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlDistributionGroups" runat="server" NavigateUrl="~/company/email/groups.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_Groups %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlResourceMailboxes" runat="server" NavigateUrl="~/company/email/resourcemailboxes.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_ResourceMailboxes %></asp:HyperLink></li>
                    <li><asp:HyperLink ID="hlPublicFolders" runat="server" NavigateUrl="~/company/email/publicfolders.aspx"><i class="fa fa-caret-right"></i><%= Resources.SharedResources.MENU_PublicFolders %></asp:HyperLink></li>
                </asp:PlaceHolder>
            </ul>
        </li>
        <li><asp:HyperLink ID="hlCitrix" runat="server" NavigateUrl="~/company/citrix/applications.aspx"><i class="fa fa-cloud"></i><span><%= Resources.SharedResources.MENU_Citrix %></span></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlBilling" runat="server" NavigateUrl="~/company/billing/customprices.aspx"><i class="fa fa-money"></i><span><%= Resources.SharedResources.MENU_Billing %></span></asp:HyperLink></li>
    </ul>
</asp:PlaceHolder>

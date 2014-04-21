<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CloudPanel.Dashboard" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/Highcharts-3.0.1/js/highcharts.js") %>'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-home"></i>Dashboard</h2>
    </div>

    <div class="contentpanel">
        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="dashboardSuperAdmin" runat="server">
            <div class="row">
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <div class="tiles green added-margin m-b-20">
                            <div class="tiles-body">
                                <div class="tiles-title text-black">OVERVIEW </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Resellers</span>
                                        <span id="spanResellers" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Companies</span>
                                        <span id="spanCompanies" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper last">
                                        <span class="item-title">Users</span>
                                        <span id="spanUsers" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="tiles red added-margin  m-b-20">
                            <div class="tiles-body">
                                <div class="tiles-title text-black">EXCHANGE </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Mailboxes</span>
                                        <span id="spanMailboxes" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Groups</span>
                                        <span id="spanDistributionGroups" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Contacts</span>
                                        <span id="spanContacts" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="tiles blue added-margin  m-b-20">
                            <div class="tiles-body">
                                <div class="tiles-title text-black">CITRIX </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Apps</span>
                                        <span id="spanTotalApps" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">Servers</span>
                                        <span id="spanTotalServers" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper last">
                                        <span class="item-title">Users</span>
                                        <span id="spanTotalCitrixUsers" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="tiles black added-margin  m-b-20">
                            <div class="tiles-body">
                                <div class="tiles-title text-black">TODAY </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">New Companies</span>
                                        <span id="spanTodayNewCompanies" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                                <div class="widget-stats">
                                    <div class="wrapper transparent">
                                        <span class="item-title">New Users</span>
                                        <span id="spanTodayNewUsers" class="item-count animate-number semi-bold" data-value="0" data-animation-duration="1000" runat="server"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-8">
                                    <h5 class="subtitle mb5">Environment History</h5>
                                    <p class="mb15">Statistics over the past 12 months</p>
                                    <div style="width: 100%; margin-bottom: 20px">
                                        <asp:Literal ID="litAreaChart" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <h5 class="subtitle mb5">Environment Overview</h5>
                                    <p class="mb15">Overview of your entire environment</p>

                                    <span class="sublabel">Mailboxes 
                                        <asp:Label ID="lbTotalMailboxes" runat="server" Text="(0)"></asp:Label></span>
                                    <div class="progress progress-sm progress-striped active">
                                        <div class="progress-bar progress-bar-primary animate-progress-bar" data-percentage="0%" id="progBarMailboxes" runat="server"></div>
                                    </div>
                                    <!-- progress -->

                                    <span class="sublabel">Spaces Used vs Allocated
                                    <asp:Label ID="lbUsedVsAllocatedMailbox" runat="server" Text="(0)"></asp:Label></span>
                                    <div class="progress progress-sm progress-striped active">
                                        <div class="progress-bar progress-bar-success animate-progress-bar" data-percentage="0%" runat="server" id="progBarMailboxesUsedVsAllocated"></div>
                                    </div>
                                    <!-- progress -->

                                    <asp:PlaceHolder ID="PlaceHolderCitrixProgressBar" runat="server" Visible="false">
                                        <span class="sublabel">Citrix Users 
                                            <asp:Label ID="lbTotalCitrixUsers" runat="server" Text="(0)"></asp:Label></span>
                                        <div class="progress progress-sm progress-striped active">
                                            <div class="progress-bar progress-bar-danger animate-progress-bar" data-percentage="0%" runat="server" id="progBarCitrix"></div>
                                        </div>
                                    </asp:PlaceHolder>
                                    <!-- progress -->

                                    <asp:PlaceHolder ID="PlaceHolderLyncProgressBar" runat="server" Visible="false">
                                        <span class="sublabel">Lync Users 
                                            <asp:Label ID="lbTotalLyncUsers" runat="server" Text="(0)"></asp:Label></span>
                                        <div class="progress progress-sm progress-striped active">
                                            <div class="progress-bar progress-bar-warning animate-progress-bar" data-percentage="0%" runat="server" id="progBarLync"></div>
                                        </div>
                                    </asp:PlaceHolder>
                                    <!-- progress -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-6 col-md-6" runat="server" id="divBarChart">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <h5 class="subtitle mb5">Mailbox Database Sizes</h5>
                            <p class="mb15">Microsoft Exchange mailbox database sizes in gigabytes</p>
                            <div id="barchart" style="width: 100%; height: 300px">
                                <asp:Literal ID="litBarChart" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- col-md-6 -->

                <div class="col-sm-6 col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <h5 class="subtitle mb5">Most Recent Actions</h5>
                            <p class="mb15">The top 10 most recent actions that were performed in the system</p>
                            <div class="panel-alt widget-messaging">
                                <div style="overflow: auto; height: 300px">
                                    <ul>
                                        <asp:Repeater ID="repeaterAudits" runat="server">
                                            <ItemTemplate>
                                                <li style="background-color: #fcfcfc">
                                                    <small class="pull-right"><%# ((DateTime)Eval("WhenEntered")).ToString("MMMM dd hh:mm tt") %></small>
                                                    <h4 class="sender"><%# Eval("Username") %></h4>
                                                    <small><%# string.Format("{0}: {1}, {2}", Eval("ActionIDGlobalization"), Eval("Variable1"), Eval("Variable2")) %></small>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- col-md-6 -->

            </div>
        </asp:Panel>

        <br />

    </div>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.animate-progress-bar').each(function () {
                $(this).css('width', $(this).attr("data-percentage"));
            });

            $('.animate-number').each(function () {
                $(this).animateNumbers($(this).attr("data-value"), true, parseInt($(this).attr("data-animation-duration")));
            });
        });
    </script>
</asp:Content>

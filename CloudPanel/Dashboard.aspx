<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CloudPanel.Dashboard" %>
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

        <div class="row">
            <div class="col-sm-8 col-md-12">
                <div class="panel panel-default">
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-sm-8">
                                <h5 class="subtitle mb5"><%= Resources.LocalizedText.Dashboard_EnvironmentHistory %></h5>
                                <p class="mb15"><%= Resources.LocalizedText.Dashboard_EnvironmentHistoryInfo %></p>
                                <div  style="width: 100%; margin-bottom: 20px">
                                    <asp:Literal ID="litAreaChart" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <!-- col-sm-8 -->
                            <div class="col-sm-4">
                                <h5 class="subtitle mb5"><%= Resources.LocalizedText.Dashboard_EnvironmentOverview %></h5>
                                <p class="mb15"><%= Resources.LocalizedText.Dashboard_EnvironmentOverviewInfo %></p>

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_TotalUsers %></span>
                                        <h4><asp:Label ID="lbTotalUsers" runat="server" Text="0"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_TotalResellers %></span>
                                        <h4><asp:Label ID="lbTotalResellers" runat="server" Text="0"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_TotalCompanies %></span>
                                        <h4><asp:Label ID="lbTotalCompanies" runat="server" Text="0"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <br />
                                <br />

                                <span class="sublabel"><%= Resources.LocalizedText.Dashboard_Mailboxes %> <asp:Label ID="lbTotalMailboxes" runat="server" Text="(0)"></asp:Label></span>
                                <div class="progress progress-sm">
                                    <div style="width: 82%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-danger" runat="server" id="progBarMailboxes"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel"><%= Resources.LocalizedText.Dashboard_CitrixUsers %> <asp:Label ID="lbTotalCitrixUsers" runat="server" Text="(0)"></asp:Label></span>
                                <div class="progress progress-sm">
                                    <div style="width: 63%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-warning" runat="server" id="progBarCitrix"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel"><%= Resources.LocalizedText.Dashboard_LyncUsers %> <asp:Label ID="lbTotalLyncUsers" runat="server" Text="(0)"></asp:Label></span>
                                <div class="progress progress-sm">
                                    <div style="width: 63%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-info" runat="server" id="progBarLync"></div>
                                </div>
                                <!-- progress -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_TotalDomains %></span>
                                        <h4><asp:Label ID="lbTotalDomains" runat="server" Text="0"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_TotalAcceptedDomains %></span>
                                        <h4><asp:Label ID="lbTotalAcceptedDomains" runat="server" Text="0"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted"><%= Resources.LocalizedText.Dashboard_AllocatedEmailSpace %></span>
                                        <h4><asp:Label ID="lbTotalAllocatedMailboxSpace" runat="server" Text="Unknown"></asp:Label></h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                            </div>
                            <!-- col-sm-4 -->

                        </div>
                        <!-- row -->

                        <br />
                        <hr />
                        <br />

                        <div class="row">
                            <div class="col-md-6 mb30">
                                <h5 class="subtitle mb5"><%= Resources.LocalizedText.Dashboard_MailboxDatabaseSizeTitle %></h5>
                                <p class="mb15"><%= Resources.LocalizedText.Dashboard_MailboxDatabaseSizeSubTitle %></p>
                                <div id="barchart" style="width: 100%; height: 300px">
                                    <asp:Literal ID="litBarChart" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <!-- col-md-6 -->
                            <div class="col-sm-6 mb30">
                                <h5 class="subtitle mb5"><%= Resources.LocalizedText.Dashboard_MostRecentActionsTitle %></h5>
                                <p class="mb15"><%= Resources.LocalizedText.Dashboard_MostRecentActionsSubTitle %></p>
                                <div class="panel panel-default panel-alt widget-messaging">
                                    <div class="panel-body" style="overflow: auto; height: 300px">
                                        <ul >
                                        <asp:Repeater ID="repeaterAudits" runat="server">
                                            <ItemTemplate>
                                                <li style="background-color: #fcfcfc">
                                                    <small class="pull-right"><%# ((DateTime)Eval("WhenEntered")).ToString("MMMM dd hh:mm tt") %></small>
                                                    <h4 class="sender"><%# Eval("Username") %></h4>
                                                    <small><%# Eval("Message") %></small>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <!-- col-md-6 -->
                        </div>
                        <!-- row -->

                    </div>
                    <!-- panel-body -->
                </div>
                <!-- panel -->
            </div>
            <!-- col-sm-12 -->
        </div>
        <!-- row -->

    </div>
    <!-- contentpanel -->
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.resize.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/raphael-2.1.0.min.js") %>'></script>

    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.symbol.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.crosshair.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.categories.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/flot/flot.pie.min.js") %>'></script>

    <script type="text/javascript">

        jQuery(document).ready(function () {

            /***** PIE CHART *****/

            var piedata = [
                { label: "Series 1", data: [[1, 10]], color: '#D9534F' },
                { label: "Series 2", data: [[1, 30]], color: '#1CAF9A' },
                { label: "Series 3", data: [[1, 90]], color: '#F0AD4E' },
                { label: "Series 4", data: [[1, 70]], color: '#428BCA' },
                { label: "Series 5", data: [[1, 80]], color: '#5BC0DE' }
            ];

            jQuery.plot('#piechart', piedata, {
                series: {
                    pie: {
                        show: true,
                        radius: 1,
                        label: {
                            show: true,
                            radius: 2 / 3,
                            formatter: labelFormatter,
                            threshold: 0.1
                        }
                    }
                },
                grid: {
                    hoverable: true,
                    clickable: true
                }
            });

            function labelFormatter(label, series) {
                return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
            }
        });
    </script>
</asp:Content>

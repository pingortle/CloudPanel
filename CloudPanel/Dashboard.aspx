<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CloudPanel.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-home"></i>Dashboard</h2>
    </div>

    <div class="contentpanel">

        <div class="row">
            <div class="col-sm-8 col-md-12">
                <div class="panel panel-default">
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-sm-8">
                                <h5 class="subtitle mb5">Environment History</h5>
                                <p class="mb15">Statistics over the past few months</p>
                                <div id="basicflot" style="width: 100%; height: 300px; margin-bottom: 20px"></div>
                            </div>
                            <!-- col-sm-8 -->
                            <div class="col-sm-4">
                                <h5 class="subtitle mb5">Environment Overview</h5>
                                <p class="mb15">Overview of your entire environment</p>

                                <span class="sublabel">Resellers</span>
                                <div class="progress progress-sm">
                                    <div style="width: 40%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-primary"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel">Companies</span>
                                <div class="progress progress-sm">
                                    <div style="width: 32%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-success"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel">Mailboxes</span>
                                <div class="progress progress-sm">
                                    <div style="width: 82%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-danger"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel">Citrix Users</span>
                                <div class="progress progress-sm">
                                    <div style="width: 63%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-warning"></div>
                                </div>
                                <!-- progress -->

                                <span class="sublabel">Lync Users</span>
                                <div class="progress progress-sm">
                                    <div style="width: 63%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-info"></div>
                                </div>
                                <!-- progress -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted">Domains</span>
                                        <h4>$630,201</h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted">Accepted Domains</span>
                                        <h4>$106,850</h4>
                                    </div>
                                </div>
                                <!-- tinystat -->

                                <div class="tinystat mr20">
                                    <div class="datainfo">
                                        <span class="text-muted">Allocated Email Space</span>
                                        <h4>7.13TB</h4>
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
                                <h5 class="subtitle mb5">Mailbox Database Sizes</h5>
                                <p class="mb15">Microsoft Exchange mailbox database sizes in gigabytes</p>
                                <div id="barchart" style="width: 100%; height: 300px"></div>
                            </div>
                            <!-- col-md-6 -->
                            <div class="col-md-6 mb30">
                                <h5 class="subtitle mb5">Top 5 Largest Customers</h5>
                                <p class="mb15">The top 5 customers with the most users</p>
                                <div id="piechart" style="width: 100%; height: 300px"></div>
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
            var flash = [[0, 2], [1, 6], [2, 3], [3, 8], [4, 5], [5, 13], [6, 8]];
            var html5 = [[0, 5], [1, 4], [2, 4], [3, 1], [4, 9], [5, 10], [6, 13]];

            var plot = jQuery.plot(jQuery("#basicflot"),
               [{
                   data: flash,
                   label: "Flash",
                   color: "#1CAF9A"
               },
               {
                   data: html5,
                   label: "HTML5",
                   color: "#428BCA"
               }
               ],
             {
                 series: {
                     lines: {
                         show: true,
                         fill: true,
                         lineWidth: 1,
                         fillColor: {
                             colors: [{ opacity: 0.5 },
                                       { opacity: 0.5 }
                             ]
                         }
                     },
                     points: {
                         show: true
                     },
                     shadowSize: 0
                 },
                 legend: {
                     position: 'nw'
                 },
                 grid: {
                     hoverable: true,
                     clickable: true,
                     borderColor: '#ddd',
                     borderWidth: 1,
                     labelMargin: 10,
                     backgroundColor: '#fff'
                 },
                 yaxis: {
                     min: 0,
                     max: 15,
                     color: '#eee'
                 },
                 xaxis: {
                     color: '#eee'
                 }
             });

            var previousPoint = null;
            jQuery("#basicflot").bind("plothover", function (event, pos, item) {
                jQuery("#x").text(pos.x.toFixed(2));
                jQuery("#y").text(pos.y.toFixed(2));

                if (item) {
                    if (previousPoint != item.dataIndex) {
                        previousPoint = item.dataIndex;

                        jQuery("#tooltip").remove();
                        var x = item.datapoint[0].toFixed(2),
                        y = item.datapoint[1].toFixed(2);

                        showTooltip(item.pageX, item.pageY,
                             item.series.label + " of " + x + " = " + y);
                    }

                } else {
                    jQuery("#tooltip").remove();
                    previousPoint = null;
                }

            });

            jQuery("#basicflot").bind("plotclick", function (event, pos, item) {
                if (item) {
                    plot.highlight(item.series, item.datapoint);
                }
            });

            /***** BAR CHART *****/

            var bardata = [["Jan", 10], ["Feb", 23], ["Mar", 18], ["Apr", 13], ["May", 17], ["Jun", 30], ["Jul", 26], ["Aug", 16], ["Sep", 17], ["Oct", 5], ["Nov", 8], ["Dec", 15]];

            jQuery.plot("#barchart", [bardata], {
                series: {
                    lines: {
                        lineWidth: 1
                    },
                    bars: {
                        show: true,
                        barWidth: 0.5,
                        align: "center",
                        lineWidth: 0,
                        fillColor: "#428BCA"
                    }
                },
                grid: {
                    borderColor: '#ddd',
                    borderWidth: 1,
                    labelMargin: 10
                },
                xaxis: {
                    mode: "categories",
                    tickLength: 0
                }
            });


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

using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Dashboard;
using CloudPanel.Modules.Base.Statistics;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Common.ViewModel;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcessPlaceHolders();

                // Get area chart
                GetAreaChart();

                // Get mailbox database sizes
                GetBarChart();

                // Get overall stats
                GetOtherStatistics();

                // Get recent actions
                GetRecentAudits();
            }
        }

        private void ProcessPlaceHolders()
        {
            PlaceHolderCitrixProgressBar.Visible = StaticSettings.CitrixEnabled;
            PlaceHolderLyncProgressBar.Visible = StaticSettings.LyncEnabled;
        }

        private void GetAreaChart()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.ViewModelEvent += dashboard_ViewModelEvent;

            // Get statistics
            List<Dictionary<string, object>> values = dashboard.GetAreaChart();

            if (values != null)
            {
                Highcharts areaChart = new Highcharts("areaChart");
                areaChart.InitChart(new Chart()
                {
                    DefaultSeriesType = ChartTypes.Area,
                    BackgroundColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(Color.Transparent),
                    Height = 300
                });
                areaChart.SetPlotOptions(new PlotOptions()
                {
                    Series = new PlotOptionsSeries()
                    {
                        ConnectNulls = true,
                        ConnectEnds = true
                    }
                });
                areaChart.SetLegend(new DotNet.Highcharts.Options.Legend()
                {
                    Align = DotNet.Highcharts.Enums.HorizontalAligns.Center,
                    Layout = DotNet.Highcharts.Enums.Layouts.Horizontal,
                    VerticalAlign = DotNet.Highcharts.Enums.VerticalAligns.Bottom,
                    BorderWidth = 0
                });
                areaChart.SetCredits(new DotNet.Highcharts.Options.Credits() { Enabled = false });
                areaChart.SetTitle(new DotNet.Highcharts.Options.Title() { Text = "" });

                YAxis yAxis = new YAxis();
                yAxis.Title = new DotNet.Highcharts.Options.YAxisTitle() { Text = "" };
                yAxis.Min = 0;

                XAxis xAxis = new XAxis();
                xAxis.Categories = values[0].Keys.ToArray();

                List<Series> seriesCollection = new List<Series>();

                Series seriesUsers = new Series();
                seriesUsers.Data = new DotNet.Highcharts.Helpers.Data(values[0].Values.ToArray());
                seriesUsers.Name = "Users";
                seriesCollection.Add(seriesUsers);

                Series seriesMailbox = new Series();
                seriesMailbox.Data = new DotNet.Highcharts.Helpers.Data(values[1].Values.ToArray());
                seriesMailbox.Name = "Mailbox";
                seriesCollection.Add(seriesMailbox);

                if (StaticSettings.CitrixEnabled)
                {
                    Series seriesCitrix = new Series();
                    seriesCitrix.Data = new DotNet.Highcharts.Helpers.Data(values[2].Values.ToArray());
                    seriesCitrix.Name = "Citrix";
                    seriesCollection.Add(seriesCitrix);
                }

                areaChart.SetXAxis(xAxis);
                areaChart.SetYAxis(yAxis);
                areaChart.SetSeries(seriesCollection.ToArray());

                litAreaChart.Text = areaChart.ToHtmlString();
            }
            else
                litAreaChart.Text = Resources.LocalizedText.Dashboard_ErrorPopulatingAreaChart;
        }

        private void GetBarChart()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.ViewModelEvent += dashboard_ViewModelEvent;

            Dictionary<string, object> mdbData = dashboard.GetDatabaseSizeChart(ConfigurationManager.ConnectionStrings["CPDatabase"].ConnectionString);

            if (mdbData != null)
            {
                var databaseNames = from d in mdbData orderby d.Key select d.Key;
                var databaseValues = from d in mdbData orderby d.Key select d.Value;

                List<object> arrayValues = databaseValues.ToList();
                List<DotNet.Highcharts.Options.Point> points = new List<DotNet.Highcharts.Options.Point>();
                for (int i = 0; i< arrayValues.Count; i++)
                {
                    double theValue = 0;
                    double.TryParse(arrayValues[i].ToString(), out theValue);

                    points.Add(new DotNet.Highcharts.Options.Point()
                        {
                            Y = Number.GetNumber(theValue),
                            Color = decimal.Parse(arrayValues[i].ToString()) > 500 ? System.Drawing.Color.Red : System.Drawing.Color.Blue
                        });
                }

                Highcharts columnChart = new Highcharts("mdbBarChart")
                         .SetOptions(new DotNet.Highcharts.Helpers.GlobalOptions()
                         {
                             Lang = new DotNet.Highcharts.Helpers.Lang()
                             {
                                 DecimalPoint = Resources.LocalizedText.Formats_DecimalSeperator,
                                 ThousandsSep = Resources.LocalizedText.Formats_NumberGroupSeperator
                             }
                         })
                         .InitChart(new Chart
                         {
                             DefaultSeriesType = ChartTypes.Column,
                             BackgroundColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(Color.Transparent),
                             Height = 300
                         })
                         .SetTitle(new Title
                         {
                             Text = ""
                         })
                         .SetCredits(new Credits
                         {
                             Enabled = false
                         })
                         .SetXAxis(new XAxis
                         {
                             Categories = databaseNames.ToArray()
                         })
                         .SetYAxis(new YAxis
                         {
                             Title = new YAxisTitle { Text = "GB" },
                             Labels = new YAxisLabels() { Enabled = true },
                             PlotLines = new[]
                        {
                            new YAxisPlotLines
                            {
                                Value = 0,
                                Width = 1
                            }
                        }
                         })
                         .SetLegend(new Legend
                         {
                             Enabled = false
                         })
                         .SetPlotOptions(new PlotOptions()
                         {
                             Column = new PlotOptionsColumn()
                             {
                                 DataLabels = new PlotOptionsColumnDataLabels()
                                 {
                                     Enabled = true,
                                     Style = "fontWeight: 'bold'"
                                 }
                             }
                         })
                         .SetSeries(new[]
                    {
                        new Series 
                        { 
                            Name = "GB", 
                            Data = new DotNet.Highcharts.Helpers.Data(points.ToArray()) 
                        }
                    }
                     );

                litBarChart.Text = columnChart.ToHtmlString();

                // Change the div
                if (databaseNames.Count() > 8)
                    divBarChart.Style.Value = "col-sm-12 col-md-12"; // If more than 8 database then make the graph full length of page
                else
                    divBarChart.Style.Value = "col-sm-6 col-md-6"; // Otherwise half length
            }
            else
                litBarChart.Text = Resources.LocalizedText.Dashboard_ErrorPopulatingBarChart;
        }

        private void GetOtherStatistics()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.ViewModelEvent += dashboard_ViewModelEvent;

            OverallStats overall = dashboard.GetOtherStatistics(ConfigurationManager.ConnectionStrings["CPDatabase"].ConnectionString);
            if (overall != null)
            {
                spanUsers.Attributes.Add("data-value", overall.TotalUsers.ToString());
                spanResellers.Attributes.Add("data-value", overall.TotalResellers.ToString());
                spanCompanies.Attributes.Add("data-value", overall.TotalCompanies.ToString());

                spanMailboxes.Attributes.Add("data-value", overall.TotalMailboxes.ToString());
                spanDistributionGroups.Attributes.Add("data-value", overall.TotalDistributionGroups.ToString());
                spanContacts.Attributes.Add("data-value", overall.TotalMailContacts.ToString());

                spanTotalCitrixUsers.Attributes.Add("data-value", overall.TotalCitrixUsers.ToString());
                spanTotalApps.Attributes.Add("data-value", overall.TotalCitrixApps.ToString());
                spanTotalServers.Attributes.Add("data-value", overall.TotalCitrixServers.ToString());

                spanTodayNewCompanies.Attributes.Add("data-value", overall.TodayCompanies.ToString());
                spanTodayNewUsers.Attributes.Add("data-value", overall.TodayUsers.ToString());

                // Set mailbox space allocated
                lbUsedVsAllocatedMailbox.Text = string.Format("{0}{1} / {2}{3}", overall.TotalUsedEmailSpace.ToString("#.##"), overall.TotalUsedEmailSpaceSizeType, overall.TotalAllocatedEmailSpace.ToString("#.##"), overall.TotalAllocatedEmailSpaceSizeType);
                progBarMailboxesUsedVsAllocated.Attributes.Add("data-percentage", string.Format("{0}%", (int)Math.Round((decimal)(100 * overall.TotalUsedEmailSpaceInKB) / overall.TotalAllocatedEmailSpaceInKB)));

                // Set mailbox progress bar
                lbTotalMailboxes.Text = overall.TotalMailboxes.ToString();
                progBarMailboxes.Attributes.Add("data-percentage", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalMailboxes) / overall.TotalUsers)));

                // Set citrix progress bar
                lbTotalCitrixUsers.Text = overall.TotalCitrixUsers.ToString();
                progBarCitrix.Attributes.Add("data-percentage", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalCitrixUsers) / overall.TotalUsers)));

                // Set lync progress bar
                lbTotalLyncUsers.Text = overall.TotalLyncUsers.ToString();
                progBarLync.Attributes.Add("data-percentage", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalLyncUsers) / overall.TotalUsers)));
            }
        }

        private void GetRecentAudits()
        {
            var audits = AuditGlobalization.GetAuditing(string.Empty);

            if (audits != null)
            {
                // Select only the top 20
                IEnumerable<Audits> top20Audits = audits.Take(20);

                repeaterAudits.DataSource = top20Audits;
                repeaterAudits.DataBind();
            }
        }

        void dashboard_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }
    }
}
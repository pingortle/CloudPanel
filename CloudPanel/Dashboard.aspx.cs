using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Dashboard;
using CloudPanel.Modules.Base.Statistics;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Common.ViewModel;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
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
                        new Series { Name = "GB", Data = new DotNet.Highcharts.Helpers.Data(databaseValues.ToArray()) }
                    }
                     );

                litBarChart.Text = columnChart.ToHtmlString();
            }
            else
                litBarChart.Text = Resources.LocalizedText.Dashboard_ErrorPopulatingBarChart;
        }

        private void GetOtherStatistics()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.ViewModelEvent += dashboard_ViewModelEvent;

            OverallStats overall = dashboard.GetOtherStatistics(ConfigurationManager.ConnectionStrings["CPDatabase"].ConnectionString);
            lbTotalUsers.Text = overall.TotalUsers.ToString();
            lbTotalResellers.Text = overall.TotalResellers.ToString();
            lbTotalCompanies.Text = overall.TotalCompanies.ToString();
            lbTotalDomains.Text = overall.TotalDomains.ToString();
            lbTotalAcceptedDomains.Text = overall.TotalAcceptedDomains.ToString();
            lbTotalAllocatedMailboxSpace.Text = overall.TotalAllocatedEmailSpace;

            // Set mailbox progress bar
            lbTotalMailboxes.Text = overall.TotalMailboxes.ToString();
            progBarMailboxes.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalMailboxes) / overall.TotalUsers)));

            // Set citrix progress bar
            lbTotalCitrixUsers.Text = overall.TotalCitrixUsers.ToString();
            progBarCitrix.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalCitrixUsers) / overall.TotalUsers)));

            // Set lync progress bar
            lbTotalLyncUsers.Text = overall.TotalLyncUsers.ToString();
            progBarLync.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * overall.TotalLyncUsers) / overall.TotalUsers)));
        }

        private void GetRecentAudits()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.ViewModelEvent += dashboard_ViewModelEvent;

            List<Audits> audits = dashboard.RetrieveAudits();
            if (audits != null)
            {
                // Filter out the top 10
                IEnumerable<Audits> top10Audits = audits.Take(10);

                repeaterAudits.DataSource = top10Audits;
                repeaterAudits.DataBind();
            }
        }

        void dashboard_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }
    }
}
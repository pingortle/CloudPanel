<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="activesync.aspx.cs" Inherits="CloudPanel.plans.activesync" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">

    <div class="pageheader">
        <h2><i class="fa fa-pencil-square"></i><%= Resources.SharedResources.MENU_ActivesyncPlans %></h2>
    </div>

    <div class="contentpanel">
        <div class="row">
            <div class="col-md-12">
                <ul class="nav nav-tabs nav-justified">
                    <li class="active"><a href="#General" data-toggle="tab"><strong><%= Resources.LocalizedText.Settings_General %></strong></a></li>
                    <li><a href="#Password" data-toggle="tab"><strong><%= Resources.LocalizedText.Settings_Password %></strong></a></li>
                    <li><a href="#SyncSettings" data-toggle="tab"><strong><%= Resources.LocalizedText.ActiveSync_SyncSettings %></strong></a></li>
                    <li><a href="#Device" data-toggle="tab"><strong><%= Resources.LocalizedText.ActiveSync_Device %></strong></a></li>
                    <li><a href="#DeviceApplications" data-toggle="tab"><strong><%= Resources.LocalizedText.ActiveSync_DeviceApplications %></strong></a></li>
                </ul>

                <div class="tab-content" id="validateForm">

                    <div class="tab-pane active" id="General">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_Plan %></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlPlans" runat="server" CssClass="form-control chosen-select" DataTextField="DisplayName" DataValueField="ID" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_DisplayName %></label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_Description %></label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowNonProvisionableDevices" runat="server" />
                                                <label for='<%= cbAllowNonProvisionableDevices.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowNonProvisionableDevices %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_RefreshIntervalHours %></label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtRefreshInterval" runat="server" CssClass="form-control" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="Password">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <label class="col-sm-2 control-label">&nbsp;</label>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbRequirePassword" runat="server" />
                                                <label for='<%= cbRequirePassword.ClientID %>'><%= Resources.LocalizedText.ActiveSync_RequirePassword %></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="SyncSettings">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_IncludePastCalendar %></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlIncludePastCalendar" runat="server" CssClass="form-control chosen-select">
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_All %>" Value="All" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_TwoWeeks %>" Value="TwoWeeks" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_OneMonth %>" Value="OneMonth" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_ThreeMonths %>" Value="ThreeMonths" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_SixMonths %>" Value="SixMonths" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_IncludePastEmail %></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlIncludePastEmail" runat="server" CssClass="form-control chosen-select">
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_All %>" Value="All" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_OneDay %>" Value="OneDay" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_ThreeDays %>" Value="ThreeDays" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_OneWeek %>" Value="OneWeek" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_TwoWeeks %>" Value="TwoWeeks" />
                                                <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_PastQuantity_OneMonth %>" Value="OneMonth" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_LimitEmailSizeKB %></label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtEmailSizeLimit" runat="server" CssClass="form-control" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbRoamingManualSync" runat="server" />
                                                <label for='<%= cbRoamingManualSync.ClientID %>'><%= Resources.LocalizedText.ActiveSync_RequireManualSync %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowHTML" runat="server" />
                                                <label for='<%= cbAllowHTML.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowHTML %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowAttachmentDownloads" runat="server" />
                                                <label for='<%= cbAllowAttachmentDownloads.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowDownloadedAttachments %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_MaximumAttachmentSizeKB %></label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtMaxAttachmentSize" runat="server" CssClass="form-control" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="Device">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <strong><%= Resources.LocalizedText.ActiveSync_PremiumExchangeMessage %></strong>
                                    <br />
                                    <br />
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowRemovableStorage" runat="server" />
                                                <label for='<%= cbAllowRemovableStorage.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowRemovableStorage %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowCamera" runat="server" />
                                                <label for='<%= cbAllowCamera.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowCamera %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowWifi" runat="server" />
                                                <label for='<%= cbAllowWifi.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowWifi %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowInfrared" runat="server" />
                                                <label for='<%= cbAllowInfrared.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowInfrared %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowInternetSharing" runat="server" />
                                                <label for='<%= cbAllowInternetSharing.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowInternetSharing %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowRemoteDesktop" runat="server" />
                                                <label for='<%= cbAllowRemoteDesktop.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowRemoteDesktop %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowDesktopSync" runat="server" />
                                                <label for='<%= cbAllowDesktopSync.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowDesktopSync %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowTextMessaging" runat="server" />
                                                <label for='<%= cbAllowTextMessaging.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowTextMessaging %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><%= Resources.LocalizedText.ActiveSync_AllowBluetooth %></label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlBluetooth" runat="server" CssClass="form-control chosen-select">
                                            <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_BluetoothOptions_Allow %>" Value="Allow" />
                                            <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_BluetoothOptions_Handsfree %>" Value="Handsfree" />
                                            <asp:ListItem Text="<%$ Resources:LocalizedText, ActiveSync_BluetoothOptions_Disable %>" Value="Disable" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="DeviceApplications">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <strong><%= Resources.LocalizedText.ActiveSync_PremiumExchangeMessage %></strong>
                                    <br />
                                    <br />

                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowBrowser" runat="server" />
                                                <label for='<%= cbAllowBrowser.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowBrowser %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowConsumerEmail" runat="server" />
                                                <label for='<%= cbAllowConsumerEmail.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowConsumerEmail %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowUnsignedApplications" runat="server" />
                                                <label for='<%= cbAllowUnsignedApplications.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowUnsignedApps %></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowUnsignedPackages" runat="server" />
                                                <label for='<%= cbAllowUnsignedPackages.ClientID %>'><%= Resources.LocalizedText.ActiveSync_AllowUnsignedPackages %></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="row" style="margin-top: 25px;">
            <div class="panel-footer" style="text-align: right">
                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:SharedResources, BUTTON_Submit %>" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script src='<%= this.ResolveClientUrl("~/js/jquery.validate.min.js") %>'></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });
        });
    </script>
</asp:Content>

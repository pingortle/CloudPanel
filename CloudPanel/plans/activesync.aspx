<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="activesync.aspx.cs" Inherits="CloudPanel.plans.activesync" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">

    <div class="pageheader">
        <h2><i class="fa fa-pencil-square"></i>ActiveSync Plans</h2>
    </div>

    <div class="contentpanel">
        <div class="row">
            <div class="col-md-12">
                <ul class="nav nav-tabs nav-justified">
                    <li class="active"><a href="#General" data-toggle="tab"><strong>General</strong></a></li>
                    <li><a href="#Password" data-toggle="tab"><strong>Password</strong></a></li>
                    <li><a href="#SyncSettings" data-toggle="tab"><strong>Sync Settings</strong></a></li>
                    <li><a href="#Device" data-toggle="tab"><strong>Device</strong></a></li>
                    <li><a href="#DeviceApplications" data-toggle="tab"><strong>Device Applications</strong></a></li>
                </ul>

                <div class="tab-content" id="validateForm">

                    <div class="tab-pane active" id="General">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Plan</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlPlans" runat="server" CssClass="form-control chosen-select" DataTextField="DisplayName" DataValueField="ID" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Display Name</label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Description</label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowNonProvisionableDevices" runat="server" />
                                                <label for='<%= cbAllowNonProvisionableDevices.ClientID %>'>Allow non-provisionable devices</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Refresh Interval (Hours)</label>
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
                                                <label for='<%= cbRequirePassword.ClientID %>'>Require Password</label>
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
                                        <label class="col-sm-2 control-label">Include past calendar items</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlIncludePastCalendar" runat="server" CssClass="form-control chosen-select">
                                                <asp:ListItem Text="All" Value="All" />
                                                <asp:ListItem Text="Two Weeks" Value="TwoWeeks" />
                                                <asp:ListItem Text="One Month" Value="OneMonth" />
                                                <asp:ListItem Text="Three Months" Value="ThreeMonths" />
                                                <asp:ListItem Text="Six Months" Value="SixMonths" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Include past email items</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlIncludePastEmail" runat="server" CssClass="form-control chosen-select">
                                                <asp:ListItem Text="All" Value="All" />
                                                <asp:ListItem Text="One Day" Value="OneDay" />
                                                <asp:ListItem Text="Three Days" Value="ThreeDays" />
                                                <asp:ListItem Text="One Week" Value="OneWeek" />
                                                <asp:ListItem Text="Two Weeks" Value="TwoWeeks" />
                                                <asp:ListItem Text="One Month" Value="OneMonth" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Limit email size (KB)</label>
                                        <div class="col-sm-4"><asp:TextBox ID="txtEmailSizeLimit" runat="server" CssClass="form-control" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbRoamingManualSync" runat="server" />
                                                <label for='<%= cbRoamingManualSync.ClientID %>'>Require manual sync when roaming</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowHTML" runat="server" />
                                                <label for='<%= cbAllowHTML.ClientID %>'>Allow HTML formatted email</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowAttachmentDownloads" runat="server" />
                                                <label for='<%= cbAllowAttachmentDownloads.ClientID %>'>Allow attachments to be downloaded to device</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Maximum attachment size (KB)</label>
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
                                    <strong>The ability to modify policies on this tab is a premium Exchange ActiveSync feature that requires an Exchange Enterprise Client Access License for each mailbox policies are restricted on.</strong>
                                    <br />
                                    <br />
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowRemovableStorage" runat="server" />
                                                <label for='<%= cbAllowRemovableStorage.ClientID %>'>Allow removable storage</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowCamera" runat="server" />
                                                <label for='<%= cbAllowCamera.ClientID %>'>Allow camera</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowWifi" runat="server" />
                                                <label for='<%= cbAllowWifi.ClientID %>'>Allow Wi-Fi</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowInfrared" runat="server" />
                                                <label for='<%= cbAllowInfrared.ClientID %>'>Allow infrared</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowInternetSharing" runat="server" />
                                                <label for='<%= cbAllowInternetSharing.ClientID %>'>Allow internet sharing from device</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowRemoteDesktop" runat="server" />
                                                <label for='<%= cbAllowRemoteDesktop.ClientID %>'>Allow remote desktop from device</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowDesktopSync" runat="server" />
                                                <label for='<%= cbAllowDesktopSync.ClientID %>'>Allow desktop synchronization</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowTextMessaging" runat="server" />
                                                <label for='<%= cbAllowTextMessaging.ClientID %>'>Allow text messaging</label>
                                            </div>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label">Allow Bluetooth</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlBluetooth" runat="server" CssClass="form-control chosen-select">
                                            <asp:ListItem Text="Allow" Value="Allow" />
                                            <asp:ListItem Text="Handsfree Only" Value="Handsfree" />
                                            <asp:ListItem Text="Disable" Value="Disable" />
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
                                    <strong>The ability to modify policies on this tab is a premium Exchange ActiveSync feature that requires an Exchange Enterprise Client Access License for each mailbox policies are restricted on.</strong>
                                    <br />
                                    <br />

                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowBrowser" runat="server" />
                                                <label for='<%= cbAllowBrowser.ClientID %>'>Allow browser</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowConsumerEmail" runat="server" />
                                                <label for='<%= cbAllowConsumerEmail.ClientID %>'>Allow consumer email</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowUnsignedApplications" runat="server" />
                                                <label for='<%= cbAllowUnsignedApplications.ClientID %>'>Allow unsigned applications</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">&nbsp;</label>
                                        <div class="col-sm-4">
                                            <div class="ckbox ckbox-primary">
                                                <asp:CheckBox ID="cbAllowUnsignedPackages" runat="server" />
                                                <label for='<%= cbAllowUnsignedPackages.ClientID %>'>Allow unsigned installation packages</label>
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
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
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

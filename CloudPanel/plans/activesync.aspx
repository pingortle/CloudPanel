<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="activesync.aspx.cs" Inherits="CloudPanel.plans.activesync" %>
<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">

    <div class="pageheader">
        <h2><i class="fa fa-pencil-square"></i>ActiveSync Plans***</h2>
    </div>

    <div class="contentpanel">
        <div class="row">
            <div class="col-md-12">
                <ul class="nav nav-tabs nav-justified">
                    <li class="active"><a href="#General" data-toggle="tab"><strong>General***</strong></a></li>
                    <li><a href="#Password" data-toggle="tab"><strong>Password***</strong></a></li>
                    <li><a href="#SyncSettings" data-toggle="tab"><strong>Sync Settings***</strong></a></li>
                    <li><a href="#Device" data-toggle="tab"><strong>Device***</strong></a></li>
                    <li><a href="#DeviceApplications" data-toggle="tab"><strong>Device Applications***</strong></a></li>
                </ul>

                <div class="tab-content" id="validateForm">

                    <div class="tab-pane active" id="General">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Plan***</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtPlan" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Display Name***</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Description***</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbAllowNonProvisionableDevices" runat="server" />
                                            <label for='<%= cbAllowNonProvisionableDevices.ClientID %>'>Allow non-provisionable devices***</label>
                                        </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Refresh Interval (Hours)***</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtRefreshInterval" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="Password">
                        <div class="form-horizontal">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="ckbox ckbox-primary">
                                            <asp:CheckBox ID="cbRequirePassword" runat="server" />
                                            <label for='<%= cbRequirePassword.ClientID %>'>Require Password***</label>
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
                <asp:Button ID="btnSubmit" runat="server" Text="Submit***" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
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

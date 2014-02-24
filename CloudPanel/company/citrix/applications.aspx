<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="applications.aspx.cs" Inherits="CloudPanel.company.citrix.applications" %>

<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-cloud"></i>Virtual Applications and Servers</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <div class="people-list">
            <div class="row">

                <h2>Servers</h2>
                <asp:Repeater ID="serversRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6">
                            <div class="people-item">
                                <div class="media">
                                    <a href="#" class="pull-left">
                                        <asp:Image ID="imgServerPicture" runat="server" ImageUrl='<%# Eval("PictureURL") %>' CssClass="thumbnail media-object" />
                                    </a>
                                    <div class="media-body">
                                        <div style="float: right">
                                            <asp:Button ID="btnEditMembers" runat="server" Text="Edit Members" CssClass="btn btn-primary btn-xs" />
                                        </div>

                                        <h4 class="person-name">
                                            <asp:Label ID="lbServerName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </h4>
                                        <div class="text-muted">
                                            <i class="fa fa-money"></i>
                                            <asp:Label ID="lbPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label> per user
                                        </div>
                                        <div class="text-muted">
                                            <i class="fa fa-info"></i>
                                            <asp:Label ID="lbServerDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- col-md-6 -->
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="people-list">
            <div class="row">

                <h2>Applications</h2>
                <asp:Repeater ID="repeaterApps" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6">
                            <div class="people-item">
                                <div class="media">
                                    <a href="#" class="pull-left">
                                        <asp:Image ID="imgAppPicture" runat="server" ImageUrl='<%# Eval("PictureURL") %>' CssClass="thumbnail media-object" />
                                    </a>
                                    <div class="media-body">
                                        <div style="float: right">
                                            <asp:Button ID="btnEditMembers" runat="server" Text="Edit Members" CssClass="btn btn-primary btn-xs" />
                                        </div>
                                        <h4 class="person-name">
                                            <asp:Label ID="lbAppName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </h4>
                                        <div class="text-muted">
                                            <i class="fa fa-money"></i>
                                            <asp:Label ID="lbPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label> per user
                                        </div>
                                        <div class="text-muted">
                                            <i class="fa fa-info"></i>
                                            <asp:Label ID="lbAppDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- col-md-6 -->
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
</asp:Content>

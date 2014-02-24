<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="alertmessage.ascx.cs" Inherits="CloudPanel.cpcontrols.alertmessage" %>

<div class="alert alert-success" id="alertMessageDiv" runat="server" style="display: none">
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>

    <strong><asp:Label ID="lbErrorType" runat="server" Text=""></asp:Label></strong> &nbsp; &nbsp; <asp:Literal ID="litMessage" runat="server"></asp:Literal>
</div>

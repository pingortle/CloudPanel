<%@ Page Title="" Language="C#" MasterPageFile="~/CloudPanel.Master" AutoEventWireup="true" CodeBehind="groups.aspx.cs" Inherits="CloudPanel.company.email.groups" %>

<%@ Register Src="~/cpcontrols/alertmessage.ascx" TagPrefix="uc1" TagName="alertmessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainPanel" runat="server">
    <div class="pageheader">
        <h2><i class="fa fa-users"></i>Distribution Groups</h2>
    </div>

    <div class="contentpanel">

        <uc1:alertmessage runat="server" ID="alertmessage" />

        <asp:Panel ID="panelGroupList" runat="server" CssClass="row">

            <div style="float: right; margin: 10px">
                <asp:Button ID="btnAddGroup" runat="server" Text="Add New Group" CssClass="btn btn-success" OnClick="btnAddGroup_Click" />
            </div>

            <div class="col-md-12">
                <div class="table-responsive">
                    <table class="table table-striped mb30">
                        <thead>
                            <tr>
                                <th>Display Name</th>
                                <th>Email</th>
                                <th>Hidden</th>
                                <th style="width: 10%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>All of Compsys</td>
                                <td>AllOfCompsys@compsysar.com</td>
                                <th>Yes</th>
                                <td>
                                    <div class="btn-group">
                                        <span class="btn btn-xs btn-primary">Modify</span>
                                        <button type="button" class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>'>Edit</asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                            <li>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Disable" CommandArgument='<%# Eval("ID") %>'>Disable</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- btn-group -->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- table-responsive -->
            </div>
            <!-- col-md-6 -->

        </asp:Panel>

        <asp:Panel ID="panelEditGroup" runat="server" CssClass="row" Visible="false">

            <div class="form-horizontal">

                <!-- General -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Add/Edit Distribution Group</h4>
                        </div>

                        <div class="panel-body">

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Display Name <span class="asterisk">*</span></label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hfGroupDistinguishedName" runat="server" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">E-mail <span class="asterisk">*</span></label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control mb15">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <!-- col-sm-6 -->

                            <div class="form-group">
                                <label class="col-sm-2 control-label">&nbsp;</label>
                                <div class="col-sm-4">
                                    <div class="ckbox ckbox-success">
                                        <asp:CheckBox ID="cbHidden" runat="server" />
                                        <label for='<%= cbHidden.ClientID %>'>Is group hidden from global address list?</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- panel-body -->

                    </div>
                </div>

                <!-- Owners -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Group Owners</h4>
                        </div>

                        <div class="panel-body">
                            <div class="table-responsive" style="height: 300px; overflow-y: auto;">
                                <asp:Repeater ID="repeaterOwners" runat="server">
                                    <HeaderTemplate>
                                        <table id="tableOwners" class="table mb30">
                                            <thead>
                                                <tr>
                                                    <th>&nbsp;</th>
                                                    <th>Display Name</th>
                                                    <th>E-mail Address</th>
                                                    <th>&nbsp;</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td></td>
                                            <td><%# Eval("DisplayName") %></td>
                                            <td><%# Eval("Email") %></td>
                                            <td class="table-action">
                                                <a class="delete-row" onclick="javascript: RemoveRow(this, 'Owners');" style="cursor: pointer"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:HiddenField ID="hfGroupOwners" runat="server" />
                            </div>
                            <!-- table-responsive -->
                        </div>
                        <!-- panel-body -->

                    </div>
                </div>

                <!-- Members -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Group Members</h4>
                        </div>

                        <div class="panel-body">
                            <div class="table-responsive" style="height: 300px; overflow-y: auto;">
                                <asp:Repeater ID="repeaterMembers" runat="server">
                                    <HeaderTemplate>
                                        <table id="tableMembers" class="table mb30">
                                            <thead>
                                                <tr>
                                                    <th>&nbsp;</th>
                                                    <th>Display Name</th>
                                                    <th>E-mail Address</th>
                                                    <th>&nbsp;</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td></td>
                                            <td><%# Eval("DisplayName") %></td>
                                            <td><%# Eval("Email") %></td>
                                            <td class="table-action">
                                                <a class="delete-row" onclick="javascript: RemoveRow(this, 'Members');" style="cursor: pointer"><i class="fa fa-trash-o"></i></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:HiddenField ID="hfGroupMembers" runat="server" />
                            </div>
                            <!-- table-responsive -->
                        </div>
                        <!-- panel-body -->

                    </div>
                </div>

                <!-- Membership Approval -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Membership Approval</h4>
                        </div>

                        <div class="panel-body">

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Owner Approval Required to Join?</label>
                                <div class="col-sm-8">
                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbMAOpen" runat="server" GroupName="MembershipApproval" Checked="true" />
                                        <label for="<%= cbMAOpen.ClientID %>">OPEN: Anyone can join this group without being approved by the group owners</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbMAClosed" runat="server" GroupName="MembershipApproval" />
                                        <label for="<%= cbMAClosed.ClientID %>">CLOSED: Members can be added only by the group owners. All requests to join will be rejected automatically</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbMAOwnerApproval" runat="server" GroupName="MembershipApproval" />
                                        <label for="<%= cbMAOwnerApproval.ClientID %>">OWNER APPROVAL: All requests are approved or rejected by the group owners</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Can Users Leave?</label>
                                <div class="col-sm-8">
                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="rbMALeaveOpen" runat="server" GroupName="MembershipApproval2" Checked="true" />
                                        <label for="<%= rbMALeaveOpen.ClientID %>">OPEN: Anyone can leave this group without being approved by the group owners</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="rbMALeaveClosed" runat="server" GroupName="MembershipApproval2" />
                                        <label for="<%= rbMALeaveClosed.ClientID %>">CLOSED: Members can be removed only by the group owners. All requests to leave will be rejected automatically</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- panel-body -->

                    </div>
                </div>

                <!-- Delivery Management -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Delivery Management</h4>
                        </div>

                        <div class="panel-body">

                            <div class="form-group">
                                <label class="col-sm-2 control-label"></label>
                                <div class="col-sm-8">
                                    By default, only senders inside your organization can send messages to this group. You can also allow people outside the organization to send to this group. Choose one of the options below:<br />

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbDMInsideOnly" runat="server" GroupName="DeliveryManagement" Checked="true" />
                                        <label for="<%= cbDMInsideOnly.ClientID %>">Only senders inside my organization can send to this group</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbDMOutsideInside" runat="server" GroupName="DeliveryManagement" />
                                        <label for="<%= cbDMOutsideInside.ClientID %>">Senders inside and outside of my organization can send to this group</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label"></label>
                                <div class="col-sm-8">
                                    If you want to restrict who can send messages to the group, add users or groups to the list below. Only the specified senders will be able to send to the group and mail sent by anyone else will be rejected.<br />

                                    <asp:DropDownList ID="ddlDeliveryManagement" runat="server" data-placeholder="Choose multiple users..." CssClass="chosen-select" multiple>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>
                        <!-- panel-body -->

                    </div>
                </div>

                <!-- Message Approval -->
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Message Approval</h4>
                        </div>

                        <div class="panel-body">

                            <div class="form-group">
                                <label class="col-sm-2 control-label">&nbsp;</label>
                                <div class="col-sm-4">
                                    <div class="ckbox ckbox-success">
                                        <asp:CheckBox ID="cbMAApprovedByModerator" runat="server" />
                                        <label for='<%= cbMAApprovedByModerator.ClientID %>'>Messages sent to this group have to be approved by a moderator</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Group Moderators</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlGroupModerators" runat="server" data-placeholder="Choose multiple users..." CssClass="chosen-select" multiple>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Senders that don't require approval</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlSendersDontRequireApproval" runat="server" data-placeholder="Choose multiple users..." CssClass="chosen-select" multiple>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label"></label>
                                <div class="col-sm-8">
                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbMANotifyAll" runat="server" GroupName="MessageApproval" />
                                        <label for="<%= cbMANotifyAll.ClientID %>">Notify all senders when their messages aren't approved</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbMANotifyInternal" runat="server" GroupName="MessageApproval" />
                                        <label for="<%= cbMANotifyInternal.ClientID %>">Notify senders in your organization when their messages aren't approved</label>
                                    </div>

                                    <div class="rdio rdio-success">
                                        <asp:RadioButton ID="cbDontNotify" runat="server" GroupName="MessageApproval" Checked="true" />
                                        <label for="<%= cbDontNotify.ClientID %>">Don't notify anyone when a message isn't approved</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- panel-body -->

                    </div>

                    <div class="panel-footer" style="text-align: right">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" />
                    </div>
                    <!-- panel-footer -->

                </div>

            </div>
        </asp:Panel>

    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphJavascript" runat="server">
    <script src='<%= this.ResolveClientUrl("~/js/chosen.jquery.min.js") %>'></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            // Chosen Select
            jQuery(".chosen-select").chosen({ 'width': '100%', 'white-space': 'nowrap' });

        });

        function RemoveRow(row, section) {
            var table = $(row).closest('table');
            var tableRow = $(row).closest('tr');

            var objectValue = $(tableRow).find('td').eq(2).text();

            var theHiddenField = null;
            if (section == "Owners") {
                hiddenField = document.getElementById("<%= hfGroupOwners.ClientID %>");

                $(tableRow).remove();
            } else if (section == "Members") {
                hiddenField = document.getElementById("<%= hfGroupMembers.ClientID %>");

                    $(tableRow).remove();
                }

            hiddenField.value = hiddenField.value.replace(objectValue + "||", "");
        }
    </script>
</asp:Content>

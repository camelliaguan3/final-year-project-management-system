<%@ Page Title="Assign Group To FYP" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignGroupToFYP.aspx.cs" Inherits="FYPMSWebsite.Faculty.AssignGroupToFYP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Assign Group To FYP</strong></span></h4>
        <!-- Number of groups currently supervised -->
        <asp:Panel ID="pnlSelectFYP" runat="server">
            <div class="form-group">
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:Label ID="lblNumberAssignedMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
                <!-- Select project -->
                <asp:Label runat="server" Text="FYP:" CssClass="control-label col-xs-1" AssociatedControlID="ddlFYPs"></asp:Label>
                <div class="col-xs-11" style="margin-top: 10px">
                    <asp:DropDownList ID="ddlFYPs" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlFYPs_SelectedIndexChanged" CausesValidation="True"></asp:DropDownList>
                </div>
                <div class="col-xs-offset-1 col-xs-11" style="margin-top: 10px">
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Please select a project." ControlToValidate="ddlFYPs"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ToolTip="Groups" InitialValue="none selected"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblSelectFYPMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGroupAssignment" runat="server" Visible="False">
            <!-- Groups currently assigned to a project -->
            <div class="form-group">
                <div class="col-xs-12">
                    <h5><span style="text-decoration: underline; color: #00008B" class="h5"><strong>Groups Assigned To This FYP:</strong></span></h5>
                    <asp:GridView ID="gvCurrentlyAssigned" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowDataBound="GvCurrentlyAssigned_RowDataBound">
                        <HeaderStyle Wrap="False" />
                    </asp:GridView>
                    <!-- Error/feedback message for groups currently assigned to a project -->
                    <asp:Label ID="lblCurrentlyAssignedMessage" runat="server" CssClass="label-info" Visible="false" BackColor="Transparent"></asp:Label>
                </div>
            </div>
            <!-- Groups available to assign to a project -->
            <div class="form-group">
                <div class="col-xs-12">
                    <h5><span style="text-decoration: underline; color: #00008B" class="h5"><strong>Groups Available To Be Assigned To This FYP:</strong></span></h5>
                    <asp:GridView ID="gvAvailableToAssign" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowDataBound="GvAvailableToAssign_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SELECT">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <!-- Error/feedback message for groups available to assign to a project -->
                    <div>
                        <asp:Label ID="lblAvailableToAssignMessage" runat="server" CssClass="label-info" Visible="false" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
                <asp:Panel ID="pnlAssignButton" runat="server" Visible="false">
                    <div class="col-xs-2" style="margin-top: 12px">
                        <asp:Button ID="btnAssignGroups" runat="server" Text="Assign Selected Groups" CssClass="btn-sm" OnClick="BtnAssignGroups_Click" />
                    </div>
                    <div class="col-xs-10" style="margin-top: 18px">
                        <asp:Label ID="lblNoGroupAssignedMessage" runat="server" CssClass="label-info" Visible="false" BackColor="Transparent"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>
        <!-- Error/feedback message -->
        <div class="col-xs-12">
            <asp:Label ID="lblAssignGroupMessage" runat="server" CssClass="label-info" Visible="false" BackColor="Transparent"></asp:Label>
        </div>
    </div>
</asp:Content>

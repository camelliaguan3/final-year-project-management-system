<%@ Page Title="Assign Readers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignReaders.aspx.cs" Inherits="FYPMSWebsite.Coordinator.AssignReaders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Project Groups Without Assigned Readers</strong></span></h4>
        <!-- Groups without readers -->
        <div class="form-group">
            <div class="col-xs-12" style="margin-bottom: 6px">
                <asp:GridView ID="gvProjectGroupsWithoutReaders" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                    AutoGenerateSelectButton="True" OnSelectedIndexChanged="GvProjectGroupsWithoutReaders_SelectedIndexChanged"
                    OnRowDataBound="GvProjectGroupsWithoutReaders_RowDataBound">
                </asp:GridView>
            </div>
            <div class="col-xs-12">
                <asp:Label ID="lblGetSupervisorsMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
            <div class="col-xs-12">
                <asp:Label ID="lblProjectGroupsWithoutReadersMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="pnlAssignReader" runat="server" Visible="False">
            <br />
            <h5><span style="text-decoration: underline; color: #00008B" class="h5"><strong>Readers Available For Assignment To Selected Project Group</strong></span></h5>
            <div class="form-group">
                <!-- Group code and title -->
                <asp:Label runat="server" Text="Selected Group:" CssClass="control-label col-xs-2" AssociatedControlID="txtSelectedGroup"></asp:Label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtSelectedGroup" runat="server" CssClass="form-control-static" ReadOnly="True" Wrap="False" BorderColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%">
                    </asp:TextBox>
                </div>
            </div>
            <!-- Available readers -->
            <div class="form-group">
                <div class="col-xs-12" style="margin-bottom: 6px">
                    <asp:GridView ID="gvAvailableReaders" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        AutoGenerateSelectButton="True" OnSelectedIndexChanged="GvAvailableReaders_SelectedIndexChanged"
                        OnRowDataBound="GvAvailableReaders_RowDataBound">
                    </asp:GridView>
                </div>
                <div class="col-xs-12">
                    <asp:Label ID="lblAvailableReadersMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <div class="form-group">
            <div class="col-xs-12">
                <asp:HyperLink ID="hlDisplayReaders" runat="server" NavigateUrl="~/Coordinator/DisplayReaders.aspx">Project Groups With Assigned Readers</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

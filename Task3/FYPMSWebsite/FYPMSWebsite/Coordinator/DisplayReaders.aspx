<%@ Page Title="Display Readers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayReaders.aspx.cs" Inherits="FYPMSWebsite.Coordinator.DisplayReaders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Project Groups With Assigned Readers</strong></span></h4>
        <!-- Display readers -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:GridView ID="gvAssignedReaders" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                    OnRowDataBound="GvAssignedReaders_RowDataBound">
                </asp:GridView>
            </div>
            <!-- Error/feedback message -->
            <div class="col-xs-12">
                <asp:Label ID="lblAssignedReadersMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
        <!-- Hyperlink to display assigned readers -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:HyperLink ID="hlAssignReaders" runat="server" NavigateUrl="~/Coordinator/AssignReaders.aspx">Project Groups Without Assigned Readers</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

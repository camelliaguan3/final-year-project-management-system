<%@ Page Title="View Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewGrades.aspx.cs" Inherits="FYPMSWebsite.Student.ViewGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Your Grades</strong></span></h4>
        <asp:Panel ID="pnlTitle" runat="server" Visible="false">
            <!-- FYP title -->
            <div class="form-group">
                <asp:Label runat="server" Text="FYP Title:" CssClass="control-label col-xs-1" AssociatedControlID="txtTitle"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-static" Visible="false" ReadOnly="True" BackColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%"></asp:TextBox>
                    <div style="margin-top: 6px">
                        <asp:Label ID="lblTitleMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlStudentGrades" runat="server" Visible="false">
            <div class="form-group">
                <!-- Supervisor -->
                <asp:Label runat="server" Text="Supervisor:" CssClass="control-label col-xs-1" AssociatedControlID="txtSupervisors"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtSupervisors" runat="server" CssClass="form-control-static" ReadOnly="True" Wrap="False" BorderColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%">
                    </asp:TextBox>
                </div>
                <!-- Supervisor grades -->
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:GridView ID="gvSupervisorGrades" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowDataBound="GvSupervisorGrades_RowDataBound">
                    </asp:GridView>
                </div>
                <!-- Supervisor grades error message -->
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:Label ID="lblSupervisorGradesMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <!-- Reader -->
                <asp:Label runat="server" Text="Reader:" CssClass="control-label col-xs-1" AssociatedControlID="txtReader"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtReader" runat="server" CssClass="form-control-static" ReadOnly="True" Wrap="False" BorderColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%">
                    </asp:TextBox>
                </div>
                <!-- Reader grades -->
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:GridView ID="gvReaderGrades" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowDataBound="GvReaderGrades_RowDataBound">
                    </asp:GridView>
                </div>
                <!-- Reader grades error message -->
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:Label ID="lblReaderGradesMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <div class="col-xs-12">
            <asp:Label ID="lblProjectAssignedMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
        </div>
        <div class="col-xs-12">
            <asp:Label ID="lblGetGroupIdMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
        </div>
    </div>
</asp:Content>

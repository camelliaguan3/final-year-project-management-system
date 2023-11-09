<%@ Page Title="Review Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewGrades.aspx.cs" Inherits="FYPMSWebsite.Coordinator.ReviewGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Review Grades</strong></span></h4>
        <asp:Panel ID="pnlSelectGroup" runat="server">
            <!-- Assigned groups dropdown list -->
            <div class="form-group">
                <asp:Label runat="server" Text="Group:" CssClass="control-label col-xs-1" AssociatedControlID="ddlAssignedGroups"></asp:Label>
                <div class="col-xs-6" style="margin-top: 10px; margin-bottom: 6px">
                    <asp:DropDownList ID="ddlAssignedGroups" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlAssignedGroups_SelectedIndexChanged" CausesValidation="True"></asp:DropDownList>
                </div>
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Please select a group." ControlToValidate="ddlAssignedGroups"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ToolTip="Groups" InitialValue="none selected"></asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:Label ID="lblSelectAssignedGroupMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlStudentGrades" runat="server" Visible="false">
            <!-- Supervisor -->
            <div class="form-group">
                <asp:Label runat="server" Text="Supervisor:" CssClass="control-label col-xs-1" AssociatedControlID="txtSupervisors"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtSupervisors" runat="server" CssClass="form-control-static" ReadOnly="True" Wrap="False" BorderColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%">
                    </asp:TextBox>
                </div>
            </div>
            <!-- Supervisor grades -->
            <div class="form-group">
                <div class="col-xs-offset-1 col-xs-11" style="margin-bottom: 10px">
                    <asp:GridView ID="gvSupervisorGrades" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowCancelingEdit="GvSupervisorGrades_RowCancelingEdit" OnRowEditing="GvSupervisorGrades_RowEditing" OnRowUpdating="GvSupervisorGrades_RowUpdating"
                        OnRowDataBound="GvSupervisorGrades_RowDataBound" AutoGenerateEditButton="True" DataKeyNames="NAME">
                        <HeaderStyle Wrap="False" />
                    </asp:GridView>
                </div>
                <!-- Supervisor grades error messages -->
                <div class="col-xs-offset-1 col-xs-11">
                    <div>
                        <asp:Label ID="lblSupervisorGradesMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblSupervisorProposalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblSupervisorProgressGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblSupervisorFinalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblSupervisorPresentationGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
            <!-- Reader -->
            <div class="form-group">
                <asp:Label runat="server" Text="Reader:" CssClass="control-label col-xs-1" AssociatedControlID="txtReader"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtReader" runat="server" CssClass="form-control-static" ReadOnly="True" Wrap="False" BorderColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%">
                    </asp:TextBox>
                </div>
            </div>
            <!-- Reader grades -->
            <div class="form-group">
                <div class="col-xs-offset-1 col-xs-11" style="margin-bottom: 10px">
                    <asp:GridView ID="gvReaderGrades" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowCancelingEdit="GvReaderGrades_RowCancelingEdit" OnRowEditing="GvReaderGrades_RowEditing" OnRowUpdating="GvReaderGrades_RowUpdating"
                        OnRowDataBound="GvReaderGrades_RowDataBound" AutoGenerateEditButton="True" DataKeyNames="NAME">
                        <HeaderStyle Wrap="False" />
                    </asp:GridView>
                </div>
                <!-- Reader grades error messages -->
                <div class="col-xs-offset-1 col-xs-11">
                    <div>
                        <asp:Label ID="lblReaderGradesMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblReaderProposalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblReaderProgressGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblReaderFinalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblReaderPresentationGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!-- No group assigned -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:Label ID="lblNoAssignedGroupMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

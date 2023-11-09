<%@ Page Title="Grade Group" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GradeGroup.aspx.cs" Inherits="FYPMSWebsite.Faculty.GradeGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Grade Project Group</strong></span></h4>
        <div class="form-group">
            <!-- Select role radio buttons -->
            <asp:Label runat="server" Text="Grade as:" CssClass="control-label col-xs-2" AssociatedControlID="rblGraderRole"></asp:Label>
            <div class="col-xs-3">
                <asp:RadioButtonList ID="rblGraderRole" runat="server" CssClass="" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    AutoPostBack="True" OnSelectedIndexChanged="RblGraderRole_SelectedIndexChanged">
                    <asp:ListItem class="radio-inline" Value="supervisor" Selected="True">Supervisor</asp:ListItem>
                    <asp:ListItem class="radio-inline" Value="reader">Reader</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <!-- Assigned groups dropdown list -->
        <asp:Panel ID="pnlSelectGroup" runat="server" Visible="false">
            <div class="form-group">
                <asp:Label runat="server" Text="Group:" CssClass="control-label col-xs-1" AssociatedControlID="ddlAssignedGroups"></asp:Label>
                <div class="col-xs-11" style="margin-top: 10px">
                    <asp:DropDownList ID="ddlAssignedGroups" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlAssignedGroups_SelectedIndexChanged"
                        CausesValidation="True">
                    </asp:DropDownList>
                </div>
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Please select a group." ControlToValidate="ddlAssignedGroups"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ToolTip="Groups" InitialValue="none selected"></asp:RequiredFieldValidator>
                </div>
                <!-- Select assigned groups error/feedback message -->
                <div class="col-xs-offset-1 col-xs-11">
                    <asp:Label ID="lblSelectAssignedGroupsMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <!-- No assigned groups feedback message -->
        <div class="form-group">
            <div class="col-xs-offset-2 col-xs-10">
                <asp:Label ID="lblNoAssignedGroupMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="pnlStudentGrades" runat="server" Visible="false">
            <!-- Display FYP title -->
            <div class="form-group">
                <asp:Label runat="server" Text="FYP Title:" CssClass="control-label col-xs-1" AssociatedControlID="txtTitle"></asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-static" Visible="false" ReadOnly="True" BackColor="White"
                        BorderStyle="None" BorderWidth="0px" Width="100%"></asp:TextBox>
                    <!-- FYP title error message -->
                    <div style="margin-top: 10px">
                        <asp:Label ID="lblTitleMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
            <!-- Display student grades -->
            <div class="form-group">
                <div class="col-xs-12">
                    <asp:GridView ID="gvStudentGrades" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                        OnRowCancelingEdit="GvStudentGrades_RowCancelingEdit" OnRowEditing="GvStudentGrades_RowEditing" OnRowUpdating="GvStudentGrades_RowUpdating"
                        OnRowDataBound="GvStudentGrades_RowDataBound" AutoGenerateEditButton="True" DataKeyNames="NAME">
                        <HeaderStyle Wrap="False" />
                    </asp:GridView>
                </div>
                <!-- Student grades error/feedback message -->
                <div class="col-xs-12" style="margin-top: 10px">
                    <asp:Label ID="lblStudentGradesResultMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
                <!-- Edit grades error messages -->
                <div class="col-xs-12" style="margin-top: 10px">
                    <div>
                        <asp:Label ID="lblProposalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblProgressGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblFinalGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblPresentationGradeErrorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

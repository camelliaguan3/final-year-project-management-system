<%@ Page Title="Create FYP" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateFYP.aspx.cs" Inherits="FYPMSWebsite.Faculty.CreateFYP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>Create FYP</strong></span></h4>
        <asp:Panel ID="pnlFYPInfo" runat="server">
            <div class="form-group">
                <!-- Title -->
                <asp:Label runat="server" Text="Title:" CssClass="control-label col-xs-2" AssociatedControlID="txtTitle"></asp:Label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="The title field is required." ControlToValidate="txtTitle"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <!-- Description -->
                <asp:Label runat="server" Text="Description:" CssClass="control-label col-xs-2" AssociatedControlID="txtDescription"></asp:Label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control input-sm" MaxLength="1200" TextMode="MultiLine" Height="100"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="The description field is required." ControlToValidate="txtDescription"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <!-- Cosupervisor -->
                <asp:Label runat="server" Text="Cosupervisor:" CssClass="control-label col-xs-2" AssociatedControlID="ddlCosupervisor"></asp:Label>
                <div class="col-xs-10">
                    <asp:DropDownList ID="ddlCosupervisor" runat="server" CssClass="input-sm dropdown"></asp:DropDownList>
                    <div>
                        <asp:Label ID="lblSelectCosupervisorMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <!-- Category -->
                <asp:Label runat="server" Text="Category:" CssClass="control-label col-xs-2" AssociatedControlID="txtCategory"></asp:Label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control input-sm" placeholder="Input a new category or select an existing category" MaxLength="30"></asp:TextBox>
                </div>
                <div class="col-xs-4">
                    <asp:DropDownList ID="DdlCategory" runat="server" CssClass="input-sm dropdown" AutoPostBack="true" OnSelectedIndexChanged="DdlCategory_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-xs-offset-2 col-xs-10">
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Please input or select a category." ControlToValidate="txtCategory"
                        CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ToolTip="Category"></asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-offset-6 col-xs-6">
                    <asp:Label ID="lblSelectCategoryMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <!-- Project type -->
                <asp:Label runat="server" Text="Type:" CssClass="control-label col-xs-2" AssociatedControlID="rblType"></asp:Label>
                <div class="col-xs-10">
                    <asp:RadioButtonList ID="rblType" runat="server" CssClass="" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="RblType_SelectedIndexChanged">
                        <asp:ListItem class="radio-inline" Value="project" Selected="True">Project</asp:ListItem>
                        <asp:ListItem class="radio-inline" Value="thesis">Thesis</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group">
                <!-- Other requirements -->
                <asp:Label runat="server" Text="Requirements:" CssClass="control-label col-xs-2" AssociatedControlID="txtOtherRequirements"></asp:Label>
                <div class="col-xs-10">
                    <asp:TextBox ID="txtOtherRequirements" runat="server" CssClass="form-control input-sm" MaxLength="200" Wrap="False"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <!-- Minimum number of students -->
                <asp:Label runat="server" Text="Minimum students in this FYP (1 if individual):" CssClass="control-label col-xs-5"
                    AssociatedControlID="rblMinStudents"></asp:Label>
                <div class="col-xs-7">
                    <asp:RadioButtonList ID="rblMinStudents" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="RblMinStudents_SelectedIndexChanged">
                        <asp:ListItem class="radio-inline" Selected="True">1</asp:ListItem>
                        <asp:ListItem class="radio-inline">2</asp:ListItem>
                        <asp:ListItem class="radio-inline">3</asp:ListItem>
                        <asp:ListItem class="radio-inline">4</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group">
                <!-- Maximum number of students -->
                <asp:Label runat="server" Text="Maximum students in this FYP (1 if individual):" CssClass="control-label col-xs-5"
                    AssociatedControlID="rblMaxStudents"></asp:Label>
                <div class="col-xs-7">
                    <asp:RadioButtonList ID="rblMaxStudents" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="RblMaxStudents_SelectedIndexChanged">
                        <asp:ListItem class="radio-inline" Selected="True">1</asp:ListItem>
                        <asp:ListItem class="radio-inline">2</asp:ListItem>
                        <asp:ListItem class="radio-inline">3</asp:ListItem>
                        <asp:ListItem class="radio-inline">4</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-2">
                    <asp:CompareValidator runat="server" ErrorMessage="The minimum number of students cannot be greater than the maximum number of students."
                        ControlToCompare="rblMinStudents" ControlToValidate="rblMaxStudents" Operator="GreaterThanEqual" CssClass="text-danger"
                        Display="Dynamic" EnableClientScript="False" ID="CvMinMaxStudents"></asp:CompareValidator>
                </div>
            </div>
            <div class="form-group">
                <!-- FYP status -->
                <asp:Label runat="server" Text="Status:" CssClass="control-label col-xs-2" AssociatedControlID="rblStatus"></asp:Label>
                <div class="col-xs-4">
                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem class="radio-inline" Value="available" Selected="True">available</asp:ListItem>
                        <asp:ListItem class="radio-inline" Value="unavailable">unavailable</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group">
                <!-- Create/cancel buttons -->
                <div class="col-xs-offset-2 col-xs-2">
                    <asp:Button ID="btnCreate" runat="server" Text="Create FYP" CssClass="btn-sm" OnClick="BtnCreate_Click" Enabled="False" />
                </div>
                <!-- Error/feedback message -->
                <div class="col-xs-offset-2 col-xs-10" style="margin-top: 10px">
                    <asp:Label ID="lblCreateMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

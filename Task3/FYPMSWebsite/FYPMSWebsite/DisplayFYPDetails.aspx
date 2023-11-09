<%@ Page Title="FYP Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayFYPDetails.aspx.cs" Inherits="FYPMSWebsite.DisplayFYPDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>FYP Details</strong></span></h4>
        <div class="form-group">
            <!-- Title -->
            <asp:Label runat="server" Text="Title:" CssClass="control-label col-xs-2" AssociatedControlID="txtTitle"></asp:Label>
            <div class="col-xs-10">
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None"
                    BorderWidth="0px" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <!-- Description -->
            <asp:Label runat="server" Text="Description:" CssClass="control-label col-xs-2" AssociatedControlID="txtDescription"></asp:Label>
            <div id="description" class="col-xs-10">
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None"
                    TextMode="MultiLine" BorderWidth="0px" Height="150px" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <!-- Supervisor -->
            <asp:Label runat="server" Text="Supervisor(s):" CssClass="control-label col-xs-2" AssociatedControlID="txtSupervisor"></asp:Label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtSupervisor" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
            </div>
            <!-- Category -->
            <asp:Label runat="server" Text="Category:" CssClass="control-label col-xs-1" AssociatedControlID="txtCategory"></asp:Label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
            </div>
            <!-- Project type -->
            <asp:Label runat="server" Text="Type:" CssClass="control-label col-xs-1" AssociatedControlID="txtType"></asp:Label>
            <div class="col-xs-2">
                <asp:TextBox ID="txtType" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
            </div>
        </div>
        <!-- Error/feedback message for supervisor information -->
        <asp:Label ID="lblGetSupervisorsMessage" runat="server" CssClass="label-info col-xs-offset-2" Visible="False" BackColor="Transparent"></asp:Label>
        <div class="form-group">
            <!-- Other Requirements -->
            <asp:Label runat="server" Text="Requirements:" CssClass="control-label col-xs-2" AssociatedControlID="txtOtherRequirements"></asp:Label>
            <div class="col-xs-10">
                <asp:TextBox ID="txtOtherRequirements" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <!-- Minimum number of students -->
            <asp:Label runat="server" Text="Minimum&nbsp;students:" CssClass="control-label col-xs-offset-2 col-xs-2" AssociatedControlID="txtMinStudents"></asp:Label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtMinStudents" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None"></asp:TextBox>
            </div>
            <!-- Maximum number of students -->
            <asp:Label runat="server" Text="Maximum&nbsp;students:" CssClass="control-label col-xs-2" AssociatedControlID="txtMaxStudents"></asp:Label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtMaxStudents" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <!-- Status -->
            <asp:Label runat="server" Text="Status:" CssClass="control-label col-xs-2" AssociatedControlID="txtStatus"></asp:Label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control-static" ReadOnly="True" BackColor="White" BorderStyle="None"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <!-- Hyperlink to FYP digests webpage -->
            <asp:LinkButton ID="btnReturn" runat="server" CssClass="col-xs-2 text-right" OnClick="BtnReturn_Click"><<< Go Back</asp:LinkButton>
            <!-- Error/feedback message for FYP information -->
            <asp:Label ID="lblGetFYPDetailsMessage" runat="server" CssClass="label-info col-xs-10" Visible="false" BackColor="Transparent"></asp:Label>
        </div>
    </div>
</asp:Content>

<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FYPMSWebsite.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h4><span style="text-decoration: underline; color: #191970" class="h4"><strong><%: Title %></strong></span></h4>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Username" CssClass="col-xs-1 control-label">Username</asp:Label>
            <div class="col-xs-2">
                <asp:TextBox runat="server" ID="Username" CssClass="form-control input-sm" />
            </div>
            <div class="col-xs-9">
                <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn-sm" />
            </div>
            <div class="col-xs-offset-1 col-xs-11">
                <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ControlToValidate="Username" CssClass="text-danger" ErrorMessage="A username is required." Display="Dynamic" />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
            </div>
        </div>
        <asp:Panel ID="pnlPassword" runat="server" Visible="false">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-xs-1 control-label">Password</asp:Label>
                <div class="col-xs-11">
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" Text="FYProject1#" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." Display="Dynamic" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FYPMSWebsite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <!-- Display splashscreen when no error message -->
        <asp:Panel ID="pnlSplashscreen" runat="server">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <div style="background: transparent !important" class="jumbotron">
                <div style="text-align: center">
                    <asp:Image runat="server" ImageUrl="~/Images/FYP.jpg" ImageAlign="Middle" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

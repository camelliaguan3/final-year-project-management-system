<%@ Page Title="FYP Digests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayFYPDigests.aspx.cs" Inherits="FYPMSWebsite.DisplayFYPDigests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>FYP Information</strong></span></h4>
        <div class="form-group">
            <!-- FYP digests -->
            <asp:Panel ID="pnlFYPInfo" runat="server">
                <div class="col-xs-12">
                    <asp:GridView ID="gvFYPs" runat="server" Visible="false" CssClass="table-condensed" OnRowDataBound="GvFYPs_RowDataBound"
                        AllowPaging="True" OnPageIndexChanging="GvFYPs_PageIndexChanging" PageSize="15" AllowSorting="True" OnSorting="GvFYPs_Sorting">
                    </asp:GridView>
                </div>
            </asp:Panel>
            <!-- Error/feedback message -->
            <div class="col-xs-12">
                <asp:Label ID="lblDisplayFYPDigestsMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

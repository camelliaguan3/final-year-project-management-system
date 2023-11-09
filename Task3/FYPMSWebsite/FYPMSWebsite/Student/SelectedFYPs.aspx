<%@ Page Title="Selected FYPs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectedFYPs.aspx.cs" Inherits="FYPMSWebsite.Student.SelectedFYPs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>FYPs For Which Your Group Has Indicated An Interest</strong></span></h4>
        <!-- Display selected FYPs -->
        <div class="form-group">
            <div class="col-xs-12" style="margin-bottom: 10px">
                <asp:GridView ID="gvSelectedProjects" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                    OnRowDataBound="GvSelectedProjects_RowDataBound">
                    <Columns>
                        <asp:HyperLinkField Text="Details" DataNavigateUrlFields="FYPID" NavigateUrl="../DisplayFYPDetails.aspx"
                            DataNavigateUrlFormatString="../DisplayFYPDetails.aspx?fypId={0}&returnUrl=~/Student/SelectedFYPs.aspx" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-xs-12">
                <asp:Label ID="lblSelectedFYPsMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
        <!-- Hyperlink to available FYPs -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:HyperLink runat="server" NavigateUrl="~/Student/AvailableFYPs.aspx">Show Available FYPs</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

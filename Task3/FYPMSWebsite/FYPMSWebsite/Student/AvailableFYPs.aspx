<%@ Page Title="Available FYPs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AvailableFYPs.aspx.cs" Inherits="FYPMSWebsite.Student.AvailableFYPs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>FYPs For Which Your Group Can Indicate An Interest</strong></span></h4>
        <!-- Display available FYPs -->
        <div class="form-group">
            <div class="col-xs-12" style="margin-bottom: 10px">
                <asp:GridView ID="gvFYPsAvailableForSelection" runat="server" CssClass="table-condensed" Visible="false" BorderStyle="Solid" CellPadding="0"
                    OnRowDataBound="GvFYPsAvailableForSelection_RowDataBound">
                    <Columns>
                        <asp:HyperLinkField Text="Details" DataNavigateUrlFields="FYPID" NavigateUrl="../DisplayFYPDetails.aspx"
                            DataNavigateUrlFormatString="../DisplayFYPDetails.aspx?fypId={0}&returnUrl=~/Student/AvailableFYPs.aspx" />
                        <asp:TemplateField HeaderText="PRIORITY">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPriority" runat="server">
                                    <asp:ListItem>Select</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Error.feedback message -->
            <div class="col-xs-12">
                <div>
                    <asp:Label ID="lblAvailableForSelectionMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblGroupAssignedMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlUpdateButton" runat="server" Visible="false">
            <!-- Update button -->
            <div class="form-group">
                <div class="col-xs-2">
                    <asp:Button ID="btnUpdateFYPInterest" runat="server" Text="Update FYP Interest" CssClass="btn-sm" OnClick="BtnUpdateFYPInterest_Click" />
                </div>
                <div class="col-xs-10" style="margin-top: 6px">
                    <asp:Label ID="lblUpdateFYPInterestMessage" runat="server" CssClass="label-info" Visible="false" BackColor="Transparent"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <!-- Hyperlink to selected projects -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:HyperLink runat="server" NavigateUrl="~/Student/SelectedFYPs.aspx">Show Selected FYPs</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

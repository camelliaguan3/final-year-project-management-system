<%@ Page Title="Display/Edit FYP" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayEditFYPs.aspx.cs" Inherits="FYPMSWebsite.Faculty.DisplayEditFYPs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #00008B" class="h4"><strong>My FYPs</strong></span></h4>
        <!-- FYP information -->
        <asp:Panel ID="pnlFYPInfo" runat="server" Visible="false">
            <div class="form-group">
                <div class="col-xs-12">
                    <asp:GridView ID="gvFYPs" runat="server" CssClass="table-condensed" OnRowDataBound="GvFYPs_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="editHyperLink" runat="server" NavigateUrl='<%# Eval("FYPID", "EditFYP.aspx?fypId={0}") %>' Text="Edit"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <!-- Error/feedback message -->
        <div class="form-group">
            <div class="col-xs-12">
                <asp:Label ID="lblDisplayFYPsMessage" runat="server" CssClass="label-info" Visible="False" BackColor="Transparent"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite
{
    public partial class DisplayFYPDigests : System.Web.UI.Page
    {
        //***************
        // Uses TODO 01 *
        //***************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        /***** Private Methods *****/

        private void PopulateFYPDigests() // Uses TODO 01
        {
            //********************************************************************
            // Uses TODO 01 to populate a GridView with the digests of all FYPs. *
            //********************************************************************
            DataTable dtFYPs = myFYPMSDB.GetFYPDigests();
            if (myHelpers.PopulateGridView("TODO 01",
                                           gvFYPs,
                                           dtFYPs,
                                           new List<string> { "FYPID", "TITLE", "CATEGORY",
                                                   "TYPE", "MINSTUDENTS", "MAXSTUDENTS", "STATUS" },
                                           lblDisplayFYPDigestsMessage,
                                           lblDisplayFYPDigestsMessage,
                                           $"{queryError}TODO 01{queryErrorNoRecordsRetrieved}"))
            {
                if (!isEmptyQueryResult)
                {
                    CurrentSortDirection = SortDirection.Ascending;
                    ViewState["dtFYPs"] = dtFYPs;
                    pnlFYPInfo.Visible = true;
                }
            }
        }

        private SortDirection CurrentSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            { ViewState["sortDirection"] = value; }
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                PopulateFYPDigests();
            }
        }

        protected void GvFYPs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // GridView columns: 0-FYPID, 1-TITLE, 2-CATEGORY, 3-TYPE, 4-MINSTUDENTS, 5-MAXSTUDENTS, 6-STATUS
                int fypIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "FYPID", lblDisplayFYPDigestsMessage);             // index 0
                int titleColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "TITLE", lblDisplayFYPDigestsMessage);             // index 1
                int categoryColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "CATEGORY", lblDisplayFYPDigestsMessage);       // index 2
                int typeColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "TYPE", lblDisplayFYPDigestsMessage);               // index 3
                int minStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "MINSTUDENTS", lblDisplayFYPDigestsMessage); // index 4
                int maxStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "MAXSTUDENTS", lblDisplayFYPDigestsMessage); // index 5
                int statusColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 01", "STATUS", lblDisplayFYPDigestsMessage);           // index 6

                if (fypIdColumn != -1 && titleColumn != -1 && categoryColumn != -1 && typeColumn != -1 && minStudentsColumn != -1
                    && maxStudentsColumn != -1 && statusColumn != -1)
                {
                    e.Row.Cells[fypIdColumn].Visible = false;

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnSortMin = (LinkButton)e.Row.Cells[minStudentsColumn].Controls[0];
                        btnSortMin.Text = "MIN";
                        LinkButton btnSortMax = (LinkButton)e.Row.Cells[maxStudentsColumn].Controls[0];
                        btnSortMax.Text = "MAX";
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        // Change the FYP title to a hyperlink to the display FYP details page.
                        var titleCell = e.Row.Cells[titleColumn];
                        titleCell.Controls.Clear();
                        titleCell.Controls.Add(new HyperLink
                        {
                            NavigateUrl = "~/DisplayFYPDetails.aspx?fypId="
                                          + e.Row.Cells[fypIdColumn].Text,
                            Text = titleCell.Text
                        });
                        e.Row.Cells[minStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[maxStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[statusColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }

        protected void GvFYPs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFYPs.PageIndex = e.NewPageIndex;
            gvFYPs.DataSource = ViewState["dtFYPs"];
            gvFYPs.DataBind();
        }

        protected void GvFYPs_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnToSort = e.SortExpression; // Get the column name.
            gvFYPs.PageIndex = 0;

            // Determine the current sort direction and reverse the direction.
            if (CurrentSortDirection == SortDirection.Ascending)
            {
                CurrentSortDirection = SortDirection.Descending;
                ViewState["dtFYPs"] = myHelpers.SortGridview(gvFYPs,
                                                             (DataTable)ViewState["dtFYPs"],
                                                             columnToSort,
                                                             DESCENDING);
            }
            else // Previous sort direction was descending, so change to ascending. 
            {
                CurrentSortDirection = SortDirection.Ascending;
                ViewState["dtFYPs"] = myHelpers.SortGridview(gvFYPs,
                                                             (DataTable)ViewState["dtFYPs"],
                                                             columnToSort,
                                                             ASCENDING);
            }
        }
    }
}
using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;


namespace FYPMSWebsite.Faculty
{
    public partial class DisplayEditFYPs : System.Web.UI.Page
    {
        //************************
        // Uses TODO 05, TODO 06 *
        //************************
        private FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private HelperMethods myHelpers = new HelperMethods();
        readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;

        /***** Private Methods *****/

        private bool IsInterestIndicatedInFYP(string fypId) // Uses TODO 06
        {
            bool result = false;

            if (!isSqlError && !isQueryError)
            {
                //********************************************************************************************************
                // Uses TODO 06 to get the information of the groups that have indicated an interest in a specified FYP. *
                //********************************************************************************************************
                DataTable dtInterestedIn = myFYPMSDB.GetInterestedInFYP(fypId);

                if (myHelpers.IsQueryResultValid("TODO 06",
                                                 dtInterestedIn,
                                                 new List<string> { "FYPID", "GROUPID", "PRIORITY" },
                                                 lblDisplayFYPsMessage))
                {
                    if (dtInterestedIn.Rows.Count != 0)
                    {
                        // Interest in the FYP has been indicated if a tuple exists in the query result.
                        result = true;
                    }
                }
            }

            return result;
        }

        private void PopulateSupervisorFYPs() // Uses TODO 05
        {
            //*************************************************************************
            // Uses TODO 05 to populate a GridView with the FYP digests of a faculty. *
            //*************************************************************************
            if (myHelpers.PopulateGridView("TODO 05",
                                           gvFYPs,
                                           myFYPMSDB.GetSupervisorFYPDigest(loggedinUsername),
                                           new List<string> { "FYPID", "TITLE", "CATEGORY", "TYPE",
                                               "MINSTUDENTS", "MAXSTUDENTS", "STATUS" },
                                           lblDisplayFYPsMessage,
                                           lblDisplayFYPsMessage,
                                           noFYPsPosted))
            {
                if (!isEmptyQueryResult)
                {
                    pnlFYPInfo.Visible = true;
                }
            }
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                PopulateSupervisorFYPs();
            }
        }

        protected void GvFYPs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 8)
            {
                // Offset by 1 due to Edit/Details column.
                // GridView columns: 0-Edit/Details hyperlink, 1-FYPID, 2-TITLE, 3-CATEGORY, 4-TYPE, 5-MINSTUDENTS, 6-MAXSTUDENTS, 7-STATUS
                int hyperlinkColumn = 0;
                int fypIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 05", "FYPID", lblDisplayFYPsMessage) + 1;             // index 1
                int minStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 05", "MINSTUDENTS", lblDisplayFYPsMessage) + 1; // index 5
                int maxStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 05", "MAXSTUDENTS", lblDisplayFYPsMessage) + 1; // index 6

                if (fypIdColumn != 0 && minStudentsColumn != 0 && maxStudentsColumn != 0)
                {
                    e.Row.Cells[fypIdColumn].Visible = false;
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[hyperlinkColumn].Text = "ACTION";
                        myHelpers.RenameGridViewColumn(e, "MINSTUDENTS", "MIN");
                        myHelpers.RenameGridViewColumn(e, "MAXSTUDENTS", "MAX");
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (IsInterestIndicatedInFYP(e.Row.Cells[fypIdColumn].Text))
                        {
                            // Change the Edit hyperlink to a Details hyperlink.
                            HyperLink editHyperLink = (HyperLink)e.Row.FindControl("editHyperLink");
                            editHyperLink.NavigateUrl = $"~/DisplayFYPDetails.aspx?fypId={e.Row.Cells[fypIdColumn].Text}";
                            editHyperLink.Text = "Details";
                        }
                        // Disable the hyperlink if there was an SQL error.
                        if (isSqlError || isQueryError)
                        {
                            e.Row.Cells[hyperlinkColumn].Text = "Details/Edit";
                        }
                        e.Row.Cells[minStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[maxStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
    }
}
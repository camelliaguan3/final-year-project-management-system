using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Student
{
    public partial class SelectedFYPs : System.Web.UI.Page
    {
        //************************
        // Uses TODO 28, TODO 33 *
        //************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private string groupId;

        /***** Private Methods *****/

        private bool GetStudentGroupId(string username) // Uses TODO 28
        {
            bool result = false;

            //********************************************************************************
            // Uses TODO 28 to get the group id of the group of which a student is a member. *
            //********************************************************************************
            DataTable dtGroup = myFYPMSDB.GetStudentGroupId(username);

            if (myHelpers.IsQueryResultValid("TODO 28",
                                             dtGroup,
                                             new List<string> { "GROUPID" },
                                             lblSelectedFYPsMessage))
            {
                if (dtGroup.Rows.Count == 0) // No record was retrieved -> the student is not a member of a group.
                {
                    groupId = string.Empty;
                    result = true;
                }
                else if (dtGroup.Rows.Count == 1) // Only one record was retrieved -> the student is a member of a group.
                {
                    groupId = dtGroup.Rows[0]["GROUPID"].ToString();
                    result = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblSelectedFYPsMessage, $"{queryError}TODO 28{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }

            return result;
        }

        private void PopulateSelectedFYPs(string groupId) // Uses TODO 33
        {
            //************************************************************************************************
            // Uses TODO 33 to populate a GridView with the FYPs in which a group has indicated an interest. *
            //************************************************************************************************
            if (myHelpers.PopulateGridView("TODO 33",
                                           gvSelectedProjects,
                                           myFYPMSDB.GetFYPsGroupHasIndicatedInterestIn(groupId),
                                           new List<string> { "FYPID", "TITLE", "CATEGORY", "TYPE", "PRIORITY", "STATUS" },
                                           lblSelectedFYPsMessage,
                                           lblSelectedFYPsMessage,
                                           noFYPInterestIndicated))
            {; } // No action needed.
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;

                if (GetStudentGroupId(loggedinUsername))
                {
                    // If the group id is empty, the student is not yet a member of any project group.
                    if (groupId != string.Empty)
                    {
                        PopulateSelectedFYPs(groupId);
                    }
                    else if (!isQueryError)
                    {
                        myHelpers.DisplayMessage(lblSelectedFYPsMessage, $"{notGroupMember}<br/>{createOrJoinGroup}{toIndicateInterest}");
                    }
                }
            }
        }

        protected void GvSelectedProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // Offset by 1 due to Details column.
                // GridView columns: 0-Details hyperlink, 1-FYPID, 2-TITLE, 3-CATEGORY, 4-TYPE, 5-PRIORITY, 6-STATUS
                int fypIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 33", "FYPID", lblSelectedFYPsMessage) + 1;       // index 1
                int typeColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 33", "TYPE", lblSelectedFYPsMessage) + 1;         // index 2
                int priorityColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 33", "PRIORITY", lblSelectedFYPsMessage) + 1; // index 5
                int statusColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 33", "STATUS", lblSelectedFYPsMessage) + 1;     // index 6

                if (fypIdColumn != 0 && typeColumn != 0 && priorityColumn != 0 && statusColumn != 0)
                {
                    e.Row.Cells[fypIdColumn].Visible = false;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[typeColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[priorityColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[statusColumn].HorizontalAlign = HorizontalAlign.Center;
                        if (e.Row.Cells[statusColumn].Text.Trim() == "unavailable")
                        {
                            e.Row.Cells[statusColumn].ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }
    }
}
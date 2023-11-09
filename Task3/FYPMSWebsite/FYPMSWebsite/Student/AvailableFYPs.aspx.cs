using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Student
{
    public partial class AvailableFYPs : System.Web.UI.Page
    {
        //***************************************************
        // Uses TODO 28, TODO 29, TODO 30, TODO 31, TODO 32 *
        //***************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private string groupId;

        /***** Private Methods *****/

        private void PopulateFYPsAvailableToSelect(string groupId) // Uses TODO 30
        {
            //**************************************************************************
            // Uses TODO 30 to get the FYPs in which a group can indicate an interest. *
            //**************************************************************************
            if (myHelpers.PopulateGridView("TODO 30",
                                           gvFYPsAvailableForSelection,
                                           myFYPMSDB.GetGroupAvailableFYPDigests(groupId),
                                           new List<string> { "FYPID", "TITLE", "CATEGORY", "TYPE", "MINSTUDENTS", "MAXSTUDENTS" },
                                           lblAvailableForSelectionMessage,
                                           lblAvailableForSelectionMessage,
                                           noFYPsToIndicateInterest))
            {
                if (!isEmptyQueryResult)
                {
                    ViewState["groupId"] = groupId;
                    pnlUpdateButton.Visible = true;
                }
            }
        }

        private bool GetStudentGroupId(string username) // Uses TODO 28
        {
            bool result = false;

            //****************************************************************************
            // Uses TODO 28 to get the group id of the group to which a student belongs. *
            //****************************************************************************
            DataTable dtGroup = myFYPMSDB.GetStudentGroupId(username);

            if (myHelpers.IsQueryResultValid("TODO 28",
                                             dtGroup,
                                             new List<string> { "GROUPID" },
                                             lblAvailableForSelectionMessage))
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
                    myHelpers.DisplayMessage(lblAvailableForSelectionMessage, $"{queryError}TODO 28{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }

            return result;
        }

        private bool IsGroupAssignedToFYP(string groupId) // Uses TODO 29
        {
            bool result = false;

            //************************************************************
            // Uses TODO 29 to get the FYP to which a group is assigned. *
            //************************************************************
            DataTable dtIsAssigned = myFYPMSDB.GetAssignedFYPId(groupId);

            if (myHelpers.IsQueryResultValid("TODO 29",
                                             dtIsAssigned,
                                             new List<string> { "ASSIGNEDFYPID" },
                                             lblAvailableForSelectionMessage))
            {
                if (dtIsAssigned.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblAvailableForSelectionMessage, $"{queryError}TODO 29{queryErrorNoRecordsRetrieved}");
                    isQueryError = true;
                }
                else if (dtIsAssigned.Rows.Count == 1) // Only one record was retrieved -> check if the group is assigned.
                {
                    if (dtIsAssigned.Rows[0]["ASSIGNEDFYPID"].ToString() != string.Empty)
                    {
                        myHelpers.DisplayMessage(lblAvailableForSelectionMessage, cannotIndicateInterest);
                        SetFYPTitle(groupId);
                        result = true;
                    }
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblAvailableForSelectionMessage, $"{queryError}TODO 29{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }

            return result;
        }

        private void SetFYPTitle(string groupId) // Uses TODO 31
        {
            //*************************************************************************
            // Uses TODO 31 to get the title of the FYP to which a group is assigned. *
            //*************************************************************************
            DataTable dtAssignedFYPTitle = myFYPMSDB.GetAssignedFYPTitle(groupId);

            if (myHelpers.IsQueryResultValid("TODO 31",
                                             dtAssignedFYPTitle,
                                             new List<string> { "TITLE" },
                                             lblGroupAssignedMessage))
            {
                if (dtAssignedFYPTitle.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, $"{queryError}TODO 31{queryErrorNoRecordsRetrieved}");
                }
                else if (dtAssignedFYPTitle.Rows.Count == 1) // Only one record was retrieved -> set the FYP title.
                {
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, $"{groupAssignedToFYP}{dtAssignedFYPTitle.Rows[0]["TITLE"]}.");
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, $"{queryError}TODO 31{queryErrorMultipleRecordsRetrieved}");
                }
            }
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
                        bool isAssigned = IsGroupAssignedToFYP(groupId);
                        if (!isSqlError & !isQueryError)
                        {
                            if (!isAssigned)
                            {
                                // The group is not assigned to a project.
                                PopulateFYPsAvailableToSelect(groupId);
                            }
                        }
                    }
                    else if (!isQueryError)
                    {
                        myHelpers.DisplayMessage(lblAvailableForSelectionMessage, $"{notGroupMember}<br/>{createOrJoinGroup}{toIndicateInterest}");
                    }
                }
            }
        }

        protected void BtnUpdateFYPInterest_Click(object sender, EventArgs e) // Uses TODO 32
        {
            if (IsValid)
            {
                string groupId = ViewState["groupId"].ToString();
                string fypId;
                string priority;
                bool fypSelected = false;

                // For each FYP for which a priority has been specified, get the priority and update the InterestedIn table.
                for (int i = 0; i < gvFYPsAvailableForSelection.Rows.Count; i++)
                {
                    DropDownList ddlPriority = (DropDownList)gvFYPsAvailableForSelection.Rows[i].FindControl("ddlPriority");
                    fypId = gvFYPsAvailableForSelection.Rows[i].Cells[2].Text; // Column 2 is the fyp id.
                    if (ddlPriority.SelectedIndex != 0)
                    {
                        priority = ddlPriority.SelectedItem.Value;
                        fypSelected = true;

                        //******************************************************************
                        // Uses TODO 32 to update the FYPs in which a group is interested. *
                        //******************************************************************
                        if (!myFYPMSDB.IndicateInterestInFYP(fypId, groupId, priority))
                        {
                            // An SQL error occurred -> exit the update.
                            myHelpers.DisplayMessage(lblUpdateFYPInterestMessage, sqlErrorMessage);
                            btnUpdateFYPInterest.Enabled = false;
                            return;
                        }
                    }
                }

                // Determine if any FYP was selected.
                if (fypSelected)
                {
                    Response.Redirect(selectedFYPsUrl);
                }
                else // No FYP was selected.
                {
                    myHelpers.DisplayMessage(lblUpdateFYPInterestMessage, noFYPSelected);
                }
            }
        }

        protected void GvFYPsAvailableForSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 8)
            {
                // Offset by 2 due to Details and Priority columns.
                // GridView columns: 0-Details hyperlink, 1-Priority dropdown list, 2-FYPID, 3-TITLE, 4-CATEGORY, 5-TYPE, 6-MINSTUDENTS, 7-MAXSTUDENTS
                int fypIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 30", "FYPID", lblAvailableForSelectionMessage) + 2;             // index 2
                int minStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 30", "MINSTUDENTS", lblAvailableForSelectionMessage) + 2; // index 6
                int maxStudentsColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 30", "MAXSTUDENTS", lblAvailableForSelectionMessage) + 2; // index 7

                if (fypIdColumn != 1 && minStudentsColumn != 1 && maxStudentsColumn != 1)
                {
                    e.Row.Cells[fypIdColumn].Visible = false;
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        myHelpers.RenameGridViewColumn(e, "MINSTUDENTS", "MIN");
                        myHelpers.RenameGridViewColumn(e, "MAXSTUDENTS", "MAX");
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[minStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[maxStudentsColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
    }
}
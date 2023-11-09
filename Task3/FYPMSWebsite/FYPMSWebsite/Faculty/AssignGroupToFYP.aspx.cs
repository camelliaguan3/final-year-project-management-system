using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Faculty
{
    public partial class AssignGroupToFYP : System.Web.UI.Page
    {
        //******************************************************************************
        // Uses TODO 13, TODO 14, TODO 15, TODO 16, TODO 17, TODO 18, TODO 19, TODO 20 *
        //******************************************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private enum GroupType { assigned, unassigned };
        private bool isFypAvailable = false;

        /***** Private Methods *****/

        private string CreateGroupCode(string fypId) // Uses TODO 18, TODO 19
        {
            string groupCode = string.Empty;

            //**************************************************************************
            // Uses TODO 18 to get the faculty codes of all faculty supervising a FYP. *
            //**************************************************************************
            DataTable dtFacultyCodes = myFYPMSDB.GetFYPSupervisorFacultyCodes(fypId);

            if (myHelpers.IsQueryResultValid("TODO 18",
                                             dtFacultyCodes,
                                             new List<string> { "FACULTYCODE" },
                                             lblAssignGroupMessage))
            {
                if (dtFacultyCodes.Rows.Count == 0) // No faculty codes were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblAssignGroupMessage, $"{queryError}TODO 18{queryErrorNoRecordsRetrieved}");
                    lblAssignGroupMessage.Visible = true;
                }
                else
                {
                    // Add the group code prefix this project to the group code.
                    foreach (DataRow row in dtFacultyCodes.Rows)
                    {
                        groupCode += row["FACULTYCODE"].ToString().Trim();
                    }

                    //********************************************************************
                    // Uses TODO 19 to get the current sequence number for a group code. *
                    //********************************************************************
                    decimal sequenceNumber = myFYPMSDB.GetGroupCodePrefixCount(groupCode);

                    if (sequenceNumber != -1)
                    {
                        groupCode += (sequenceNumber + 1).ToString();
                    }
                    else // An SQL error occurred getting the current sequence number.
                    {
                        myHelpers.DisplayMessage(lblAssignGroupMessage, sqlErrorMessage);
                        groupCode = string.Empty;
                    }
                }
            }

            return groupCode;
        }

        private DataTable CreateGroupList(DataTable dtGroups, GroupType groupType)
        {
            // Create a new DataTable containing only one row for each project group.
            DataTable dtGroupList = new DataTable();

            // Columns for assigned groups:   GROUPID, GROUPCODE, PRIORITY, MEMBERS, READER
            // Columns for unassigned groups: GROUPID, PRIORITY, MEMBERS
            // Add columns to the DataTable according to the type of group, assigned or unassigned.
            dtGroupList.Columns.Add("GROUPID", typeof(string));
            if (groupType == GroupType.assigned) { dtGroupList.Columns.Add("GROUPCODE", typeof(string)); }
            dtGroupList.Columns.Add("PRIORITY", typeof(string));
            dtGroupList.Columns.Add("MEMBERS", typeof(string));
            if (groupType == GroupType.assigned) { dtGroupList.Columns.Add("READER", typeof(string)); }

            // Populate the DataTable if there are groups.
            if (dtGroups.Rows.Count > 0)
            {
                // Initialize the values for the DataTable row.
                string previousGroupId = string.Empty;
                string groupId = string.Empty;
                string groupCode = string.Empty;
                string priority = string.Empty;
                string members = string.Empty;
                string reader = string.Empty;

                // Create only one DataTable entry for each project group by concatenating
                // the names of group members into a list (first name last name, ...).
                foreach (DataRow row in dtGroups.Rows)
                {
                    groupId = row["GROUPID"].ToString();

                    if (previousGroupId != groupId && previousGroupId != string.Empty)
                    {
                        members = members.Remove(members.Length - 2); // Remove trailing comma and space separator.

                        if (groupType == GroupType.assigned) // Group type is assigned.
                        {
                            dtGroupList.Rows.Add(new object[] { previousGroupId, groupCode, priority, members, reader });
                        }
                        else // Group type is unassigned.
                        {
                            dtGroupList.Rows.Add(new object[] { previousGroupId, priority, members });
                        }
                        members = string.Empty;
                    }

                    if (groupType == GroupType.assigned)
                    {
                        groupCode = row["GROUPCODE"].ToString();
                    }

                    priority = row["PRIORITY"].ToString();
                    members = $"{members}{row["MEMBERS"]}, ";

                    if (groupType == GroupType.assigned)
                    {
                        reader = row["READER"].ToString();
                    }
                    previousGroupId = groupId;
                }

                members = members.Remove(members.Length - 2); // Remove trailing comma and space separator.

                if (groupType == GroupType.assigned) // Group type is assigned.
                {
                    dtGroupList.Rows.Add(new object[] { groupId, groupCode, priority, members, reader });
                }
                else // Group type is unassigned.
                {
                    dtGroupList.Rows.Add(new object[] { groupId, priority, members });
                }
            }

            return dtGroupList;
        }

        private double GetGroupSupervisionCount(string groupCode)
        {
            // Calculate the increment a group contributes to the supervison count of a faculty.
            // Sole supervison counts as 1; cosupervision counts as 0.5.

            double result = 0;

            // The group code length of sole supervised groups is exactly 3 (e.g., HJ1)
            // while that of cosupervised groups is exactly 5 (e.g., HJJN1).
            if (groupCode.Length == 3) // Sole supervised group.
            {
                result += 1;
            }
            else // Cosupervised group.
            {
                result += 0.5;
            }

            return result;
        }

        private bool IsFYPAvailable(string fypId) // Uses TODO 15
        {
            bool result = false;
            isSqlError = isQueryError = false;

            //************************************************************************
            // Uses TODO 15 to determine whether an FYP is available for assignment. *
            //************************************************************************
            DataTable dtFYPAvailability = myFYPMSDB.GetFYPStatus(fypId);

            if (myHelpers.IsQueryResultValid("TODO 15",
                                             dtFYPAvailability,
                                             new List<string> { "STATUS" },
                                             lblAvailableToAssignMessage))
            {
                if (dtFYPAvailability.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    isQueryError = true;
                    myHelpers.DisplayMessage(lblAvailableToAssignMessage, $"{queryError}TODO 15{queryErrorNoRecordsRetrieved}");
                }
                else if (dtFYPAvailability.Rows.Count == 1) // Only one record was retrieved -> check the FYP status.
                {
                    if (dtFYPAvailability.Rows[0]["STATUS"].ToString().Trim() == "available") // The FYP is available.
                    {
                        result = true;
                    }
                    else // The FYP is not available.
                    {
                        myHelpers.DisplayMessage(lblSelectFYPMessage, $"{fypNotAvailable}");
                        lblSelectFYPMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else // Multiple records were retrieved -> query error.
                {
                    isQueryError = true;
                    myHelpers.DisplayMessage(lblAvailableToAssignMessage, $"{queryError}TODO 15{queryErrorMultipleRecordsRetrieved}");
                }
            }

            return result;
        }

        private void PopulateGroupsAvailableForAssignment(string fypId) // Uses TODO 16
        {
            pnlAssignButton.Visible = false;

            // Determine if the FYP is available. This value is also used in GvAvailableToAssign_RowDataBound.
            isFypAvailable = IsFYPAvailable(fypId);

            // If there was an SQL or query error in TODO 15 -> cannot determine availability of FYP.
            if (!isSqlError && !isQueryError)
            {
                //**************************************************************************
                // Uses TODO 16 to get the project groups available to assign to this FYP. *
                //**************************************************************************
                DataTable dtAvailableForAssignment = myFYPMSDB.GetGroupsAvailableToAssignToFYP(fypId);

                if (myHelpers.IsQueryResultValid("TODO 16",
                                                 dtAvailableForAssignment,
                                                 new List<string> { "GROUPID", "PRIORITY", "MEMBERS" },
                                                 lblAvailableToAssignMessage))
                {
                    // Populate a GridView with the groups available to assign to this FYP.
                    if (myHelpers.PopulateGridView("TODO 16",
                                                   gvAvailableToAssign,
                                                   CreateGroupList(dtAvailableForAssignment, GroupType.unassigned),
                                                   new List<string> { "GROUPID", "PRIORITY", "MEMBERS" },
                                                   lblAvailableToAssignMessage,
                                                   lblAvailableToAssignMessage,
                                                   noGroupsAvailable))
                    {
                        // The FYP is available for assignment if the query result was not empty AND its status is 'available'
                        // AND the faculty has not reached the maximum number of groups that can be supervised.
                        // numGroups is the number of groups the faculty is currently supervising.
                        // maxGroups is the maximum number of groups that can be supervised and is specified in Global.asax.cs.
                        if (!isEmptyQueryResult && isFypAvailable && (Convert.ToDouble(ViewState["numGroups"]) < maxGroups))
                        {
                            pnlAssignButton.Visible = true;
                        }
                    }
                }
            }
        }

        private void PopulateGroupsCurrentlyAssigned(string fypId) // Uses TODO 17
        {
            lblCurrentlyAssignedMessage.Visible = false;

            //**********************************************************************************************
            // Uses TODO 17 to populate a GridView with the groups currently assigned to a faculty's FYPs. *
            //**********************************************************************************************
            DataTable dtCurrentlyAssigned = myFYPMSDB.GetGroupsAssignedToFYP(fypId);

            if (myHelpers.IsQueryResultValid("TODO 17",
                                             dtCurrentlyAssigned,
                                             new List<string> { "GROUPID", "PRIORITY", "MEMBERS", "GROUPCODE", "READER" },
                                             lblCurrentlyAssignedMessage))
            {
                ViewState["dtCurrentlyAssigned"] = dtCurrentlyAssigned;

                // Populate a GridView with the groups currently assigned to this project.
                if (myHelpers.PopulateGridView("TODO 17",
                                               gvCurrentlyAssigned,
                                               CreateGroupList(dtCurrentlyAssigned, GroupType.assigned),
                                               new List<string> { "GROUPID", "GROUPCODE", "PRIORITY", "MEMBERS", "READER" },
                                               lblCurrentlyAssignedMessage,
                                               lblCurrentlyAssignedMessage,
                                               noGroupsAssigned))
                {; } // No action needed.
            }
        }

        private void PopulateFYPDropDownList() // Uses TODO 14
        {
            //**********************************************************************************
            // Uses TODO 14 to populate a dropdown list with the FYPs supervised by a faculty. *
            //**********************************************************************************
            if (!myHelpers.PopulateDropDownList("TODO 14",
                                                string.Empty,
                                                ddlFYPs,
                                                myFYPMSDB.GetFacultyFYPs(loggedinUsername),
                                                new List<string> { "FYPID", "TITLE" },
                                                lblSelectFYPMessage,
                                                lblAssignGroupMessage,
                                                noFYPsPosted,
                                                EmptyQueryResultMessageType.Information))
            {
                // There are no supervised FYPs if the query result is empty.
                if (isEmptyQueryResult)
                {
                    pnlSelectFYP.Visible = false;
                }
            }
        }

        private void ShowNumberOfGroupsAssigned() // Uses TODO 13
        {
            double numGroups = 0;

            //******************************************************************
            // Uses TODO 13 to get the project groups supervised by a faculty. *
            //******************************************************************
            DataTable dtProjectGroups = myFYPMSDB.GetSupervisorProjectGroups(loggedinUsername);

            if (myHelpers.IsQueryResultValid("TODO 13",
                                             dtProjectGroups,
                                             new List<string> { "GROUPID", "GROUPCODE", "ASSIGNEDFYPID" },
                                             lblNumberAssignedMessage))
            {
                {
                    // Calculate the number of groups supervised.
                    foreach (DataRow row in dtProjectGroups.Rows)
                    {
                        numGroups += GetGroupSupervisionCount(row["GROUPCODE"].ToString().Trim());
                    }
                }

                // Save the number of groups supervised for use when populating groups available for assignment.
                ViewState["numGroups"] = numGroups;

                // Construct the message to indicate the number of groups currently supervised.
                string supervisonMessage = $"{groupsSupervised}{numGroups}.";

                // Check if the maximum number of groups is already supervised.
                if (numGroups >= maxGroups) { supervisonMessage += $"&nbsp;{maxGroupsSupervised}"; }
                myHelpers.DisplayMessage(lblNumberAssignedMessage, supervisonMessage);
            }
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                ShowNumberOfGroupsAssigned();
                PopulateFYPDropDownList();
            }
        }

        protected void BtnAssignGroups_Click(object sender, EventArgs e) // Uses TODO 20
        {
            string fypId = ddlFYPs.SelectedValue;
            string groupId = string.Empty;
            double numGroups = (double)ViewState["numGroups"];
            lblNoGroupAssignedMessage.Visible = false;

            // Determine if any groups were selected for assignment to FYPs.
            foreach (GridViewRow row in gvAvailableToAssign.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.Cells[0].FindControl("chkSelected") is CheckBox chkRow && chkRow.Checked)
                    {
                        groupId = row.Cells[1].Text; // Get the group id in column 1.
                        string groupCode = CreateGroupCode(fypId); 

                        if (groupCode == string.Empty)
                        {
                            return; // An SQL error occurred if the groupCode is empty.
                        }

                        // Check if the maximum allowed number of supervised groups has been exceeded.
                        numGroups += GetGroupSupervisionCount(groupCode);
                        if (numGroups <= maxGroups)
                        {
                            //****************************************************
                            // Uses TODO 20 to assign a project group to an FYP. *
                            //****************************************************
                            if (!myFYPMSDB.AssignGroupToFYP(groupId, fypId, groupCode))
                            {
                                myHelpers.DisplayMessage(lblAssignGroupMessage, sqlErrorMessage);
                                return; // An SQL error occurred.
                            }
                        }
                        else // Assignment would exceed allowed number of supervised groups.
                        {
                            myHelpers.DisplayMessage(lblNoGroupAssignedMessage, $"{maxGroupSupervisonExceeded}{maxGroups}.");
                            break;
                        }
                    }
                }
            }

            // If the groupId is not empty, a project group was selected.
            if (groupId != string.Empty)
            {
                lblAssignGroupMessage.Visible = false;

                // Show the revised number of groups supervised and refresh the group assignments.
                ViewState["numGroups"] = numGroups;
                ShowNumberOfGroupsAssigned();
                PopulateGroupsCurrentlyAssigned(fypId);
                PopulateGroupsAvailableForAssignment(fypId);
            }
            else // No project group was selected.
            {
                myHelpers.DisplayMessage(lblNoGroupAssignedMessage, noGroupSelected);
            }
        }

        protected void DdlFYPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlGroupAssignment.Visible = lblSelectFYPMessage.Visible = lblNoGroupAssignedMessage.Visible = false;

            if (IsValid)
            {
                isSqlError = isQueryError = lblAssignGroupMessage.Visible = lblAvailableToAssignMessage.Visible
                    = lblCurrentlyAssignedMessage.Visible = false;
                PopulateGroupsAvailableForAssignment(ddlFYPs.SelectedValue);
                PopulateGroupsCurrentlyAssigned(ddlFYPs.SelectedValue);
                pnlGroupAssignment.Visible = true;
            }
        }

        protected void GvAvailableToAssign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 4)
            {
                // Offset by 1 due to Select column.
                // GridView columns: 0-Select hyperlink, 1-GROUPID, 2-PRIORITY, 3-MEMBERS
                int selectColumn = 0;
                int groupIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 16", "GROUPID", lblAssignGroupMessage) + 1;   // index 1
                int priorityColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 16", "PRIORITY", lblAssignGroupMessage) + 1; // index 2

                if (groupIdColumn != 0 && priorityColumn != 0)
                {
                    e.Row.Cells[groupIdColumn].Visible = false; // Hide the group id column.

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (isFypAvailable == false || (Convert.ToDouble(ViewState["numGroups"]) >= maxGroups))
                        {
                            ((CheckBox)e.Row.FindControl("chkSelected")).Enabled = false;
                        }

                        e.Row.Cells[selectColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[priorityColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }

        protected void GvCurrentlyAssigned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 5)
            {
                // GridView columns: 0-GROUPID, 1-GROUPCODE, 2-PRIORITY, 3-MEMBERS, 4-READER
                int groupIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 17", "GROUPID", lblAssignGroupMessage);     // index 0
                int groupCodeColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 17", "GROUPCODE", lblAssignGroupMessage); // index 1
                int priorityColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 17", "PRIORITY", lblAssignGroupMessage);   // index 2

                if (groupIdColumn != -1 && groupCodeColumn != -1 && priorityColumn != -1)
                {
                    e.Row.Cells[groupIdColumn].Visible = false;

                    if (e.Row.RowType == DataControlRowType.Header)
                    { myHelpers.RenameGridViewColumn(e, "GROUPCODE", "CODE"); }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[groupCodeColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[priorityColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
    }
}
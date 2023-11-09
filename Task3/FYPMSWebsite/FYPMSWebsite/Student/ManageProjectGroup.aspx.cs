using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Student
{
    public partial class ManageProjectGroup : Page
    {
        //***************************************************************************************
        // Uses TODO 24, TODO 29, TODO 31, TODO 33, TODO 34, TODO 35, TODO 36, TODO 37, TODO 38 *
        //***************************************************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly DBHelperMethods myDBHelpers = new DBHelperMethods();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private DataTable dtMemberRecord = new DataTable();

        /***** Private Methods *****/

        private bool AddGroupMember(string username, string groupId) // Uses TODO 36
        {
            bool result = false;

            //****************************************************
            // Uses TODO 36 to add a student to a project group. *
            //****************************************************
            if (myFYPMSDB.AddStudentToProjectGroup(username, groupId))
            {
                PopulateGroupMembers(groupId);
                result = true;
            }
            else // An SQL error occurred.
            {
                myHelpers.DisplayMessage(lblAddGroupMemberMessage, sqlErrorMessage);
            }

            return result;
        }

        private bool CanGetStudentRecord(string username, bool isForUsernameValidation) // Uses TODO 34
        {
            bool result = false;

            //***********************************************************
            // Uses TODO 34 to get the record of the logged in student. *
            //***********************************************************
            dtMemberRecord = myFYPMSDB.GetStudentRecord(username);

            if (myHelpers.IsQueryResultValid("TODO 34",
                                             dtMemberRecord,
                                             new List<string> { "USERNAME", "FIRSTNAME", "LASTNAME", "GROUPID" },
                                             lblGroupMembersMessage))
            {
                if (dtMemberRecord.Rows.Count == 0) // No record was retrieved.
                {
                    if (!isForUsernameValidation) //  Query error if not being used for username validation.
                    {
                        myHelpers.DisplayMessage(lblGroupMembersMessage, $"{queryError}TODO 34{queryErrorNoRecordsRetrieved}");
                        isQueryError = true;
                    }
                }
                else if (dtMemberRecord.Rows.Count == 1)  // Only one recod was retrieved.
                {
                    result = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGroupMembersMessage, $"{queryError}TODO 34{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }
            return result;
        }

        private void CheckHasGroupIndicatedInterestInFYP(string groupId) // Uses TODO 33
        {
            //***************************************************************************
            // Uses TODO 33 to get the FYPs in which a group has indicated an interest. *
            //***************************************************************************
            DataTable dtProjectsGroupInterestedIn = myFYPMSDB.GetFYPsGroupHasIndicatedInterestIn(groupId);

            if (myHelpers.IsQueryResultValid("TODO 33",
                                             dtProjectsGroupInterestedIn,
                                             new List<string> { "FYPID", "TITLE", "CATEGORY", "TYPE", "PRIORITY", "STATUS" },
                                             lblGroupMembersMessage))
            {
                if (dtProjectsGroupInterestedIn.Rows.Count == 0) // The group has indicated and interest in no FYPs.
                {
                    // Allow the group to be changed.
                    txtUsername.Text = string.Empty;
                    pnlAddGroupMember.Visible = true;
                }
                else // The group has indicated an interest in an FYP -> the group cannot be changed.
                {
                    myHelpers.DisplayMessage(lblGroupMembersMessage, cannotChangeGroup);
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, groupInterestedInFYP);
                    pnlAddGroupMember.Visible = gvGroupMembers.AutoGenerateDeleteButton = false;
                }
            }
        }

        private bool DeleteGroup(string groupId) // Uses TODO 38
        {
            bool result = false;

            //**********************************
            // Uses TODO 38 to delete a group. *
            //**********************************
            if (myFYPMSDB.DeleteProjectGroup(groupId))
            {
                ViewState["groupId"] = string.Empty;
                result = true;
            }
            else // An SQL error occurred.
            {
                myHelpers.DisplayMessage(lblDeleteProjectGroupMessage, sqlErrorMessage);
                pnlGroupMembers.Visible = pnlAddGroupMember.Visible = false;
            }

            return result;
        }

        private int GetGroupMembers(string groupId) // Uses TODO 24
        {
            int numMembers = 0;

            //******************************************************
            // Uses TODO 24 to get the members in a project group. *
            //******************************************************
            DataTable dtGroupMembers = myFYPMSDB.GetProjectGroupMembers(groupId);

            if (myHelpers.PopulateGridView("TODO 24",
                                           gvGroupMembers,
                                           dtGroupMembers,
                                           new List<string> { "USERNAME", "NAME", "GROUPID" },
                                           lblGroupMembersMessage,
                                           lblGroupMembersMessage,
                                           $"{queryError}TODO 24{queryErrorNoRecordsRetrieved}"))
            {
                if (isEmptyQueryResult) // No members in the group -> query error.
                {
                    pnlAddGroupMember.Visible = false;
                }
                else // Save the result for future use; set the number of members in the group.
                {
                    ViewState["dtGroupMembers"] = dtGroupMembers;
                    numMembers = dtGroupMembers.Rows.Count;
                    pnlGroupMembers.Visible = true;
                }
            }

            return numMembers;
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
                                             lblGroupAssignedMessage))
            {
                if (dtIsAssigned.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, $"{queryError}TODO 29{queryErrorNoRecordsRetrieved}");
                    isQueryError = true;
                }
                else if (dtIsAssigned.Rows.Count == 1) // One record was retrieved -> check if the group is assigned to an FYP.
                {
                    // If the group is not assigned to an FYP, allow it to be changed.
                    if (dtIsAssigned.Rows[0]["ASSIGNEDFYPID"].ToString().Trim() == string.Empty)
                    {
                        gvGroupMembers.AutoGenerateDeleteButton = true;
                        pnlAddGroupMember.Visible = true;
                    }
                    else // The group is assigned to an FYP -> it cannot be changed.
                    {
                        myHelpers.DisplayMessage(lblGroupAssignedMessage, cannotChangeGroup);
                        SetFYPTitle(groupId);
                        result = true;
                    }
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGroupAssignedMessage, $"{queryError}TODO 29{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }

            return result;
        }

        private void PopulateGroupMembers(string groupId)
        {
            // If the group is assigned to an FYP or has indicated interest in an FYP, it cannot be changed.
            // Only need to check if interest indicated in an FYP if not already assigned to an FYP.
            if (!IsGroupAssignedToFYP(groupId))
            {
                // If an SQL or query error occurred, then cannot determine if group is assigned to an FYP;
                // so no need to check if interest in an FYP has been indicated.
                if (!isSqlError && !isQueryError)
                {
                    CheckHasGroupIndicatedInterestInFYP(groupId);
                }
            }

            // Get the number of members in the group; limit the group size to maxMembers.
            // maxMembers is set in Global.asax.cs.
            // Do not allow adding members if maxMembers exceeded or an SQL error occurred.
            if (GetGroupMembers(groupId) == maxMembers || isSqlError)
            {
                pnlAddGroupMember.Visible = false;
            }
        }

        private void RemoveGroupMember(string selectedUsername, string groupId) // Uses TODO 37
        {
            //*********************************************************
            // Uses TODO 37 to remove a student from a project group. *
            //*********************************************************
            if (myFYPMSDB.RemoveStudentFromProjectGroup(selectedUsername))
            {
                // Delete the project group if the last member has been removed. Exit if an SQL error occurred.
                if (gvGroupMembers.Rows.Count == 1)
                {
                    if (!DeleteGroup(groupId))
                    {
                        return;
                    }
                }

                // If the removed user is the loggedin user, then do not display project group members.
                if (selectedUsername == loggedinUsername)
                {
                    myHelpers.DisplayMessage(lblGroupMembersMessage, $"{notGroupMember}<br/>{createOrJoinGroup}.");
                    pnlGroupMembers.Visible = pnlAddGroupMember.Visible = false;
                    pnlCreateGroup.Visible = true;
                }
                else // Repopulate the group members.
                {
                    PopulateGroupMembers(groupId);
                }
            }
            else // An SQL error occurred.
            {
                myHelpers.DisplayMessage(lblGroupMembersMessage, sqlErrorMessage);
            }
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

                // Delete any "dangling" groups due to SQL errors when removing members.
                myDBHelpers.CleanUpProjectGroups(lblDeleteProjectGroupMessage);

                if (CanGetStudentRecord(loggedinUsername, false))
                {
                    if (dtMemberRecord.Rows[0]["GROUPID"].ToString().Trim() != string.Empty)
                    {
                        // The student is a member of a group, so show all group members.
                        PopulateGroupMembers(dtMemberRecord.Rows[0]["GROUPID"].ToString());
                        pnlGroupMembers.Visible = true;
                    }
                    else // The student is not a member of a group, so show the button to create a group.
                    {
                        myHelpers.DisplayMessage(lblCreateDeleteGroupMessage, $"{notGroupMember}<br/>{createOrJoinGroup}.");
                        pnlCreateGroup.Visible = true;
                    }

                    // Save the group id in ViewState for use in subsequent processing.
                    ViewState["groupId"] = dtMemberRecord.Rows[0]["GROUPID"].ToString();
                }
            }
        }

        protected void BtnAddGroupMember_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string groupId = ViewState["groupId"].ToString();

                if (groupId != null)
                {
                    if (!AddGroupMember(txtUsername.Text.Trim(), groupId))
                    {
                        btnAddGroupMember.Enabled = false;
                    }
                }
                else // Group id is not assigned -> should not happen.
                {
                    myHelpers.DisplayMessage(lblAddGroupMemberMessage,
                                             $"{dbError}Student/ManageProjectGroup - BtnCreateGroup_Click{dbErrorNoGroupId}{contact3311rep}");
                    btnAddGroupMember.Enabled = false;
                }
            }
        }

        protected void BtnCreateGroup_Click(object sender, EventArgs e) // Uses TODO 35
        {
            if (IsValid)
            {
                // This is a new group, so get a new group id.
                string groupId = myDBHelpers.GetNextTableId("ProjectGroup", "groupId", lblCreateDeleteGroupMessage);

                if (groupId != string.Empty) // If the group id is empty, an error occurred getting a new group id.
                {
                    ViewState["groupId"] = groupId;

                    //******************************************
                    // Uses TODO 35 to create a project group. *
                    //******************************************
                    if (myFYPMSDB.CreateProjectGroup(groupId))
                    {
                        txtUsername.Text = loggedinUsername;
                        pnlGroupMembers.Visible = pnlAddGroupMember.Visible = true;
                        isSqlError = pnlCreateGroup.Visible = lblGroupMembersMessage.Visible = lblCreateDeleteGroupMessage.Visible = false;
                        if (!AddGroupMember(loggedinUsername, groupId))
                        {
                            btnAddGroupMember.Enabled = false;
                        }
                    }
                    else // An SQL error occurred.
                    {
                        myHelpers.DisplayMessage(lblCreateDeleteGroupMessage, sqlErrorMessage);
                        btnCreateGroup.Enabled = false;
                    }
                }
            }
        }

        protected void CvValidateNewMember_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            DataTable dtGroupMembers = (DataTable)ViewState["dtGroupMembers"];

            if (dtGroupMembers != null)
            {
                // Display the group members if the query result is valid.
                if (CanGetStudentRecord(txtUsername.Text.Trim(), true))
                {
                    // Check if the student has already been added to the project group.
                    foreach (DataRow row in dtGroupMembers.Rows)
                    {
                        if (row["USERNAME"].ToString().Trim() == txtUsername.Text.Trim())
                        {
                            CvValidateNewMember.ErrorMessage = memberOfThisGroup;
                            return;
                        }
                    }

                    // Check if the student is already a member of another project group.
                    if (dtMemberRecord.Rows[0]["GROUPID"].ToString().Trim() == string.Empty)
                    {
                        // The student is not a member of any group.
                        args.IsValid = true;
                    }
                    else  // The student is a member of another group.
                    {
                        CvValidateNewMember.ErrorMessage = memberOfAnotherGroup;
                    }
                }
                else  // No student with this username -> validation check.
                {
                    CvValidateNewMember.ErrorMessage = invalidUsername;
                }
            }
        }

        protected void GvGroupMembers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 3)
            {
                // GridView columns: 0-USERNAME, 1-NAME, 2-GROUPID
                int groupIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 24", "GROUPID", lblGroupMembersMessage);

                if (groupIdColumn != -1)
                {
                    e.Row.Cells[groupIdColumn].Visible = false;
                }
            }

            if (e.Row.Controls.Count == 4)
            {
                // Offset by 1 due to Delete column.
                // GridView columns: 0-Delete hyperlink, 1-USERNAME, 2-NAME, 3-GROUPID
                int groupIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 24", "GROUPID", lblGroupMembersMessage) + 1;

                if (groupIdColumn != 0)
                {
                    e.Row.Cells[groupIdColumn].Visible = false;
                }
            }
        }

        protected void GvGroupMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblGroupMembersMessage.Visible = false;
            string selectedUsername = gvGroupMembers.Rows[e.RowIndex].Cells[1].Text.Trim();
            string groupId = gvGroupMembers.Rows[e.RowIndex].Cells[3].Text;

            if (selectedUsername == loggedinUsername)
            {
                // The loggedin student is removing themseleves from the group. Verify removal.
                DialogResult dialogResult = MessageBox.Show($"If you remove yourself from the project group,"
                                                            + $"{Environment.NewLine}you will no longer have access to the project group."
                                                            + $"{Environment.NewLine}Do you want to proceed?",
                                                            "Remove Group Member", MessageBoxButtons.YesNo);

                switch (dialogResult)
                {
                    // Confirm removal of loggedin student.
                    case DialogResult.Yes:
                        RemoveGroupMember(selectedUsername, groupId);
                        break;
                    // No action.
                    case DialogResult.No:
                        break;
                }
            }
            else // Remove the selected student from the group.
            {
                RemoveGroupMember(selectedUsername, groupId);
            }
        }
    }
}
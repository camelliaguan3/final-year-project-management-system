using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;

namespace FYPMSWebsite.Faculty
{
    public partial class GradeGroup : Page
    {
        //***************************************************************************************
        // Uses TODO 13, TODO 21, TODO 22, TODO 23, TODO 24, TODO 25, TODO 26, TODO 27, TODO 31 *
        //***************************************************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly DBHelperMethods myDBHelpers = new DBHelperMethods();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private DataTable dtRequirementGrades;

        /***** Private Methods *****/

        private void CreateRequirementGradesRecords(string groupId) // Uses TODO 24, TODO 25
        {
            //*******************************************************
            // Uses TODO 24 to get the students in a project group. *
            //*******************************************************
            DataTable dtGroupMembers = myFYPMSDB.GetProjectGroupMembers(groupId);

            if (myHelpers.IsQueryResultValid("TODO 24",
                                             dtGroupMembers,
                                             new List<string> { "USERNAME", "NAME", "GROUPID" },
                                             lblStudentGradesResultMessage))
            {
                if (dtGroupMembers.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblStudentGradesResultMessage, $"{queryError}{"TODO 24"}{queryErrorNoRecordsRetrieved}");
                }
                else // Create RequirementGrades records if none exists.
                {
                    //*********************************************************************************************************************
                    // Uses TODO 25 in App_Code\DBHelperMethods to create a RequirementGrades record for each student in a project group. *
                    //*********************************************************************************************************************
                    if (myDBHelpers.CreateRequirementGradesRecord(loggedinUsername, dtGroupMembers))
                    {
                        PopulateRequirementGrades(groupId);
                    }
                    else // An SQL error occurred.
                    {
                        myHelpers.DisplayMessage(lblStudentGradesResultMessage, sqlErrorMessage);
                    }
                }
            }
        }

        private bool IsGradeValid(string gradeName, string grade, Label labelControl)
        {
            bool result = false;

            if (labelControl.Visible == false)
            {
                if (myHelpers.IsValidAndInRange(grade, 0, 100) || grade == string.Empty)
                {
                    result = true;
                }
                else
                {
                    labelControl.Text = $"Please enter a valid {gradeName} grade.";
                    labelControl.ForeColor = Color.DarkRed;
                    labelControl.Visible = true;
                }
            }

            return result;
        }

        private void PopulateAssignedProjectGroupsDropDownList() // Uses TODO 13, TODO 21
        {
            lblSelectAssignedGroupsMessage.Visible = lblNoAssignedGroupMessage.Visible = pnlStudentGrades.Visible = false;
            pnlSelectGroup.Visible = true;
            DataTable dtAssignedGroups;
            string TODO;

            if (rblGraderRole.SelectedValue == "supervisor") // Supervisor role.
            {
                //***********************************************************************************
                // Uses TODO 13 to get project groups for which the logged in user is a supervisor. *
                //***********************************************************************************
                dtAssignedGroups = myFYPMSDB.GetSupervisorProjectGroups(loggedinUsername);
                TODO = "TODO 13";
            }

            else // Reader role.
            {
                //*********************************************************************************
                // Uses TODO 21 to get project groups for which the logged in user is the reader. *
                //*********************************************************************************
                dtAssignedGroups = myFYPMSDB.GetReaderProjectGroups(loggedinUsername);
                TODO = "TODO 21";
            }

            ViewState["dtGroups"] = dtAssignedGroups; // Save the group information for editing.

            // Populate the assigned groups dropdown list if the query result is valid.
            if (!myHelpers.PopulateDropDownList(TODO,
                                               string.Empty,
                                               ddlAssignedGroups,
                                               dtAssignedGroups,
                                               new List<string> { "GROUPID", "GROUPCODE", "ASSIGNEDFYPID" },
                                               lblSelectAssignedGroupsMessage,
                                               lblNoAssignedGroupMessage,
                                               noGroupToGrade,
                                               EmptyQueryResultMessageType.Information))
            {
                // An error occurred populating the dropdown list.
                if (isEmptyQueryResult)
                {
                    pnlSelectGroup.Visible = false;
                }
                ddlAssignedGroups.Items.Clear();
            }
        }

        private void PopulateRequirementGrades(string groupId) // Uses TODO 22, TODO 23
        {
            string TODO;
            pnlStudentGrades.Visible = true;

            if (rblGraderRole.SelectedValue == "supervisor") // Grader role is supervisor.
            {
                TODO = "TODO 22";
                //***********************************************************************************************
                // Uses TODO 22 to get the grades given by the logged in user as supervisor of a project group. *
                //***********************************************************************************************
                dtRequirementGrades = myFYPMSDB.GetSupervisorRequirementGrades(groupId,
                    (ViewState["dtGroups"] as DataTable).Rows[Convert.ToInt16(ddlAssignedGroups.SelectedIndex) - 1]["ASSIGNEDFYPID"].ToString());
            }
            else // Grader role is reader.
            {
                TODO = "TODO 23";
                //*******************************************************************************************
                // Uses TODO 23 to get the grades given by the logged in user as reader of a project group. *
                //*******************************************************************************************
                dtRequirementGrades = myFYPMSDB.GetReaderRequirementGrades(groupId, loggedinUsername);
            }

            // Populate a GridView with the grades of the project group members.
            if (myHelpers.PopulateGridView(TODO,
                                           gvStudentGrades,
                                           dtRequirementGrades,
                                           new List<string> { "USERNAME", "NAME", "PROPOSALREPORT", "PROGRESSREPORT", "FINALREPORT", "PRESENTATION" },
                                           lblStudentGradesResultMessage,
                                           lblStudentGradesResultMessage,
                                           noMessage))
            {
                // If no Requirement records were retrieved, create requirement records for each student in the project group.
                if (isEmptyQueryResult)
                {
                    CreateRequirementGradesRecords(groupId);
                }
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
                                             lblTitleMessage))
            {
                if (dtAssignedFYPTitle.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblTitleMessage, $"{queryError}TODO 31{queryErrorNoRecordsRetrieved}");
                }
                else if (dtAssignedFYPTitle.Rows.Count == 1) // Only one record was retrieved -> set the FYP title.
                {
                    txtTitle.Text = dtAssignedFYPTitle.Rows[0]["TITLE"].ToString().Trim();
                    txtTitle.ForeColor = Color.Black;
                    txtTitle.Visible = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblTitleMessage, $"{queryError}TODO 31{queryErrorMultipleRecordsRetrieved}");
                }
            }
        }

        private void UpdateRequirementGrades(string fypId,
                                             string studentUsername,
                                             string proposalReport,
                                             string progressReport,
                                             string finalReport,
                                             string presentation) // Uses TODO 26, TODO 27
        {
            if (rblGraderRole.SelectedValue == "supervisor") // Grader role is supervisor.
            {
                //*******************************************************************************
                // Uses TODO 26 to update the grades given by the logged in user as supervisor. *
                //*******************************************************************************
                if (!myFYPMSDB.UpdateSupervisorRequirementGrades(fypId, studentUsername, proposalReport,
                    progressReport, finalReport, presentation))
                {
                    // An SQL error occurred.
                    myHelpers.DisplayMessage(lblStudentGradesResultMessage, sqlErrorMessage);
                }
            }
            else // Grader role is reader.
            {
                //***************************************************************************
                // Uses TODO 27 to update the grades given by the logged in user as reader. *
                //***************************************************************************
                if (!myFYPMSDB.UpdateReaderRequirementGrades(loggedinUsername, studentUsername, proposalReport,
                    progressReport, finalReport, presentation))
                {
                    // An SQL error occurred.
                    myHelpers.DisplayMessage(lblStudentGradesResultMessage, sqlErrorMessage);
                }
            }

            gvStudentGrades.EditIndex = -1; // Cancel editing.

            PopulateRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                isSqlError = isQueryError = false;

                // Initially the grader role is supervisor.
                rblGraderRole.SelectedValue = "supervisor";

                PopulateAssignedProjectGroupsDropDownList();
            }
        }

        protected void DdlAssignedGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStudentGrades.EditIndex = -1;

            if (IsValid)
            {
                pnlStudentGrades.Visible = lblStudentGradesResultMessage.Visible = gvStudentGrades.Visible = false;

                SetFYPTitle(ddlAssignedGroups.SelectedValue);
                PopulateRequirementGrades(ddlAssignedGroups.SelectedValue);
            }
            else
            {
                pnlStudentGrades.Visible = false;
            }
        }

        protected void GvStudentGrades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudentGrades.EditIndex = -1;
            lblProposalGradeErrorMessage.Visible = lblProgressGradeErrorMessage.Visible =
                lblFinalGradeErrorMessage.Visible = lblPresentationGradeErrorMessage.Visible = false;

            if (!isSqlError && !isQueryError) // Hide the grades if an SQL or query error occurred
            {
                lblStudentGradesResultMessage.Visible = gvStudentGrades.Visible = false;
            }

            PopulateRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvStudentGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // Offset by 1 due to Edit column is column 0.
                // GridView columns: 0-Edit hyperlink, 1-USERNAME, 2-NAME, 3-PROPOSALREPORT, 4-PROGRESSREPORT, 5-FINALREPORT, 6-PRESENTATION
                int usernameColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22 or TODO 23", "USERNAME", lblStudentGradesResultMessage) + 1;             // index 1
                int proposalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22 or TODO 23", "PROPOSALREPORT", lblStudentGradesResultMessage) + 1; // index 4
                int progressReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22 or TODO 23", "PROGRESSREPORT", lblStudentGradesResultMessage) + 1; // index 5
                int finalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22 or TODO 23", "FINALREPORT", lblStudentGradesResultMessage) + 1;       // index 6
                int presentationColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22 or TODO 23", "PRESENTATION", lblStudentGradesResultMessage) + 1;     // index 7

                if (usernameColumn != -1 && proposalReportColumn != -1 && progressReportColumn != -1 && finalReportColumn != -1 && presentationColumn != -1)
                {
                    e.Row.Cells[usernameColumn].Visible = false;

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        myHelpers.RenameGridViewColumn(e, "PROPOSALREPORT", "PROPOSAL");
                        myHelpers.RenameGridViewColumn(e, "PROGRESSREPORT", "PROGRESS");
                        myHelpers.RenameGridViewColumn(e, "FINALREPORT", "FINAL");
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[proposalReportColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[progressReportColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[finalReportColumn].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[presentationColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }

        protected void GvStudentGrades_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudentGrades.EditIndex = e.NewEditIndex;
            PopulateRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvStudentGrades_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsValid)
            {
                lblProposalGradeErrorMessage.Visible = lblProgressGradeErrorMessage.Visible =
                lblFinalGradeErrorMessage.Visible = lblPresentationGradeErrorMessage.Visible =
                lblStudentGradesResultMessage.Visible = false;

                string fypId = (ViewState["dtGroups"] as DataTable).Rows[Convert.ToInt16(ddlAssignedGroups.SelectedIndex) - 1]["ASSIGNEDFYPID"].ToString();

                GridViewRow row = gvStudentGrades.Rows[e.RowIndex];

                string studentUsername = (row.Cells[1].Controls[0] as TextBox).Text; // student username column
                string proposalReport = (row.Cells[3].Controls[0] as TextBox).Text;  // proposal grade column
                string progressReport = (row.Cells[4].Controls[0] as TextBox).Text;  // progress grade column
                string finalReport = (row.Cells[5].Controls[0] as TextBox).Text;     // final grade colum
                string presentation = (row.Cells[6].Controls[0] as TextBox).Text;    // presentation grade column

                // Check if all grades are valid.
                if (IsGradeValid("proposal", proposalReport, lblProposalGradeErrorMessage)
                    & IsGradeValid("progress", progressReport, lblProgressGradeErrorMessage)
                    & IsGradeValid("final", finalReport, lblFinalGradeErrorMessage)
                    & IsGradeValid("presentation", presentation, lblPresentationGradeErrorMessage))
                {
                    // Set the grade value to 'null' if no grade is specified.
                    if (proposalReport == string.Empty) { proposalReport = "null"; }
                    if (progressReport == string.Empty) { progressReport = "null"; }
                    if (finalReport == string.Empty) { finalReport = "null"; }
                    if (presentation == string.Empty) { presentation = "null"; }

                    UpdateRequirementGrades(fypId, studentUsername, proposalReport, progressReport, finalReport, presentation);
                }
            }
        }

        protected void RblGraderRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Repopulate the project group dropdown list when the grader role is changed.
            PopulateAssignedProjectGroupsDropDownList();
        }
    }
}
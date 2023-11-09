using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Coordinator
{
    public partial class ReviewGrades : Page
    {
        //***************************************************************************************
        // Uses TODO 03, TODO 22, TODO 23, TODO 24, TODO 25, TODO 26, TODO 27, TODO 40, TODO 46 *
        //***************************************************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly DBHelperMethods myDBHelpers = new DBHelperMethods();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private string readerUsername; // Assigned in IsProjectAssigned.
        public enum GraderRole { supervisor, reader };

        /***** Private Methods *****/

        private void CreateRequirementGradesRecords(string groupId, string username, Label labelControl) // Uses TODO 24, TODO 25
        {
            //*******************************************************
            // Uses TODO 24 to get the students in a project group. *
            //*******************************************************
            DataTable dtProjectGroupMembers = myFYPMSDB.GetProjectGroupMembers(groupId);

            if (myHelpers.IsQueryResultValid("TODO 24",
                                             dtProjectGroupMembers,
                                             new List<string> { "USERNAME", "NAME", "GROUPID" },
                                             labelControl))
            {
                if (dtProjectGroupMembers.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(labelControl, $"{queryError}{"TODO 24"}{queryErrorNoRecordsRetrieved}");
                }
                else // Create RequirementGrades records if none exists.
                {
                    //*********************************************************************************************************************
                    // Uses TODO 25 in App_Code\DBHelperMethods to create a RequirementGrades record for each student in a project group. *
                    //*********************************************************************************************************************
                    if (myDBHelpers.CreateRequirementGradesRecord(username, dtProjectGroupMembers))
                    {
                        PopulateSupervisorRequirementGrades(groupId);
                        PopulateReaderRequirementGrades(groupId);
                    }
                    else // An SQL error occurred.
                    {
                        myHelpers.DisplayMessage(labelControl, sqlErrorMessage);
                    }
                }
            }
        }

        private string GetSupervisorUsername(string fypId) // Uses TODO 03
        {
            string result = string.Empty;
            txtSupervisors.ForeColor = Color.Black;
            txtSupervisors.Font.Bold = false;

            //******************************************************************
            // Uses TODO 03 to get the usernames of the supervisors of an FYP. *
            //******************************************************************
            DataTable dtProjectSupervisors = myFYPMSDB.GetFYPSupervisors(fypId);

            if (myHelpers.IsQueryResultValid("TODO 03",
                                             dtProjectSupervisors,
                                             new List<string> { "USERNAME", "NAME" },
                                             lblSupervisorGradesMessage))
            {
                if (dtProjectSupervisors.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    txtSupervisors.Text = $"{queryError}TODO 03{queryErrorNoRecordsRetrieved}";
                    txtSupervisors.ForeColor = Color.Red;
                }
                else // Set the supervisor name(s).
                {
                    txtSupervisors.Text = StringExtensions.DataTableColumnToString(dtProjectSupervisors, "NAME", ",");
                    result = dtProjectSupervisors.Rows[0]["USERNAME"].ToString().Trim();
                }
            }
            else // An SQL error occurred.
            {
                lblSupervisorGradesMessage.Visible = false;
                txtSupervisors.Text = lblSupervisorGradesMessage.Text;
                txtSupervisors.ForeColor = Color.Red;
            }

            return result;
        }

        private bool IsGradeValid(string gradeName, string grade, Label labelControl)
        {
            bool result = false;

            // Check the grade value only if an error in the value has not already been detected.
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

        private void PopulateAssignedProjectGroupsDropDownList() // Uses TODO 46
        {
            lblSelectAssignedGroupMessage.Visible = lblNoAssignedGroupMessage.Visible = pnlStudentGrades.Visible = false;
            pnlSelectGroup.Visible = true;

            //************************************************************************************************
            // Uses TODO 46 to populate a dropdown list with the project groups that are assigned to an FYP. *
            //************************************************************************************************
            DataTable dtAssignedGroups = myFYPMSDB.GetAssignedProjectGroups();

            if (myHelpers.PopulateDropDownList("TODO 46",
                                                string.Empty,
                                                ddlAssignedGroups,
                                                dtAssignedGroups,
                                                new List<string> { "GROUPID", "GROUPCODE", "ASSIGNEDFYPID", "READER" },
                                                lblSelectAssignedGroupMessage,
                                                lblNoAssignedGroupMessage,
                                                noAssignedGroups,
                                                EmptyQueryResultMessageType.Information))
            {
                // Save the group information for editing.
                ViewState["dtAssignedGroups"] = dtAssignedGroups;
            }
            else // An error occurred populating the fropdown list.
            {
                if (isEmptyQueryResult)
                {
                    pnlSelectGroup.Visible = false;
                }
                ddlAssignedGroups.Items.Clear();
            }
        }

        private void PopulateReaderRequirementGrades(string groupId) // Uses TODO 23
        {
            pnlStudentGrades.Visible = true;

            // Get the reader for the project group, if any.
            string readerUsername = ((DataTable)ViewState["dtAssignedGroups"]).Rows[Convert.ToInt16(ddlAssignedGroups.SelectedIndex) - 1]["READER"].ToString().Trim();
            ViewState["readerUsername"] = readerUsername;

            if (readerUsername != null)
            {
                // Get the reader's name using the username.
                if (SetReaderName())
                {
                    //***********************************************************************************************
                    // Uses TODO 23 to populate a GridView with the grades given by the reader for a project group. *
                    //***********************************************************************************************
                    if (myHelpers.PopulateGridView("TODO 23",
                                                   gvReaderGrades,
                                                   myFYPMSDB.GetReaderRequirementGrades(groupId, readerUsername),
                                                   new List<string> { "USERNAME", "NAME", "PROPOSALREPORT",
                                                       "PROGRESSREPORT", "FINALREPORT", "PRESENTATION" },
                                                   lblReaderGradesMessage,
                                                   lblReaderGradesMessage,
                                                   noMessage))
                    {
                        // If no Requirement records found, create requirement records for each student in the project group.
                        if (isEmptyQueryResult)
                        {
                            CreateRequirementGradesRecords(groupId, readerUsername, lblReaderGradesMessage);
                        }
                    }
                }
            }
        }

        private void PopulateSupervisorRequirementGrades(string groupId) // Uses TODO 22
        {
            pnlStudentGrades.Visible = true;

            // Get the fyp id of the project to which the group is assigned.
            string fypId = ((DataTable)ViewState["dtAssignedGroups"]).Rows[Convert.ToInt16(ddlAssignedGroups.SelectedIndex) - 1]["ASSIGNEDFYPID"].ToString();

            if (fypId != null) // An error occurred if the supervisor username is empty.
            {
                // Get the username of a supervisor of the FYP.
                string supervisorUsername = GetSupervisorUsername(fypId);
                {
                    //***************************************************************************************
                    // Uses TODO 22 to populate a GridView with the grades given by a supervisor of an FYP. *
                    //***************************************************************************************
                    if (myHelpers.PopulateGridView("TODO 22",
                                                   gvSupervisorGrades,
                                                   myFYPMSDB.GetSupervisorRequirementGrades(groupId, fypId),
                                                   new List<string> { "USERNAME", "NAME", "PROPOSALREPORT",
                                                       "PROGRESSREPORT", "FINALREPORT", "PRESENTATION" },
                                                   lblSupervisorGradesMessage,
                                                   lblSupervisorGradesMessage,
                                                   noMessage))
                    {
                        // If no Requirement records found, create requirement records for each student in the project group.
                        if (isEmptyQueryResult)
                        {
                            if (supervisorUsername != string.Empty) // An error occured getting the supervisor name if it is empty.
                            {
                                CreateRequirementGradesRecords(groupId, supervisorUsername, lblSupervisorGradesMessage);
                            }
                        }
                    }
                }
            }
        }

        private bool SetReaderName() // Uses TODO 40
        {
            bool result = false;
            string readerUsername = ((DataTable)ViewState["dtAssignedGroups"]).Rows[ddlAssignedGroups.SelectedIndex - 1]["READER"].ToString();

            //**************************************************************************************
            // Uses TODO 40 to get the name of the faculty assigned as reader for a project group. *
            //**************************************************************************************
            DataTable dtReaderName = myFYPMSDB.GetProjectGroupReaderName(readerUsername);

            if (myHelpers.IsQueryResultValid("TODO 40",
                                             dtReaderName,
                                             new List<string> { "NAME" },
                                             lblReaderGradesMessage))
            {
                if (dtReaderName.Rows.Count == 0) // No record was retrieved -> no reader assigned.
                {
                    txtReader.Text = noReaderAssigned;
                    txtReader.ForeColor = Color.Blue;
                }
                else if (dtReaderName.Rows.Count == 1) // Only one record was retrieved -> a reader is assigned.
                {
                    txtReader.Text = dtReaderName.Rows[0]["NAME"].ToString();
                    txtReader.ForeColor = Color.Black;
                    result = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    txtReader.Text = $"{queryError}TODO 40{queryErrorMultipleRecordsRetrieved}";
                    txtReader.ForeColor = Color.Red;
                }
            }
            else // An SQL error occurred.
            {
                lblReaderGradesMessage.Visible = false;
                txtReader.Text = lblReaderGradesMessage.Text;
                txtReader.ForeColor = Color.Red;
            }

            return result;
        }

        private void UpdateRequirementGrades(GraderRole role,
                                             string fypIdORreaderUsername,
                                             string studentUsername,
                                             string proposalReport,
                                             string progressReport,
                                             string finalReport,
                                             string presentation) // Uses TODO 26, TODO 27
        {
            if (role == GraderRole.supervisor) // Grader role is supervisor.
            {
                //***********************************************************
                // Uses TODO 26 to update the grades given by a supervisor. *
                //***********************************************************
                if (!myFYPMSDB.UpdateSupervisorRequirementGrades(fypIdORreaderUsername, studentUsername,
                    proposalReport, progressReport, finalReport, presentation))
                {
                    myHelpers.DisplayMessage(lblSupervisorGradesMessage, sqlErrorMessage);
                }

                gvSupervisorGrades.EditIndex = -1; // Cancel editing.
            }
            else // Grader role is reader.
            {
                //*********************************************************
                // Uses TODO 27 to update the grades given by the reader. *
                //*********************************************************
                if (!myFYPMSDB.UpdateReaderRequirementGrades(fypIdORreaderUsername, studentUsername,
                    proposalReport, progressReport, finalReport, presentation))
                {
                    myHelpers.DisplayMessage(lblReaderGradesMessage, sqlErrorMessage);
                }

                gvReaderGrades.EditIndex = -1; // Cancel editing.
            }

            PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
            PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                PopulateAssignedProjectGroupsDropDownList();
            }
        }

        protected void DdlAssignedGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsValid)
            {
                pnlStudentGrades.Visible = lblSelectAssignedGroupMessage.Visible =
                    lblSupervisorGradesMessage.Visible = lblReaderGradesMessage.Visible =
                    gvSupervisorGrades.Visible = gvReaderGrades.Visible = false;

                PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
                PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
            }
            else // Hide the grades if an error occurred.
            {
                pnlStudentGrades.Visible = false;
            }
        }

        protected void GvReaderGrades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvReaderGrades.EditIndex = -1;
            lblReaderProposalGradeErrorMessage.Visible = lblReaderProgressGradeErrorMessage.Visible =
                lblReaderFinalGradeErrorMessage.Visible = lblReaderPresentationGradeErrorMessage.Visible = false;

            if (!isSqlError && !isQueryError)
            {
                lblSupervisorGradesMessage.Visible = lblReaderGradesMessage.Visible =
                    gvSupervisorGrades.Visible = gvReaderGrades.Visible = false;
            }

            PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
            PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvReaderGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // Offset by 1 due to Edit column.
                // GridView columns: 0-Edit hyperlink, 1-USERNAME, 2-NAME, 3-PROPOSALREPORT, 4-PROGRESSREPORT, 5-FINALREPORT, 6-PRESENTATION
                int usernameColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "USERNAME", lblReaderGradesMessage) + 1;             // index 1
                int proposalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PROPOSALREPORT", lblReaderGradesMessage) + 1; // index 3
                int progressReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PROGRESSREPORT", lblReaderGradesMessage) + 1; // index 4
                int finalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "FINALREPORT", lblReaderGradesMessage) + 1;       // index 5
                int presentationColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PRESENTATION", lblReaderGradesMessage) + 1;     // index 6

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

        protected void GvReaderGrades_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvReaderGrades.EditIndex = e.NewEditIndex;
            PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
            PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvReaderGrades_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsValid)
            {
                lblReaderProposalGradeErrorMessage.Visible = lblReaderProgressGradeErrorMessage.Visible =
                lblReaderFinalGradeErrorMessage.Visible = lblReaderPresentationGradeErrorMessage.Visible =
                lblReaderGradesMessage.Visible = false;

                string readerUsername = ViewState["readerUsername"].ToString();

                if (readerUsername != null)
                {
                    GridViewRow row = gvReaderGrades.Rows[e.RowIndex];

                    string studentUsername = (row.Cells[1].Controls[0] as TextBox).Text; // username column
                    string proposalReport = (row.Cells[3].Controls[0] as TextBox).Text;  // proposal grade column
                    string progressReport = (row.Cells[4].Controls[0] as TextBox).Text;  // progress grade column
                    string finalReport = (row.Cells[5].Controls[0] as TextBox).Text;     // final grade column
                    string presentation = (row.Cells[6].Controls[0] as TextBox).Text;    // presentation grade column

                    // Check if all grades are valid.
                    if (IsGradeValid("proposal", proposalReport, lblReaderProposalGradeErrorMessage)
                        & IsGradeValid("progress", progressReport, lblReaderProgressGradeErrorMessage)
                        & IsGradeValid("final", finalReport, lblReaderFinalGradeErrorMessage)
                        & IsGradeValid("presentation", presentation, lblReaderPresentationGradeErrorMessage))
                    {
                        if (proposalReport == string.Empty) { proposalReport = "null"; }
                        if (progressReport == string.Empty) { progressReport = "null"; }
                        if (finalReport == string.Empty) { finalReport = "null"; }
                        if (presentation == string.Empty) { presentation = "null"; }

                        UpdateRequirementGrades(GraderRole.reader, readerUsername, studentUsername, 
                            proposalReport, progressReport, finalReport, presentation);
                    }
                }
            }
        }

        protected void GvSupervisorGrades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSupervisorGrades.EditIndex = -1;
            lblSupervisorProposalGradeErrorMessage.Visible = lblSupervisorProgressGradeErrorMessage.Visible =
                lblSupervisorFinalGradeErrorMessage.Visible = lblSupervisorPresentationGradeErrorMessage.Visible = false;

            if (!isSqlError && !isQueryError)
            {
                lblSupervisorGradesMessage.Visible = lblReaderGradesMessage.Visible =
                    gvSupervisorGrades.Visible = gvReaderGrades.Visible = false;
            }

            PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
            PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvSupervisorGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // Offset by 1 due to Edit column.
                // GridView columns: 0-Edit hyperlink, 1-USERNAME, 2-NAME, 3-PROPOSALREPORT, 4-PROGRESSREPORT, 5-FINALREPORT, 6-PRESENTATION
                int usernameColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "USERNAME", lblSupervisorGradesMessage) + 1;             // index 1
                int proposalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PROPOSALREPORT", lblSupervisorGradesMessage) + 1; // index 3
                int progressReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PROGRESSREPORT", lblSupervisorGradesMessage) + 1; // index 4
                int finalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "FINALREPORT", lblSupervisorGradesMessage) + 1;       // index 5
                int presentationColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PRESENTATION", lblSupervisorGradesMessage) + 1;     // index 6

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

        protected void GvSupervisorGrades_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSupervisorGrades.EditIndex = e.NewEditIndex;
            PopulateSupervisorRequirementGrades(ddlAssignedGroups.SelectedValue);
            PopulateReaderRequirementGrades(ddlAssignedGroups.SelectedValue);
        }

        protected void GvSupervisorGrades_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsValid)
            {
                lblSupervisorProposalGradeErrorMessage.Visible = lblSupervisorProgressGradeErrorMessage.Visible =
                lblSupervisorFinalGradeErrorMessage.Visible = lblSupervisorPresentationGradeErrorMessage.Visible =
                lblSupervisorGradesMessage.Visible = false;

                string fypId = ((DataTable)ViewState["dtAssignedGroups"]).Rows[Convert.ToInt16(ddlAssignedGroups.SelectedIndex) - 1]["ASSIGNEDFYPID"].ToString();

                if (fypId != null)
                {
                    GridViewRow row = gvSupervisorGrades.Rows[e.RowIndex];

                    string studentUsername = (row.Cells[1].Controls[0] as TextBox).Text; // username column
                    string proposalReport = (row.Cells[3].Controls[0] as TextBox).Text;  // proposal grade column
                    string progressReport = (row.Cells[4].Controls[0] as TextBox).Text;  // progress grade column
                    string finalReport = (row.Cells[5].Controls[0] as TextBox).Text;     // final grade column
                    string presentation = (row.Cells[6].Controls[0] as TextBox).Text;    // presentation grade column

                    // Check if all grades are valid.
                    if (IsGradeValid("proposal", proposalReport, lblSupervisorProposalGradeErrorMessage)
                        & IsGradeValid("progress", progressReport, lblSupervisorProgressGradeErrorMessage)
                        & IsGradeValid("final", finalReport, lblSupervisorFinalGradeErrorMessage)
                        & IsGradeValid("presentation", presentation, lblSupervisorPresentationGradeErrorMessage))
                    {
                        if (proposalReport == string.Empty) { proposalReport = "null"; }
                        if (progressReport == string.Empty) { progressReport = "null"; }
                        if (finalReport == string.Empty) { finalReport = "null"; }
                        if (presentation == string.Empty) { presentation = "null"; }

                        UpdateRequirementGrades(GraderRole.supervisor, fypId, studentUsername, 
                            proposalReport, progressReport, finalReport, presentation);
                    }
                }
            }
        }
    }
}
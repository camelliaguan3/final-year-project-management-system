using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Student
{
    public partial class ViewGrades : System.Web.UI.Page
    {
        //***************************************************
        // Uses TODO 03, TODO 28, TODO 39, TODO 40, TODO 41 *
        //***************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;
        private string fypId; // Assigned in IsProjectAssigned.
        private string groupId; // Assigned in GetStudentGroupId.
        private DataTable dtSupervisors; // Assigned in SetSupervisorNames;
        private DataTable dtSupervisorGrades; // Assigned in 
        private string readerUsername = string.Empty; // Assigned in IsProjectAssigned.

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
                                             lblGetGroupIdMessage))
            {
                if (dtGroup.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGetGroupIdMessage, $"{queryError}TODO 28{queryErrorNoRecordsRetrieved}");
                    isQueryError = true;
                }
                else if (dtGroup.Rows.Count == 1) // Only one record was retrieved -> the student is a member of a group.
                {
                    groupId = dtGroup.Rows[0]["GROUPID"].ToString();
                    result = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGetGroupIdMessage, $"{queryError}TODO 28{queryErrorMultipleRecordsRetrieved}");
                    isQueryError = true;
                }
            }

            return result;
        }

        private bool IsProjectAssigned(string username) // Uses TODO 39
        {
            bool result = false;

            //******************************************************************
            // Uses TODO 39 to get the project to which a student is assigned. *
            //******************************************************************
            DataTable dtFYPInfo = myFYPMSDB.GetAssignedFYPInformation(username);

            if (myHelpers.IsQueryResultValid("TODO 39",
                                             dtFYPInfo,
                                             new List<string> { "FYPID", "TITLE", "READER" },
                                             lblProjectAssignedMessage))
            {
                if (dtFYPInfo.Rows.Count == 0) // No record was retrieved -> not a member of a group.
                {
                    myHelpers.DisplayMessage(lblProjectAssignedMessage, noFYPAssigned);
                }
                else if (dtFYPInfo.Rows.Count == 1) // Only one record was retrieved -> assigned to a project.
                {
                    txtTitle.Text = dtFYPInfo.Rows[0]["TITLE"].ToString().Trim();
                    txtTitle.ForeColor = Color.Black;
                    fypId = dtFYPInfo.Rows[0]["FYPID"].ToString().Trim();
                    readerUsername = dtFYPInfo.Rows[0]["READER"].ToString().Trim();
                    result = pnlTitle.Visible = txtTitle.Visible = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblProjectAssignedMessage, $"{queryError}TODO 39{queryErrorMultipleRecordsRetrieved}");
                }
            }

            return result;
        }

        private void PopulateReaderRequirementGrades() // Uses TODO 41
        {
            pnlStudentGrades.Visible = true;

            //***********************************************************************************************
            // Uses TODO 41 to populate a GridView with the grades given by the reader for a project group. *
            //***********************************************************************************************
            DataTable dtReaderGrades = myFYPMSDB.GetStudentGrades(readerUsername, loggedinUsername);

            if (myHelpers.PopulateGridView("TODO 41",
                                           gvReaderGrades,
                                           dtReaderGrades,
                                           new List<string> { "PROPOSALREPORT", "PROGRESSREPORT", "FINALREPORT", "PRESENTATION" },
                                           lblReaderGradesMessage,
                                           lblReaderGradesMessage,
                                           notGraded))
            {
                if (dtReaderGrades.Rows.Count == 0) // No record was retrieved -> not yet graded.
                {
                    myHelpers.DisplayMessage(lblReaderGradesMessage, notGraded);
                }
                else if (dtReaderGrades.Rows.Count > 1) // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblReaderGradesMessage, $"{queryError}TODO 41{queryErrorMultipleRecordsRetrieved}");
                }
            }
        }

        private void PopulateSupervisorRequirementGrades() // Uses TODO 41
        {
            foreach (DataRow dr in dtSupervisors.Rows)
            {
                //******************************************************************************************
                // Uses TODO 41 to populate a GridView with the grades given by a supervisor of a student. *
                // There can be two supervisors for an FYP and either of them can give grades. Therefore,  *
                // it is necessary to check for grades given by either supervisor since only one           *
                // supervisor username appears in a grade record. Can exit when a grade record is found.   *
                //******************************************************************************************
                dtSupervisorGrades = myFYPMSDB.GetStudentGrades(dr["USERNAME"].ToString(), loggedinUsername);
                if (dtSupervisorGrades!=null && dtSupervisorGrades.Rows.Count == 1)
                {
                    break;
                }
            }

            if (myHelpers.PopulateGridView("TODO 41",
                                           gvSupervisorGrades,
                                           dtSupervisorGrades,
                                           new List<string> { "PROPOSALREPORT", "PROGRESSREPORT", "FINALREPORT", "PRESENTATION" },
                                           lblSupervisorGradesMessage,
                                           lblSupervisorGradesMessage,
                                           noMessage))
            {
                if (dtSupervisorGrades.Rows.Count == 0) // No record was retrieved -> not yet graded.
                {
                    myHelpers.DisplayMessage(lblSupervisorGradesMessage, notGraded);
                }
                else if (dtSupervisorGrades.Rows.Count > 1) // Multiple recordfs were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblSupervisorGradesMessage, $"{queryError}TODO 41{queryErrorMultipleRecordsRetrieved}");
                }
            }
        }

        private void SetReaderName() // Uses TODO 40
        {
            //**************************************************************************************
            // Uses TODO 40 to get the name of the faculty assigned as reader for a project group. *
            //**************************************************************************************
            DataTable dtReaderName = myFYPMSDB.GetProjectGroupReaderName(readerUsername);

            if (myHelpers.IsQueryResultValid("TODO 40",
                                             dtReaderName,
                                             new List<string> { "NAME" },
                                             lblReaderGradesMessage))
            {
                if (dtReaderName.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    txtReader.Text = $"{queryError}TODO 40{queryErrorNoRecordsRetrieved}";
                    txtReader.ForeColor = Color.Red;
                }
                else if (dtReaderName.Rows.Count == 1) // Only one record was retrieved -> set the reader name.
                {
                    txtReader.Text = dtReaderName.Rows[0]["NAME"].ToString();
                    txtReader.ForeColor = Color.Black;
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
        }

        private bool SetSupervisorNames(string fypId) // Uses TODO 03
        {
            bool result = false;

            //*************************************************
            // Uses TODO 03 to get the supervisors of an FYP. *
            //*************************************************
            dtSupervisors = myFYPMSDB.GetFYPSupervisors(fypId);

            if (myHelpers.IsQueryResultValid("TODO 03",
                                             dtSupervisors,
                                             new List<string> { "USERNAME", "NAME" },
                                             lblSupervisorGradesMessage))
            {
                if (dtSupervisors.Rows.Count == 0) // No records were retrieved -> query error.
                {
                    txtSupervisors.Text = $"{queryError}TODO 03{queryErrorNoRecordsRetrieved}";
                    txtSupervisors.ForeColor = Color.Red;
                }
                else // Set the supervisor name(s).
                {
                    txtSupervisors.Text = StringExtensions.DataTableColumnToString(dtSupervisors, "NAME", ",");
                    txtSupervisors.ForeColor = Color.Black;
                    result = txtSupervisors.Visible = true;
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

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;

                // Determine if the student is assigned to a project.
                if (IsProjectAssigned(loggedinUsername))
                {
                    if (GetStudentGroupId(loggedinUsername))
                    {
                        pnlStudentGrades.Visible = true;

                        if (SetSupervisorNames(fypId))
                        {
                            PopulateSupervisorRequirementGrades();
                        }

                        if (readerUsername == string.Empty) // No reader assigned.
                        {
                            txtReader.Text = noReaderAssigned;
                            txtReader.ForeColor = Color.Blue;
                        }
                        else // Set the reader name and grades.
                        {
                            SetReaderName();
                            PopulateReaderRequirementGrades();
                        }
                    }
                }
            }
        }

        protected void GvReaderGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 4)
            {
                // GridView columns: 0-PROPOSALREPORT, 1-PROGRESSREPORT, 2-FINALREPORT, 3-PRESENTATION
                int proposalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PROPOSALREPORT", lblReaderGradesMessage); // index 0
                int progressReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PROGRESSREPORT", lblReaderGradesMessage); // index 1
                int finalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "FINALREPORT", lblReaderGradesMessage);       // index 2
                int presentationColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 23", "PRESENTATION", lblReaderGradesMessage);     // index 3

                if (proposalReportColumn != -1 && progressReportColumn != -1 && finalReportColumn != -1 && presentationColumn != -1)
                {
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

        protected void GvSupervisorGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 4)
            {
                // GridView columns: 0-PROPOSALREPORT, 1-PROGRESSREPORT, 2-FINALREPORT, 3-PRESENTATION
                int proposalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PROPOSALREPORT", lblSupervisorGradesMessage); // index 0
                int progressReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PROGRESSREPORT", lblSupervisorGradesMessage); // index 1
                int finalReportColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "FINALREPORT", lblSupervisorGradesMessage);       // index 2
                int presentationColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 22", "PRESENTATION", lblSupervisorGradesMessage);     // index 3

                if (proposalReportColumn != -1 && progressReportColumn != -1 && finalReportColumn != -1 && presentationColumn != -1)
                {
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
    }
}
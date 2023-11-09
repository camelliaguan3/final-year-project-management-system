using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Coordinator
{
    public partial class AssignReaders : System.Web.UI.Page
    {
        //***************************************************
        // Uses TODO 03, TODO 07, TODO 42, TODO 43, TODO 44 *
        //***************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();

        /***** Private Methods *****/

        private DataTable GetFYPSupervisors(string fypId) // Uses TODO 03
        {
            //*************************************************
            // Uses TODO 03 to get the supervisors of an FYP. *
            //*************************************************
            DataTable dtFYPSupervisors = myFYPMSDB.GetFYPSupervisors(fypId);

            if (myHelpers.IsQueryResultValid("TODO 03",
                                             dtFYPSupervisors,
                                             new List<string> { "USERNAME", "NAME" },
                                             lblGetSupervisorsMessage))
            {
                if (dtFYPSupervisors.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    dtFYPSupervisors = null;
                    myHelpers.DisplayMessage(lblGetSupervisorsMessage, $"{queryError}TODO 03{queryErrorNoRecordsRetrieved}");
                }
                else // Save the supervisor data in ViewState for subsequent processing.
                {
                    ViewState["dtFYPSupervisors"] = dtFYPSupervisors;
                }
            }

            return dtFYPSupervisors;
        }

        private void PopulateAvailableReaders(string fypId) // Uses TODO 07
        {
            // Used to exclude the faculty who are the supervisors of the FYP.
            DataTable dtFYPSupervisors = GetFYPSupervisors(fypId);

            if (dtFYPSupervisors != null)
            {
                //*******************************************
                // Uses TODO 07 to get all faculty records. *
                //*******************************************
                DataTable dtAvailableReaders = myFYPMSDB.GetFaculty();

                // Use the query result if it is valid.
                if (myHelpers.IsQueryResultValid("TODO 07",
                                                 dtAvailableReaders,
                                                 new List<string> { "USERNAME", "NAME" },
                                                 lblAvailableReadersMessage))
                {
                    if (dtAvailableReaders.Rows.Count == 0) // No record was retrieved -> query error.
                    { myHelpers.DisplayMessage(lblAvailableReadersMessage, $"{queryError}TODO 07{queryErrorNoRecordsRetrieved}"); }
                    else // Remove the supervisors from the result and populate a GridView with remaining faculty.
                    {
                        if (dtFYPSupervisors != null)
                        {
                            // Remove the supervisors from the list of faculty.
                            foreach (DataRow row in dtFYPSupervisors.Rows)
                            { dtAvailableReaders = myHelpers.RemoveSupervisor(dtAvailableReaders, row["USERNAME"].ToString().Trim()); }
                        }

                        // Populate a GridView with the remaining faculty.
                        gvAvailableReaders.DataSource = dtAvailableReaders;
                        gvAvailableReaders.DataBind();
                        gvAvailableReaders.Visible = true;
                    }
                }

                // Populate a Gridview with the project groups without readers.
                gvProjectGroupsWithoutReaders.DataSource = ViewState["dtProjectGroupsWithoutReaders"] as DataTable;
                gvProjectGroupsWithoutReaders.DataBind();
                pnlAssignReader.Visible = true;
            }
        }

        private void PopulateProjectGroupsWithoutReaders() // Uses TODO 42
        {
            //**********************************************************************************************
            // Uses TODO 42 to populate a GridView with project groups that do not have a reader assigned. *
            //**********************************************************************************************
            DataTable dtProjectGroupsWithoutReaders = myFYPMSDB.GetProjectGroupsWithoutReaders();

            if (myHelpers.PopulateGridView("TODO 42",
                                           gvProjectGroupsWithoutReaders,
                                           dtProjectGroupsWithoutReaders,
                                           new List<string> { "GROUPID", "GROUPCODE", "ASSIGNEDFYPID", "TITLE", "CATEGORY", "TYPE" },
                                           lblProjectGroupsWithoutReadersMessage,
                                           lblProjectGroupsWithoutReadersMessage,
                                           noProjectGroupsNeedReader))
            {
                if (!isEmptyQueryResult)
                {
                    ViewState["dtProjectGroupsWithoutReaders"] = dtProjectGroupsWithoutReaders;
                }
            }
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                PopulateProjectGroupsWithoutReaders();
            }
        }

        protected void GvAvailableReaders_RowDataBound(object sender, GridViewRowEventArgs e) // Uses TODO 43
        {
            if (e.Row.Controls.Count == 3)
            {
                // Offset by 1 due to Select column.
                // GridView columns: 0-Select hyperlink, 1-USERNAME, 2-NAME
                int selectHyperlinkColumn = 0;                                                                                              // index 0
                int usernameColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 07", "USERNAME", lblAvailableReadersMessage) + 1; // index 1

                if (usernameColumn != 0)
                {
                    e.Row.Cells[usernameColumn].Visible = false;

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[selectHyperlinkColumn].Text = "ACTION";
                        TableHeaderCell headerCell = new TableHeaderCell { Text = "GROUPS ASSIGNED" };
                        e.Row.Cells.Add(headerCell);
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        TableCell dataCell = new TableCell();

                        //*********************************************************************************************
                        // Uses TODO 43 to get the number of project groups to which a faculty is assigned as reader. *
                        //*********************************************************************************************
                        decimal numberOfGroupsAssigned = myFYPMSDB.GetNumberOfProjectGroupsAssignedToReader(e.Row.Cells[usernameColumn].Text);

                        if (numberOfGroupsAssigned != -1)
                        {
                            dataCell.Text = numberOfGroupsAssigned.ToString();
                            dataCell.HorizontalAlign = HorizontalAlign.Center;
                        }
                        else // An SQL error occurred.
                        {
                            dataCell.Text = string.Empty;
                            myHelpers.DisplayMessage(lblAvailableReadersMessage, sqlErrorMessage);
                        }

                        e.Row.Cells.Add(dataCell);

                        // Disable the Select link if an SQL or query error occurred.
                        if (isSqlError || isQueryError) { e.Row.Cells[selectHyperlinkColumn].Text = "Select"; }
                    }
                }
            }
        }

        protected void GvAvailableReaders_SelectedIndexChanged(object sender, EventArgs e) // Uses TODO 44
        {
            string facultyUsername = gvAvailableReaders.SelectedRow.Cells[1].Text;    // The username is column 1 in gvAvailableReaders.
            string groupId = gvProjectGroupsWithoutReaders.SelectedRow.Cells[1].Text; // The group id is column 1 in gvProjectGroupsWithoutReaders.

            //******************************************************
            // Uses TODO 44 to assign a reader to a project group. *
            //******************************************************
            if (myFYPMSDB.AssignReaderToProjectGroup(facultyUsername, groupId))
            {
                PopulateProjectGroupsWithoutReaders();
                pnlAssignReader.Visible = false;
            }
            else // An SQL error occurred.
            {
                myHelpers.DisplayMessage(lblAvailableReadersMessage, sqlErrorMessage);
                PopulateAvailableReaders(ViewState["fypId"].ToString());
            }
        }

        protected void GvProjectGroupsWithoutReaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                // Offset by 1 due to Select column.
                // GridView columns: 0-Select hyperlink, 1-GROUPID, 2-GROUPCODE, 3-ASSIGNEDFYPID, 4-TITLE, 5-CATEGORY, 6-TYPE
                int selectHyperlinkColumn = 0;
                int groupIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 42", "GROUPID", lblProjectGroupsWithoutReadersMessage) + 1;             // index 1
                int groupCodeColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 42", "GROUPCODE", lblProjectGroupsWithoutReadersMessage) + 1;         // index 2
                int assignedFYPIdColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 42", "ASSIGNEDFYPID", lblProjectGroupsWithoutReadersMessage) + 1; // index 3

                if (groupIdColumn != 0 && groupCodeColumn != 0 && assignedFYPIdColumn != 0)
                {
                    e.Row.Cells[groupIdColumn].Visible = false;
                    e.Row.Cells[assignedFYPIdColumn].Visible = false;

                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[selectHyperlinkColumn].Text = "ACTION";
                        myHelpers.RenameGridViewColumn(e, "GROUPCODE", "GROUP");
                        TableHeaderCell headerCell = new TableHeaderCell { Text = "SUPERVISOR(S)" };
                        e.Row.Cells.Add(headerCell);
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[groupCodeColumn].HorizontalAlign = HorizontalAlign.Center;

                        // Get the supervisors of the project.
                        DataTable dtProjectSupervisors = GetFYPSupervisors(e.Row.Cells[assignedFYPIdColumn].Text);

                        // Populate the project supervisors; set to empty if an error occurred getting the supervisors.
                        TableCell dataCell = new TableCell { Text = string.Empty };
                        if (dtProjectSupervisors != null)
                        {
                            dataCell.Text = StringExtensions.DataTableColumnToString(dtProjectSupervisors, "NAME", ";");
                        }
                        e.Row.Cells.Add(dataCell);
                    }
                }
            }
        }

        protected void GvProjectGroupsWithoutReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fypId = gvProjectGroupsWithoutReaders.SelectedRow.Cells[3].Text;      // ASSIGNEDFYPID is index 3
            
            txtSelectedGroup.Text = $"{gvProjectGroupsWithoutReaders.SelectedRow.Cells[2].Text} - " +
                $"{gvProjectGroupsWithoutReaders.SelectedRow.Cells[4].Text}"; // GROUPCODE is index 2; TITLE is index 4
            ViewState["fypId"] = fypId;

            PopulateAvailableReaders(fypId);
        }
    }
}
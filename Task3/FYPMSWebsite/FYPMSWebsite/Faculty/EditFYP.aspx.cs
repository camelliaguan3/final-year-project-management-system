using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Collections.Generic;

namespace FYPMSWebsite.Faculty
{
    public partial class EditFYP : System.Web.UI.Page
    {
        //*********************************************************************
        // Uses TODO 02, TODO 04, TODO 07, TODO 08, TODO 10, TODO 11, TODO 12 *
        //*********************************************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly DBHelperMethods myDBHelpers = new DBHelperMethods();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;

        /***** Private Methods *****/

        private bool PopulateCosupervisor(string fypId) // Uses TODO 07
        {
            bool result = false;

            //*************************************************************************
            // Uses TODO 07 to populate a dropdown list with potential cosupervisors. *
            //*************************************************************************
            DataTable dtPossibleCosupervisors = myFYPMSDB.GetFaculty();

            if (myHelpers.IsQueryResultValid("TODO 07",
                                             dtPossibleCosupervisors,
                                             new List<string> { "USERNAME", "NAME" },
                                             lblSelectCosupervisorMessage))
            {
                // Remove the logged in faculty from the list of potential cosupervisors.
                dtPossibleCosupervisors = myHelpers.RemoveSupervisor(dtPossibleCosupervisors, loggedinUsername);

                if (myHelpers.PopulateDropDownList("TODO 07",
                                                   noCosupervisor,
                                                   ddlCosupervisor,
                                                   dtPossibleCosupervisors,
                                                   new List<string> { "USERNAME", "NAME" },
                                                   lblSelectCosupervisorMessage,
                                                   lblSelectCosupervisorMessage,
                                                   queryErrorNoRecordsRetrieved,
                                                   EmptyQueryResultMessageType.QueryError))
                {
                    result = SetCosupervisor(fypId);
                }
            }

            return result;
        }

        private bool PopulateFYPCategory() // Uses TODO 04
        {
            bool result = false;

            //********************************************************************
            // Uses TODO 04 to populate a dropdown list with the FYP categories. *
            //********************************************************************
            if (myHelpers.PopulateDropDownList("TODO 04",
                                               string.Empty,
                                               ddlCategory,
                                               myFYPMSDB.GetFYPCategories(),
                                               new List<string> { "CATEGORY" },
                                               lblSelectCategoryMessage,
                                               lblSelectCategoryMessage,
                                               queryErrorNoRecordsRetrieved,
                                               EmptyQueryResultMessageType.QueryError))
            {
                ddlCategory.Items.FindByText("-- Select --").Text = "-- Select to change category --";
                result = true;
            }

            return result;
        }

        private bool PopulateFYPDetails(string fypId) // Uses TODO 02
        {
            bool result = false;
            //*********************************************
            // Uses TODO 02 to get the details of an FYP. *
            //*********************************************
            DataTable dtFYPs = myFYPMSDB.GetFYPDetails(fypId);

            if (myHelpers.IsQueryResultValid("TODO 02",
                                             dtFYPs,
                                             new List<string> { "FYPID", "TITLE", "DESCRIPTION", "CATEGORY", "TYPE",
                                                     "OTHERREQUIREMENTS", "MINSTUDENTS", "MAXSTUDENTS", "STATUS" },
                                             lblEditMessage))
            {
                if (dtFYPs.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblEditMessage, $"{queryError}TODO 02{queryErrorNoRecordsRetrieved}");
                }
                else if (dtFYPs.Rows.Count == 1) // Only one record was retrieved -> populate the FYP details.
                {
                    ViewState["fypId"] = dtFYPs.Rows[0]["FYPID"].ToString().Trim();
                    ViewState["oldTitle"] = txtTitle.Text = dtFYPs.Rows[0]["TITLE"].ToString().Trim();
                    ViewState["oldDescription"] = txtDescription.Text = dtFYPs.Rows[0]["DESCRIPTION"].ToString().Trim();
                    ViewState["oldCategory"] = txtCategory.Text = dtFYPs.Rows[0]["CATEGORY"].ToString().Trim();
                    ViewState["oldType"] = rblType.SelectedValue = dtFYPs.Rows[0]["TYPE"].ToString().Trim();
                    ViewState["oldOtherRequirements"] = txtOtherRequirements.Text = dtFYPs.Rows[0]["OTHERREQUIREMENTS"].ToString().Trim();
                    ViewState["oldMinStudents"] = rblMinStudents.SelectedValue = dtFYPs.Rows[0]["MINSTUDENTS"].ToString().Trim();
                    ViewState["oldMaxStudents"] = rblMaxStudents.SelectedValue = dtFYPs.Rows[0]["MAXSTUDENTS"].ToString().Trim();
                    ViewState["oldStatus"] = rblStatus.SelectedValue = dtFYPs.Rows[0]["STATUS"].ToString().Trim();
                    // Show the radio buttons.
                    result = rblType.Visible = rblMinStudents.Visible = rblMaxStudents.Visible = rblStatus.Visible = true;
                }
                else // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblEditMessage, $"{queryError}TODO 02{queryErrorMultipleRecordsRetrieved}");
                }
            }

            return result;
        }

        private bool ProjectInformationIsChanged(DataTable dtOldNewProjectValues)
        {
            bool result = false;

            foreach (DataRow row in dtOldNewProjectValues.Rows)
            {
                if (!row["oldValue"].ToString().Equals(row["newValue"].ToString()))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool SetCosupervisor(string fypId) // Uses TODO 08
        {
            bool result = false;

            //*************************************************
            // Uses TODO 08 to get the cosupervisor of an FYP *
            //*************************************************
            DataTable dtCosupervisor = myFYPMSDB.GetCosupervisor(fypId, loggedinUsername);

            if (myHelpers.IsQueryResultValid("TODO 08", dtCosupervisor, new List<string> { "USERNAME" }, lblSetCosupervisorMessage))
            {
                if (dtCosupervisor.Rows.Count == 0) // No record was retrieved -> there is no cosupervisor.
                {
                    ViewState["oldCosupervisor"] = string.Empty;
                    result = true;
                }
                else if (dtCosupervisor.Rows.Count == 1) // One record was retrieved -> set cosupervisor in dropdown list.
                {
                    // NOTE: Do not trim username as it needs to match with dropdown list username.
                    ViewState["oldCosupervisor"] = ddlCosupervisor.SelectedValue = dtCosupervisor.Rows[0]["USERNAME"].ToString();
                    result = true;
                }
                else  // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblSetCosupervisorMessage, $"{queryError}TODO 02{queryErrorMultipleRecordsRetrieved}");
                }
            }
            else // An SQL error occurred; clear the cosupervisor dropdown list.
            {
                ddlCosupervisor.Items.Clear();
            }

            return result;
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                ViewState["GoBackTo"] = Request.UrlReferrer;

                if (PopulateCosupervisor(Request["fypId"]) & PopulateFYPCategory() & PopulateFYPDetails(Request["fypId"]))
                {
                    btnUpdate.Enabled = true;
                }
            }
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["GoBackTo"].ToString());
        }

        protected void BtnUpdate_Click(object sender, EventArgs e) // Uses TODO 10, TODO 11, TODO 12
        {
            if (IsValid)
            {
                string fypId = ViewState["fypId"].ToString();

                // Collect the updated FYP values.
                string newTitle = txtTitle.Text.Trim();
                string newDescription = txtDescription.Text.Trim();
                string newCategory = txtCategory.Text.Trim();
                string newType = rblType.SelectedValue;
                string newOtherRequirements = txtOtherRequirements.Text.Trim();
                string newMinStudents = rblMinStudents.SelectedValue;
                string newMaxStudents = rblMaxStudents.SelectedValue;
                string newStatus = rblStatus.SelectedValue;
                string newCosupervisor = ddlCosupervisor.SelectedValue;

                // Collect the old and new FYP values into a DataTable.
                DataTable dtOldNewProjectValues = new DataTable();
                dtOldNewProjectValues.Columns.Add("oldValue", typeof(string));
                dtOldNewProjectValues.Columns.Add("newValue", typeof(string));
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldTitle"].ToString(), newTitle });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldDescription"].ToString(), newDescription });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldCategory"].ToString(), newCategory });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldType"].ToString(), newType });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldOtherRequirements"].ToString(), newOtherRequirements });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldMinStudents"].ToString(), newMinStudents });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldMaxStudents"].ToString(), newMaxStudents });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldStatus"].ToString(), newStatus });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldCosupervisor"].ToString(), newCosupervisor });

                // Update the FYP information if it has changed.
                if (ProjectInformationIsChanged(dtOldNewProjectValues))
                {
                    //*********************************************************************************************
                    // Uses TODO 10, TODO 11, TODO 12 in App_Code\DBHelperMethods to change an FYP's information. *
                    //*********************************************************************************************
                    if (myDBHelpers.UpdateFYP(fypId,
                                              StringExtensions.CleanInput(newTitle),
                                              StringExtensions.CleanInput(newDescription),
                                              StringExtensions.CleanInput(newCategory),
                                              newType,
                                              StringExtensions.CleanInput(newOtherRequirements),
                                              newMinStudents,
                                              newMaxStudents,
                                              newStatus,
                                              ViewState["oldCosupervisor"].ToString(),
                                              newCosupervisor))
                    {
                        Response.Redirect(editFYPsUrl);
                    }
                    else // An SQL error occurred.
                    {
                        myHelpers.DisplayMessage(lblEditMessage, sqlErrorMessage);
                        btnUpdate.Enabled = false;
                    }
                }
                else // No values were changed.
                {
                    myHelpers.DisplayMessage(lblEditMessage, informationNotChanged);
                }
            }
        }

        protected void DdlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedIndex == 0)
            {
                txtCategory.Text = string.Empty;
            }
            else
            {
                txtCategory.Text = ddlCategory.SelectedValue;
            }
        }

        protected void RblMaxStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMaxStudents.SelectedValue = "1";
            }

            CvMinMaxStudents.Validate();
            Page.Validate();
        }

        protected void RblMinStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMinStudents.SelectedValue = "1";
            }

            CvMinMaxStudents.Validate();
            Page.Validate();
        }

        protected void RblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMinStudents.SelectedValue = rblMaxStudents.SelectedValue = "1";
            }
        }
    }
}
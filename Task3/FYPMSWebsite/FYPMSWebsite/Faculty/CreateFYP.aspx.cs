using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace FYPMSWebsite.Faculty
{
    public partial class CreateFYP : Page
    {
        //******************************************
        // USES TODO 04, TODO 07, TODO 09, TODO 11 *
        //******************************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly DBHelperMethods myDBHelpers = new DBHelperMethods();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;

        /***** Private Methods *****/

        private bool PopulateCosupervisor() // Uses TODO 07
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
                    result = true;
                }
            }

            return result;
        }

        private bool PopulateFYPCategory() // Uses TODO 04
        {
            bool result = false;

            //****************************************************************
            // Uses TODO 04 to populate a dropdown list with FYP categories. *
            //****************************************************************
            if (myHelpers.PopulateDropDownList("TODO 04",
                                               string.Empty,
                                               DdlCategory,
                                               myFYPMSDB.GetFYPCategories(),
                                               new List<string> { "CATEGORY" },
                                               lblSelectCategoryMessage,
                                               lblSelectCategoryMessage,
                                               queryErrorNoRecordsRetrieved,
                                               EmptyQueryResultMessageType.QueryError))
            {
                result = true;
            }

            return result;
        }

        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                if (PopulateCosupervisor() & PopulateFYPCategory())
                {
                    btnCreate.Enabled = true;
                }
            }
        }

        protected void BtnCreate_Click(object sender, EventArgs e) // Uses TODO 09, TODO 11
        {
            if (Page.IsValid)
            {
                // Collect the FYP information.
                string title = StringExtensions.CleanInput(txtTitle.Text);
                string description = StringExtensions.CleanInput(txtDescription.Text);
                string cosupervisor = ddlCosupervisor.SelectedValue.Trim();
                string category = StringExtensions.CleanInput(txtCategory.Text);
                string type = rblType.SelectedValue;
                string otherRequirements = StringExtensions.CleanInput(txtOtherRequirements.Text);
                string minStudents = rblMinStudents.SelectedValue;
                string maxStudents = rblMaxStudents.SelectedValue;
                string status = rblStatus.SelectedValue;
                string fypId = myDBHelpers.GetNextTableId("FYP", "fypId", lblCreateMessage);

                if (fypId != string.Empty)
                {
                    //Create the FYP and Supervises records. Uses TODO 09 and TODO 11 in App_Code\DBHelperMethods.
                    if (myDBHelpers.CreateFYP(fypId, title, description, category, type,
                        otherRequirements, minStudents, maxStudents, status, loggedinUsername, cosupervisor))
                    {
                        Response.Redirect("~/Faculty/DisplayEditFYPs.aspx");
                    }
                    else // An SQL error occurred.
                    {
                        btnCreate.Enabled = false;
                        myHelpers.DisplayMessage(lblCreateMessage, sqlErrorMessage);
                    }
                }
            }
        }

        protected void DdlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlCategory.SelectedIndex == 0)
            {
                txtCategory.Text = string.Empty;
            }
            else
            {
                txtCategory.Text = DdlCategory.SelectedValue;
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
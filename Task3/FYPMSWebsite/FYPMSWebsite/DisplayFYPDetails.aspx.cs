using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;

namespace FYPMSWebsite
{
    public partial class DisplayFYPDetails : System.Web.UI.Page
    {
        //************************
        // Uses TODO 02, TODO 03 *
        //************************
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();

        /***** Private Methods *****/

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
                                             lblGetFYPDetailsMessage))
            {
                if (dtFYPs.Rows.Count == 0) // No record was retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGetFYPDetailsMessage, $"{queryError}TODO 02{queryErrorNoRecordsRetrieved}");
                }
                else if (dtFYPs.Rows.Count == 1) // Only one record was retrieved -> populate the webform.
                {
                    txtTitle.Text = dtFYPs.Rows[0]["TITLE"].ToString().Trim();
                    txtDescription.Text = dtFYPs.Rows[0]["DESCRIPTION"].ToString().Trim();
                    txtCategory.Text = dtFYPs.Rows[0]["CATEGORY"].ToString().Trim();
                    txtType.Text = dtFYPs.Rows[0]["TYPE"].ToString().Trim();
                    txtOtherRequirements.Text = dtFYPs.Rows[0]["OTHERREQUIREMENTS"].ToString().Trim();
                    txtMinStudents.Text = dtFYPs.Rows[0]["MINSTUDENTS"].ToString().Trim();
                    txtMaxStudents.Text = dtFYPs.Rows[0]["MAXSTUDENTS"].ToString().Trim();
                    txtStatus.Text = dtFYPs.Rows[0]["STATUS"].ToString().Trim();
                    result = true;
                }
                else  // Multiple records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGetFYPDetailsMessage, $"{queryError}TODO 02{queryErrorMultipleRecordsRetrieved}");
                }
            }

            return result;
        }

        private bool PopulateSupervisors(string fypId) // Uses TODO 03
        {
            bool result = false;

            //*************************************************
            // Uses TODO 03 to get the supervisors of an FYP. *
            //*************************************************
            DataTable dtSupervisors = myFYPMSDB.GetFYPSupervisors(fypId);

            if (myHelpers.IsQueryResultValid("TODO 03",
                                             dtSupervisors,
                                             new List<string> { "USERNAME", "NAME" },
                                             lblGetSupervisorsMessage))
            {
                if (dtSupervisors.Rows.Count == 0) // No records were retrieved -> query error.
                {
                    myHelpers.DisplayMessage(lblGetSupervisorsMessage, $"{queryError}TODO 03{queryErrorNoRecordsRetrieved}");
                }
                else // One or more records were retrieved -> populate the supervisor name(s).
                {
                    txtSupervisor.Text = StringExtensions.DataTableColumnToString(dtSupervisors, "NAME", ",");
                    txtSupervisor.ForeColor = Color.Black;
                    result = true;
                }
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

                if (!string.IsNullOrEmpty(Request["fypId"]))
                {
                    if (PopulateFYPDetails(Request["fypId"]) & PopulateSupervisors(Request["fypId"]))
                    {
                        // Determine from where the request to display the FYP details originated.
                        // If from Display/Edit of Faculty, then show cannot edit FYP message.
                        if (Request.UrlReferrer.ToString().Contains("Faculty/DisplayEditFYPs"))
                        {
                            myHelpers.DisplayMessage(lblGetFYPDetailsMessage, cannotEditFYP);
                        }
                    }
                }
            }
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["GoBackTo"].ToString());
        }
    }
}
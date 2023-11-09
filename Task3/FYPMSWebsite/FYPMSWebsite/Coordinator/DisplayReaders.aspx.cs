using static FYPMSWebsite.Global;
using FYPMSWebsite.App_Code;
using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.Coordinator
{
    public partial class DisplayReaders : System.Web.UI.Page
    {
        //***************
        // Uses TODO 45 *
        //***************
        private FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private HelperMethods myHelpers = new HelperMethods();

        //***** Private Methods *****

        private void PopulateAssignedReaders() // Uses TODO 45
        {
            //********************************************************************************************
            // Uses TODO 45 to populate a GridView with the names of readers assigned to project groups. *
            //********************************************************************************************
            if (myHelpers.PopulateGridView("TODO 45",
                                           gvAssignedReaders,
                                           myFYPMSDB.GetAssignedReaders(),
                                           new List<string> { "NAME", "GROUPCODE", "TITLE" },
                                           lblAssignedReadersMessage,
                                           lblAssignedReadersMessage,
                                           noAssignedReaders))
            {; } // No action needed.
        }

        //***** Protcted Methods *****

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isSqlError = isQueryError = false;
                PopulateAssignedReaders();
            }
        }

        protected void GvAssignedReaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 3)
            {
                // GridView columns: 0-NAME, 1-GROUPCODE,  2-TITLE
                int groupCodeColumn = myHelpers.GetGridViewColumnIndexByName(sender, "TODO 45", "GROUPCODE", lblAssignedReadersMessage); // index 1

                if (groupCodeColumn != -1)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        myHelpers.RenameGridViewColumn(e, "NAME", "READER");
                        myHelpers.RenameGridViewColumn(e, "GROUPCODE", "GROUP");
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[groupCodeColumn].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
    }
}
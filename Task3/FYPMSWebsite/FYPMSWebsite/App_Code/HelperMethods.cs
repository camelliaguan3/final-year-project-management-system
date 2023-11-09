using static FYPMSWebsite.Global;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Helpers for the website project.
    /// </summary>

    public class HelperMethods
    {
        public void DisplayMessage(Label labelControl, string message)
        {
            labelControl.ForeColor = Color.Red; // Error message color.
            labelControl.Visible = true;

            if (!string.IsNullOrEmpty(message))
            {
                if (message.Substring(0, 3) != "***") // Information message.
                {
                    labelControl.ForeColor = Color.Blue;
                }
                labelControl.Text = message;
            }
            else // Error message was not set; should not happen!
            {
                labelControl.Text = emptyOrNullErrorMessage;
            }
        }

        public int GetGridViewColumnIndexByName(object sender, string TODO, string attributeName, Label labelControl)
        {
            DataTable dt = (DataTable)((GridView)sender).DataSource;

            if (dt != null)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Trim() == attributeName.ToUpper().Trim())
                    {
                        return i;
                    }
                }
                DisplayMessage(labelControl, $"*** SQL error in {TODO}: The attribute {attributeName} is missing in the query result.");
            }
            return -1;
        }

        public bool IsQueryResultValid(string TODO, DataTable datatableToCheck, List<string> columnNames, Label labelControl)
        {
            bool result = false;

            if (datatableToCheck != null)
            {
                if (datatableToCheck.Columns != null && datatableToCheck.Columns.Count == columnNames.Count)
                {
                    // Check that the first retrieved column is the first in the list of attributes.
                    if (datatableToCheck.Columns.IndexOf(columnNames[0]) == 0 || columnNames[0] == "ANYNAME")
                    {
                        result = true;

                        // Check if the query result contains the required attributes.
                        foreach (string columnName in columnNames)
                        {
                            if ((!datatableToCheck.Columns.Contains(columnName)) && columnName != "ANYNAME")
                            {
                                result = false;
                                isQueryError = true;
                                DisplayMessage(labelControl, $"{queryError}{TODO}: The query does not retrieve the attribute {columnName}.");
                                break;
                            }
                        }
                    }
                    else
                    {
                        isQueryError = true;
                        DisplayMessage(labelControl, $"{queryError}{TODO}: The attribute {columnNames[0]} must be the first attribute in the query result.");
                    }
                }
                else
                {
                    isQueryError = true;
                    if (datatableToCheck.Columns.Count == 1)
                    { DisplayMessage(labelControl, $"{queryError}{TODO}: The query retrieves {datatableToCheck.Columns.Count} attribute while the required number is {columnNames.Count}."); }
                    else
                    { DisplayMessage(labelControl, $"{queryError}{TODO}: The query retrieves {datatableToCheck.Columns.Count} attributes while the required number is {columnNames.Count}."); }
                }
            }
            else // An SQL error occurred.
            {
                DisplayMessage(labelControl, sqlErrorMessage);
            }

            return result;
        }

        public bool IsValidAndInRange(string number, decimal min, decimal max)
        {
            if (decimal.TryParse(number, out decimal n))
            {
                if (min <= n && n <= max)
                {
                    return true;
                }
            }

            return false;
        }

        public bool PopulateDropDownList(string TODO, string initialValue, DropDownList ddlDropDownList, DataTable dtDropDownListData, List<string> columnNames,
            Label lblQueryErrorMessage, Label lblEmptyQueryResultMessage, string queryResultMessage, EmptyQueryResultMessageType emptyResultMessageType)
        {
            /* Parameters:
             * 1. TODO - the number of the TODO that populates the dropdown list (format: "TODO 00").
             * 2. initialValue - the initial value to show in the dropdown list; uses a default if initialValue is the empty string.
             * 3. ddlDropDownList - the name of the dropdown list control that is to be populated.
             * 4. dtDropDownListData - a DataTable returned by the TODO query that is used to populate the dropdown list.
             * 5. columnNames - the names of the columns in the returned DataTable that is used to populate the dropdown list.
             * 6. lblQueryErrorMessage - the label in which to display a message if there is an error in the query and/or database.
             * 7. lblEmptyQueryResultMessage - the label in which to display a message if the query result is empty.
             * 8. queryResultMessage - the message to display, if any, indicating the result of a query.
             * 9. emptyResultMessageType - the type of message if the result is empty; one of { DBError, DBQueryError, SQLError, Information }. */

            bool populateResult = false;
            isEmptyQueryResult = false;

            // Populate the dropdown list with the dropdown list ids and names if the result is not null.
            if (IsQueryResultValid(TODO,
                                   dtDropDownListData,
                                   columnNames,
                                   lblQueryErrorMessage))
            {
                if (dtDropDownListData.Rows.Count != 0) // The query result is not empty.
                {
                    ddlDropDownList.DataSource = dtDropDownListData;
                    ddlDropDownList.DataValueField = columnNames[0]; // The DataValueField is entry 0 in columnNames.
                    if (columnNames.Count == 1)
                    {
                        ddlDropDownList.DataTextField = columnNames[0]; // The DataTextField is entry 0 in columnNames.
                    }
                    else
                    {
                        ddlDropDownList.DataTextField = columnNames[1]; // The DataTextField is entry 1 in columnNames.
                    }

                    ddlDropDownList.DataBind();

                    if (initialValue == string.Empty) // Use the default first entry when a selection is required.
                    {
                        ddlDropDownList.Items.Insert(0, "-- Select --");
                        ddlDropDownList.Items.FindByText("-- Select --").Value = noneSelected;
                    }
                    else // Use a customized first entry when no initial selection is required.
                    {
                        ddlDropDownList.Items.Insert(0, initialValue);
                        ddlDropDownList.Items.FindByText(initialValue).Value = string.Empty;
                    }

                    ddlDropDownList.SelectedIndex = 0;
                    populateResult = true;
                }
                else // The query result is empty; determine what message to show.
                {
                    switch (emptyResultMessageType)
                    {
                        case EmptyQueryResultMessageType.DBError:
                            DisplayMessage(lblQueryErrorMessage, $"{dbError}{TODO}{queryResultMessage}");
                            break;

                        case EmptyQueryResultMessageType.QueryError:
                            DisplayMessage(lblQueryErrorMessage, $"{queryError}{TODO}{queryResultMessage}");
                            break;

                        case EmptyQueryResultMessageType.Information:
                            DisplayMessage(lblEmptyQueryResultMessage, queryResultMessage);
                            break;

                        default:
                            DisplayMessage(lblQueryErrorMessage, $"{populateDDLError}{contact3311rep}");
                            break;
                    }

                    isEmptyQueryResult = true;
                }
            }

            return populateResult;
        }

        public bool PopulateGridView(string TODO, GridView gv, DataTable resultDataTable, List<string> attributeList,
            Label lblSQLQueryErrorMessage, Label lblEmptyQueryResultMessage, string emptyQueryResultMessage)
        {
            /* Parameters:
             * * 1. TODO - the number of the TODO that populates the dropdown list (format: "TODO 00").
             * * 2. gv - the name of the gridview control that is to be populated.
             * * 3. resultDataTable - a DataTable returned by the TODO query that is used to populate the gridview.
             * * 4. attributeList - the names of the columns in the returned DataTable that is used to populate the gridview.
             * * 5. lblQueryErrorMessage - the label in which to display a message if there is an error in the query and/or database.
             * * 6. lblEmptyQueryResultMessage - the label in which to display a message if the query result is empty.
             * * 7. emptyQueryResultMessage - the message to display if the query result is empty. */

            bool result = false;
            isEmptyQueryResult = gv.Visible = false;

            if (IsQueryResultValid(TODO,
                                   resultDataTable,
                                   attributeList,
                                   lblSQLQueryErrorMessage))
            {
                gv.DataSource = resultDataTable;
                gv.DataBind();

                if (resultDataTable.Rows.Count != 0)
                {
                    gv.Visible = true;
                }
                else // The query result is empty. 
                {
                    if (emptyQueryResultMessage != null)
                    {
                        DisplayMessage(lblEmptyQueryResultMessage, emptyQueryResultMessage);
                    }
                    isEmptyQueryResult = true;
                }
                result = true;
            }

            return result;
        }

        public DataTable RemoveDataTableRecord(DataTable dt, string attributeName, string attributeValue, string condition)
        {
            // If the value of attributename in the row of DataTable dt meets the specified condition for attributeValue, then remove the record.
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];

                if (condition == equal)
                {
                    if (dr[attributeName].ToString() == attributeValue)
                    {
                        dr.Delete();
                    }
                }
                else if (condition == notequal)
                {
                    if (dr[attributeName].ToString() != attributeValue)
                    {
                        dr.Delete();
                    }
                }
            }

            dt.AcceptChanges();

            return dt;
        }

        public DataTable RemoveSupervisor(DataTable dtFaculty, string username)
        {
            if (dtFaculty != null)
            {
                // Remove the existing supervisor from the list of potential cosupervisors.
                foreach (DataRow rowFaculty in dtFaculty.Rows)
                {
                    if (rowFaculty["USERNAME"].ToString().Trim().Equals(username))
                    {
                        dtFaculty.Rows.Remove(rowFaculty);
                        return dtFaculty;
                    }
                }
            }

            return dtFaculty;
        }

        public DataTable RenameDataTableColumn(DataTable dt, string fromName, string toName)
        {
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == fromName)
                {
                    column.ColumnName = toName;
                }
            }

            return dt;
        }

        public void RenameGridViewColumn(GridViewRowEventArgs e, string fromName, string toName)
        {
            for (int i = 0; i < e.Row.Controls.Count; i++)
            {
                if (e.Row.Cells[i].Text.ToUpper().Trim() == fromName.ToUpper().Trim())
                {
                    e.Row.Cells[i].Text = toName;
                }
            }
        }

        public void SortDropdownList(ListControl control, bool isAscending)
        {
            List<ListItem> collection;

            if (isAscending)
            {
                collection = control.Items.Cast<ListItem>()
                    .Select(x => x)
                    .OrderBy(x => x.Text)
                    .ToList();
            }
            else
            {
                collection = control.Items.Cast<ListItem>()
                    .Select(x => x)
                    .OrderByDescending(x => x.Text)
                    .ToList();

                control.Items.Clear();
            }

            foreach (ListItem item in collection)
            {
                control.Items.Add(item);
            }
        }

        public DataTable SortGridview(GridView gv, DataTable dt, string SortExpression, string direction)
        {
            DataTable dtsort = dt;

            DataView dv = new DataView(dtsort)
            {
                Sort = SortExpression + direction
            };

            dt = dv.ToTable();
            gv.DataSource = dt;
            gv.DataBind();

            return dt;
        }
    }
}

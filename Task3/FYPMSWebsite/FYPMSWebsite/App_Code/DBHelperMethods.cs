using System.Data;
using Oracle.DataAccess.Client;
using static FYPMSWebsite.Global;

namespace FYPMSWebsite.App_Code
{
    public class DBHelperMethods
    {
        //***************************************************
        // USES TODO 09, TODO 10, TODO 11, TODO 12, TODO 25 *
        //***************************************************
        private readonly OracleDBAccess myOracleDBAccess = new OracleDBAccess();
        private readonly FYPMSDBAccess myFYPMSDB = new FYPMSDBAccess();
        private readonly HelperMethods myHelpers = new HelperMethods();
        private string sql;

        /***** Private Methods *****/

        private bool RemoveFYP(string fypId)
        {
            sql = $"delete from FYP where fypId={fypId}";
            return myOracleDBAccess.SetData("DBHelperMethods/RemoveFYP", sql);
        }

        /***** Public Methods *****/

        public void CleanUpProjectGroups(System.Web.UI.WebControls.Label labelControl)
        {
            // Delete project group records for which there are no students in the group
            // as such records should not be present in the database.
            sql = $"delete from ProjectGroup "
                  + "where groupId not in "
                  + "(select distinct groupid "
                  + "from CSEStudent "
                  + "where groupId is not null)";
            if (!myOracleDBAccess.SetData("DBHelperMethods/CleanUpProjectGroups", sql))
            {
                myHelpers.DisplayMessage(labelControl, $"{sqlErrorMessage}{contact3311rep}");
            }
        }

        public bool CreateFYP(string fypId, string title, string description, string fcategory,
            string type, string otherRequirements, string minStudents, string maxStudents,
            string isAvailable, string supervisor, string cosupervisor) // Uses TODO 09, TODO 11
        {
            //*********************************************************************************************
            // Uses TODO 09 and TODO 11 to create an FYP record and the corresponding Supervises records. *
            //*********************************************************************************************

            // FIRST, create an Oracle transaction.
            OracleTransaction trans = myOracleDBAccess.BeginTransaction("transaction begin for TODO 09 and TODO 11");
            if (trans == null)
            {
                return false;
            }

            // *** Uses TODO 09 *** SECOND, create the FYP record.
            if (!myFYPMSDB.CreateFYP(fypId, title, description, fcategory, type,
                otherRequirements, minStudents, maxStudents, isAvailable, trans))
            {
                myOracleDBAccess.RollbackTransaction("TODO 09", trans);
                return false;
            }

            // *** Uses TODO 11 *** THIRD, create the Supervises record for the supervisor.
            if (!myFYPMSDB.AddSupervisor(supervisor, fypId, trans))
            {
                string createSqlErrorMessage = sqlErrorMessage;
                if (!RemoveFYP(fypId))
                {
                    sqlErrorMessage = $"{createSqlErrorMessage}<br/>{sqlErrorMessage}{contact3311rep}";
                }
                myOracleDBAccess.RollbackTransaction("TODO 11", trans);
                return false;
            }

            // *** Uses TODO 11 *** to create the Supervises record for the cosupervisor, if any.
            if (cosupervisor != string.Empty)
            {
                if (!myFYPMSDB.AddSupervisor(cosupervisor, fypId, trans))
                {
                    string createSqlErrorMessage = sqlErrorMessage;
                    if (!RemoveFYP(fypId))
                    {
                        sqlErrorMessage = $"{createSqlErrorMessage}<br/>{sqlErrorMessage}{contact3311rep}";
                    }
                    myOracleDBAccess.RollbackTransaction("TODO 11", trans);
                    return false;
                }
            }

            myOracleDBAccess.CommitTransaction("transaction commit for TODO 09 and TODO 11", trans);
            return true;
        }

        public bool CreateRequirementGradesRecord(string facultyUsername, DataTable dtGroupMembers) // Uses TODO 25
        {
            // FIRST, create an Oracle Transaction.
            OracleTransaction trans = myOracleDBAccess.BeginTransaction("transaction begin for TODO 25");
            if (trans == null)
            {
                return false;
            }

            // THEN, create a RequirementGrades record for each student in the group.
            foreach (DataRow row in dtGroupMembers.Rows)
            {
                //*******************************************************************
                // Uses TODO 25 to create a RequirementGrades record for a student. *
                //*******************************************************************
                if (!myFYPMSDB.CreateRequirementGrades(facultyUsername, row["USERNAME"].ToString(), "null", "null", "null", "null", trans))
                {
                    myOracleDBAccess.RollbackTransaction("TODO 25", trans);
                    return false;
                }
            }

            myOracleDBAccess.CommitTransaction("transaction commit for TODO 25", trans);
            return true;
        }

        public bool UpdateFYP(string fypId, string title, string description, string category,
            string type, string otherRequirements, string minStudents, string maxStudents,
            string isAvailable, string oldCosupervisor, string newCosupervisor) // Uses TODO 10, TODO 11, TODO 12
        {
            //**********************************************************
            // Uses TODO 10, TODO 11, TODO 12 to change an FYP record. *
            //**********************************************************

            // FIRST, create an Oracle transaction.
            OracleTransaction trans = myOracleDBAccess.BeginTransaction("transaction begin for TODO 10, TODO 11 and TODO 12");
            if (trans == null)
            {
                return false;
            }

            // SECOND, uses TODO 10 to update the project values.
            if (!myFYPMSDB.UpdateFYP(fypId, title, description, category, type,
                otherRequirements, minStudents, maxStudents, isAvailable, trans))
            {
                myOracleDBAccess.RollbackTransaction("TODO 10", trans);
                return false;
            }

            // THIRD, update the cosupervisor, if necessary.
            if (oldCosupervisor != newCosupervisor)
            {
                if (oldCosupervisor != string.Empty)
                {
                    // Uses TODO 12 to delete the old cosupervsior for the project from the Supervises table.
                    if (!myFYPMSDB.RemoveSupervisor(oldCosupervisor, fypId, trans))
                    {
                        myOracleDBAccess.RollbackTransaction("TODO 12", trans);
                        return false;
                    }
                }

                if (newCosupervisor != string.Empty)
                {
                    // Uses TODO 11 to insert a new cosupervisor for the project into the Supervises table.
                    if (!myFYPMSDB.AddSupervisor(newCosupervisor, fypId, trans))
                    {
                        myOracleDBAccess.RollbackTransaction("TODO 11", trans);
                        return false;
                    }
                }
            }

            myOracleDBAccess.CommitTransaction("transaction commit for TODO 10, TODO 11 and TODO 12", trans);
            return true;
        }

        public string GetNextTableId(string tableName, string idName, System.Web.UI.WebControls.Label labelControl)
        {
            string id = string.Empty;
            sql = $"select max({idName}) from {tableName}";
            decimal nextId = myOracleDBAccess.GetAggregateValue("DBHelperMethods/GetNextTableId", sql);

            if (nextId != -1)
            {
                id = (nextId + 1).ToString();
            }
            else //An SQL error occurred.
            {
                myHelpers.DisplayMessage(labelControl, $"{sqlErrorMessage}{contact3311rep}");
            }

            return id;
        }

        public FYPRole GetUserRole(string username)
        {
            FYPRole role = FYPRole.None;

            if (username != string.Empty)
            {
                if (username == "coordinator")
                {
                    role = FYPRole.Coordinator;
                }
                else
                {
                    if (myOracleDBAccess.GetAggregateValue("Login", $"select count(*) "
                                                                    + $"from Faculty "
                                                                    + $"where username='{username}'") == 1)
                    {
                        role = FYPRole.Faculty;
                    }
                    else if (myOracleDBAccess.GetAggregateValue("Login", $"select count(*) "
                                                                         + $"from CSEStudent "
                                                                         + $"where username='{username}'") == 1)
                    {
                        role = FYPRole.Student;
                    }
                }
            }
            else // Empty username.
            {
                sqlErrorMessage = $"*** SQL Error in DBHelperMethods/GetUserRole: The username is empty./n "
                                  + $"There is an error in your SQL statement that retrieves the username.";
            }

            return role;
        }
    }
}
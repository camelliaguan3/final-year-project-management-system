using System;
using System.Drawing;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace FYPMSWebsite
{
    public class Global : HttpApplication
    {
        public enum FYPRole { Coordinator, Faculty, Student, None };

        public enum EmptyQueryResultMessageType { DBError, QueryError, SQLError, Information, NoMessage };

        public static bool isEmptyQueryResult = false;
        public static bool isQueryError = false;
        public static bool isSqlError = false;

        public static int maxMembers = 4;
        public static double maxGroups = 5;

        public static string FYPMSPassword = "FYProject1#";

        public static string contact3311rep = " Please contact 3311rep.";
        public static string equal = "==";
        public static string noCosupervisor = "No cosupervisor";
        public static string noMessage = null;
        public static string noneSelected = "none selected";
        public static string notequal = "!=";
        public static string sqlErrorMessage = string.Empty;

        // Response redirect.
        public static string manageProjectGroupUrl = "~/Student/ManageProjectGroup.aspx";
        public static string selectedFYPsUrl = "~/Student/SelectedFYPs.aspx";
        public static string editFYPsUrl = "~/Faculty/DisplayEditFYPs.aspx";

        // Feedback messages.
        public static string cannotEditFYP = "This FYP cannot be edited as one or more project groups have indicated an interest in it.";
        public static string cannotChangeGroup = "The project group cannot be changed.";
        public static string cannotIndicateInterest = "The project group cannot indicate an interest in any FYP. ";
        public static string createOrJoinGroup = " Please create a new group or join an existing group";
        public static string fypNotAvailable = "This FYP is not available for assignment.";
        public static string groupAssignedToFYP = "The group is already assigned to FYP – ";
        public static string groupInterestedInFYP = "The group has indicated an interest in one or more FYPs.";
        public static string groupsSupervised = "Project groups currently supervised: ";
        public static string informationNotChanged = "No project information has been changed.";
        public static string invalidUsername = "There is no student with this username.";
        public static string maxGroupsSupervised = "You are supervising the maximum allowed number of groups.";
        public static string maxGroupSupervisonExceeded = "Cannot assign group as it would exceed the maximum allowed number of supervised groups which is ";
        public static string memberOfAnotherGroup = "This student is a member of another project group.";
        public static string memberOfThisGroup = "This student is already a member of this project group.";
        public static string noAssignedGroups = "There are no groups assigned to FYPs.";
        public static string noAssignedReaders = "There are no project groups with assigned readers.";
        public static string noFYPAssigned = "You are not assigned to an FYP.";
        public static string noFYPInterestIndicated = "Your group has not indicated an interest in any FYP.";
        public static string noFYPSelected = "No FYP was selected.";
        public static string noFYPsToIndicateInterest = "There are no FYPs for which your group can indicate an interest.";
        public static string noFYPsPosted = "You have not posted any FYPs.";
        public static string noGroupsAssigned = "No groups are assigned to this FYP.";
        public static string noGroupsAvailable = "No groups are available to asssign to this FYP.";
        public static string noGroupSelected = "No project group was selected for assignment.";
        public static string noGroupToGrade = "There are no groups to grade.";
        public static string noProjectGroupsNeedReader = "There are no project groups that require a reader.";
        public static string noReaderAssigned = "No reader is assigned.";
        public static string notGraded = "None of your FYP requirements have been graded yet.";
        public static string notGroupMember = "You are not a member of a project group.";
        public static string toIndicateInterest = " to see for which FYPs you can indicate an interest.";

        // Internal error messages.
        public static string emptyOrNullErrorMessage = "*** System error: Empty or null error message. " + contact3311rep;
        public static string populateDDLError = "*** System error: Unknown error in HelperMethods - PopulateDropDownList.";

        // Database error messages.
        public static string dbError = "*** Database error in ";
        public static string dbErrorNoGroupId = ": The group id is null.";

        // Query error messages.
        public static string queryError = "*** Query error in ";
        public static string queryErrorMultipleRecordsRetrieved = ": The query retrieves more than one record.";
        public static string queryErrorNoRecordsRetrieved = ": The query does not retrieve any records.";

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
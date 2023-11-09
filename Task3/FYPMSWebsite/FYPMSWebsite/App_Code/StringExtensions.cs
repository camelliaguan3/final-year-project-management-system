using System.Data;

namespace FYPMSWebsite.App_Code
{
    public static class StringExtensions
    {
        public static string CleanInput(this string text)
        {
            // Replace single quote by two quotes and remove leading and trailing spaces.
            return text.Replace("'", "''").Trim();
        }

        public static string ConcatenateName(string name1, string name2, string seperator)
        {
            // Concatenate two names into a single string with a separator between the names.
            return name1 + seperator + " " + name2;
        }

        public static string DataTableColumnToString(DataTable dt, string columnName, string separator)
        {
            // Concantenate a column of a DataTable into a single string with a separator between the column values.
            string result = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result += dt.Rows[i][columnName].ToString();
                if (i < dt.Rows.Count - 1) { result += separator + " "; }
            }

            return result;
        }
    }
}
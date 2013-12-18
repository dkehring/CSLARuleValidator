using System;
using System.Data;
using System.Data.SqlClient;

namespace WHC.UnitTesting
{
    public class DbUtility
    {
        public static bool IsRecordDeleted(string connString, string tableName, string idColumnName, Guid recordId)
        {
            using (var cn = new SqlConnection(connString))
            {
                cn.Open();

                using (var cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.Text;
                    cm.CommandText = String.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}'", tableName, idColumnName, recordId);

                    var count = (int)cm.ExecuteScalar();
                    return (count == 0);
                }//using
            }//using
        }

        public static string GetConnectionString(string name)
        {
            var val = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            return val == null ? "" : val.ConnectionString;
        }
    }
}

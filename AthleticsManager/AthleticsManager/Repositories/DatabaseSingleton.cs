using Microsoft.Data.SqlClient;
using System.Configuration;

namespace AthleticsManager.Repositories
{
    public class DatabaseSingleton
    {
        private static SqlConnection? conn = null;
        private DatabaseSingleton()
        {

        }
        public static SqlConnection GetInstance()
        {
            try
            {
                if (conn == null)
                {
                    SqlConnectionStringBuilder consStringBuilder = new SqlConnectionStringBuilder();
                    consStringBuilder.UserID = ReadSetting("Name");
                    consStringBuilder.Password = ReadSetting("Password");
                    consStringBuilder.InitialCatalog = ReadSetting("Database");
                    consStringBuilder.DataSource = ReadSetting("DataSource");
                    consStringBuilder.ConnectTimeout = 30;
                    consStringBuilder.TrustServerCertificate = true;
                    conn = new SqlConnection(consStringBuilder.ConnectionString);
                    conn.Open();
                }
                return conn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }        

        public static void CloseConnection()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        private static string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key] ?? "Not Found";
            return result;
        }
    }
}

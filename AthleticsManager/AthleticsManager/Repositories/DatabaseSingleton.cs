using Microsoft.Data.SqlClient;
using System.Configuration;

namespace AthleticsManager.Repositories
{
    /// <summary>
    /// Implements the Singleton pattern to manage the database connection.
    /// This class ensures that only one instance of the SqlConnection is created and shared across the application.
    /// </summary>
    public class DatabaseSingleton
    {
        private static SqlConnection? conn = null;

        /// <summary>
        /// Retrieves the single instance of the database connection.
        /// If the connection does not exist, it reads configuration settings, builds the connection string, and opens a new connection.
        /// </summary>
        /// <returns>The active SqlConnection instance.</returns>
        /// <exception cref="Exception">Thrown when a critical error occurs while connecting to the database.</exception>
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
                System.Windows.MessageBox.Show("Critical error connecting to DB:\n" + ex.Message, "Database error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Closes and disposes of the current database connection.
        /// Checks if the connection is open before closing and resets the instance to null.
        /// This method is essential for proper resource cleanup.
        /// </summary>
        public static void CloseConnection()
        {
            if (conn != null)
            {
                // Only attempt to close if it is actually open
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// Helper method to read specific key values from the application configuration file (App.config).
        /// </summary>
        /// <param name="key">The key of the setting to retrieve.</param>
        /// <returns>The value of the setting, or "Not Found" if the key does not exist.</returns>
        private static string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key] ?? "Not Found";
            return result;
        }
    }
}

using AthleticsManager.Repositories;
using System.Windows;

namespace AthleticsManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Occurs when the application is shutting down.
        /// This override ensures that the database connection is properly closed and resources are released
        /// regardless of how the application is exited (user action or error shutdown).
        /// </summary>
        /// <param name="e">The exit event arguments.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            DatabaseSingleton.CloseConnection();

            base.OnExit(e);
        }
    }

}

using AthleticsManager.Repositories;
using System.Windows;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for displaying aggregate club statistics.
    /// This class handles the loading and presentation of the generated report data within the user interface.
    /// </summary>
    public partial class ReportWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the ReportWindow class.
        /// Sets up the user interface components and immediately triggers the data loading process.
        /// </summary>
        public ReportWindow()
        {
            InitializeComponent();
            LoadReport();
        }

        /// <summary>
        /// Retrieves the club statistics from the repository and binds the data to the report grid.
        /// Encapsulates the data access logic within a try-catch block to handle potential runtime errors gracefully.
        /// </summary>
        private void LoadReport()
        {
            try
            {
                ClubRepositary repo = new ClubRepositary();
                var data = repo.GetClubStatistics();
                DgReport.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the event to close the report window.
        /// Sets the dialog result to true, effectively closing the modal window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}

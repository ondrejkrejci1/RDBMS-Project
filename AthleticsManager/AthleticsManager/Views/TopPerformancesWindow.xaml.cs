using AthleticsManager.Repositories;
using System.Windows;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for displaying the top athletic performances.
    /// This class retrieves high-level statistical data from the repository and presents it in a grid view.
    /// </summary>
    public partial class TopPerformancesWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the TopPerformancesWindow class.
        /// Sets up the user interface components and triggers the loading of top performance data.
        /// </summary>
        public TopPerformancesWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Retrieves the top performance records from the data repository and binds them to the display grid.
        /// Handles any potential exceptions during the data fetching process by displaying an error message.
        /// </summary>
        private void LoadData()
        {
            try
            {
                ResultRepositary repo = new ResultRepositary();
                var data = repo.GetTopPerformances();
                DgTopPerf.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the event to close the window.
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

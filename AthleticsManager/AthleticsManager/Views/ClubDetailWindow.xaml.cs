using System.Windows;
using AthleticsManager.Models;
using AthleticsManager.Models.EnumHelpers;
using AthleticsManager.Repositories;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents a window that displays detailed information about a specific athletic club.
    /// This includes the club's region, total member count, and a roster of registered athletes.
    /// </summary>
    public partial class ClubDetailWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the ClubDetailWindow class.
        /// Sets up the user interface with the club's name and triggers the loading of related data.
        /// </summary>
        /// <param name="club">The specific club object to display details for.</param>
        public ClubDetailWindow(Club club)
        {
            InitializeComponent();
            TxtTitle.Text = club.Name;
            LoadData(club);
        }

        /// <summary>
        /// Handles the event to close the club detail window.
        /// Sets the dialog result to true, effectively closing the modal.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Retrieves the list of athletes associated with the specified club from the repository.
        /// Updates the user interface with the athlete list, total member count, and region information.
        /// </summary>
        /// <param name="club">The club object containing the ID used to fetch related athletes.</param>
        private void LoadData(Club club)
        {
            AthleteRepository athRepo = new AthleteRepository();
            List<Athlete> athletes = athRepo.GetAthletesByClub(club.ClubID);

            DgResults.ItemsSource = athletes;

            TxtNumberOfMembers.Text = athletes.Count.ToString();

            TxtRegion.Text = ((Region)club.RegionID).ToFriendlyString();
        }

    }
}

using System.Windows;
using System.Windows.Controls;
using AthleticsManager.Models;
using AthleticsManager.Repositories;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for creating and adding a new athletic club.
    /// This class handles user input validation, persists the new club to the database,
    /// and optionally updates the parent window if invoked during athlete creation.
    /// </summary>
    public partial class AddClubWindow : Window
    {
        /// <summary>
        /// Gets or sets an optional reference to an open AddAthleteWindow/>.
        /// This allows the newly created club to be immediately reflected in the athlete creation form.
        /// </summary>
        public AddAthleteWindow AddAthleteWindow { set; get; }
        private ClubRepositary clubRepositary;

        /// <summary>
        /// Initializes a new instance of the AddClubWindow class.
        /// Sets up the data repository and initializes the user interface components.
        /// </summary>
        public AddClubWindow()
        {
            clubRepositary = new ClubRepositary();
            InitializeComponent();
        }

        /// <summary>
        /// Handles the submission of the new club form.
        /// Validates the input fields (name and region), persists the new club to the database,
        /// and notifies the parent window if applicable before closing the dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Submit(object sender, RoutedEventArgs e)
        {
            try 
            {
                string clubName = ClubNameTextBox.Text;

                ComboBoxItem selectedItem = (ComboBoxItem)ClubRegionComboBox.SelectedItem;

                if (string.IsNullOrWhiteSpace(clubName))
                {
                    MessageBox.Show("Please enter a valid club name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (selectedItem == null)
                {
                    MessageBox.Show("Please select a region.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int regionID = int.Parse(selectedItem.Tag.ToString());


                Club club = new Club(clubName, regionID);
                club = clubRepositary.CreateNewClub(club);

                if (AddAthleteWindow != null)
                {
                    AddAthleteWindow.NewClubAdded(club);
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new club: {ex.Message}");
            }
        }
    }
}

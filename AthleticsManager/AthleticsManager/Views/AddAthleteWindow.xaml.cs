using AthleticsManager.Models;
using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for registering new athletes into the system.
    /// This class handles data input, validation, and interaction with the database repositories,
    /// including the ability to add new clubs on the fly.
    /// </summary>
    public partial class AddAthleteWindow : Window
    {
        private AddClubWindow addClubWindow;
        private ClubRepositary clubRepositary;
        private AthleteRepository athleteRepositary;

        /// <summary>
        /// Initializes a new instance of the AddAthleteWindow class.
        /// Sets up the necessary data repositories and populates the club selection list from the database.
        /// </summary>
        public AddAthleteWindow()
        {
            athleteRepositary = new AthleteRepository();
            clubRepositary = new ClubRepositary();
            InitializeComponent();
            LoadClubs();
        }

        /// <summary>
        /// Opens a secondary modal window to allow the user to create a new club immediately.
        /// This provides a convenient workflow if the desired club does not yet exist in the selection list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void RegisterNewClub(object sender, RoutedEventArgs e)
        {
            addClubWindow = new AddClubWindow();
            addClubWindow.AddAthleteWindow = this;
            addClubWindow.ShowDialog();
        }

        /// <summary>
        /// Updates the club selection dropdown with a newly created club.
        /// This method is intended to be called externally by the <see cref="AddClubWindow"/> upon successful club creation.
        /// </summary>
        /// <param name="club">The newly created club object to add to the list.</param>
        public void NewClubAdded(Club club)
        {
            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem.Content = club.Name;
            comboBoxItem.Tag = club.ClubID;

            ClubsComboBox.Items.Add(comboBoxItem);
            ClubsComboBox.SelectedIndex = ClubsComboBox.Items.Count - 1;
        }

        /// <summary>
        /// Retrieves the list of available clubs from the repository and populates the dropdown menu for user selection.
        /// </summary>
        private void LoadClubs()
        {
            var clubList = clubRepositary.GetAll();

            foreach (var club in clubList)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = club.Name;
                comboBoxItem.Tag = club.ClubID;

                ClubsComboBox.Items.Add(comboBoxItem);
            }
        }

        /// <summary>
        /// Handles the form submission logic.
        /// Collects user input (name, birthdate, gender, club), creates a new athlete record, 
        /// and attempts to save it to the database via the repository.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = FirstNameTextBox.Text;
                string lastName = LastNameTextBox.Text;
                DateTime dateTime = DateTime.Parse(DateOfBirthSelector.Text);

                ComboBoxItem selectedGenderItem = (ComboBoxItem)GenderSelect.SelectedItem;
                string gender = selectedGenderItem.Content.ToString();
                gender = gender.Substring(0, 1);

                bool isActive = ActiveCheckBox.IsChecked ?? false;

                ComboBoxItem selectedClubItem = (ComboBoxItem)ClubsComboBox.SelectedItem;
                int clubID = (int)selectedClubItem.Tag;

                Athlete athlete = new Athlete(firstName, lastName, dateTime, gender, isActive, clubID);

                athleteRepositary.CreateNewAthlete(athlete);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new athlete: {ex.Message}");
            }
        }
    }
}

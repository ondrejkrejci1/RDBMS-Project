using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for updating existing athlete records.
    /// This class handles user input, validates selection, and communicates with the data repository to modify athlete details.
    /// </summary>
    public partial class UpdateAthleteWindow : Window
    {
        /// <summary>
        /// Reference to the data access repository for handling athlete database operations.
        /// </summary>
        private AthleteRepository athleteRepository ;

        /// <summary>
        /// Initializes a new instance of the UpdateAthleteWindow class.
        /// Sets up the UI components and populates the athlete selection list from the database.
        /// </summary>
        public UpdateAthleteWindow()
        {
            athleteRepository = new AthleteRepository();
            InitializeComponent();
            LoadAthletes();
        }

        /// <summary>
        /// Handles the click event for the confirmation button.
        /// Validates that an athlete is selected and at least one update option is active,
        /// then gathers the input data and executes the update operation in the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            try 
            {
                bool firstNameActive = FirstNameUpdate.IsChecked ?? false;
                bool lastNameActive = LastNameUpdate.IsChecked ?? false;
                bool birthDateActive = DateOfBirthUpdate.IsChecked ?? false;
                bool genderActive = GenderUpdate.IsChecked ?? false;

                if (AthleteSelect.SelectedItem == null)
                {
                    MessageBox.Show("Please select an athlete to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (firstNameActive && string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
                {
                    MessageBox.Show("You selected to update the First Name, but the input field is empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (lastNameActive && string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                {
                    MessageBox.Show("You selected to update the Last Name, but the input field is empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (birthDateActive && DateOfBirthSelector.SelectedDate == null)
                {
                    MessageBox.Show("You selected to update the Date of Birth, but no date is selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (genderActive && GenderSelect.SelectedItem == null)
                {
                    MessageBox.Show("You selected to update the Gender, but no value is selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int athleteID = (int)((ComboBoxItem)AthleteSelect.SelectedItem).Tag;

                string firstName = null;
                string lastName = null;
                DateTime? birthDate = null;
                string gender = null;

                bool? isActive = ActiveCheckBox.IsChecked;

                if (firstNameActive)
                {
                    firstName = FirstNameTextBox.Text;
                }
                if (lastNameActive)
                {
                    lastName = LastNameTextBox.Text;
                }
                if (birthDateActive)
                {
                    birthDate = DateOfBirthSelector.SelectedDate;
                }
                if (genderActive)
                {
                    ComboBoxItem selectedGenderItem = (ComboBoxItem)GenderSelect.SelectedItem;
                    gender = selectedGenderItem.Content.ToString();
                    gender = gender.Substring(0, 1);
                }

                athleteRepository.Update(firstName, lastName, birthDate, gender, isActive, athleteID);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating athlete: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the list of all athletes from the repository and populates the selection dropdown (ComboBox).
        /// Maps the athlete's name and ID to the UI elements.
        /// </summary>
        private void LoadAthletes()
        {
            var athletes = athleteRepository.GetAll();
            foreach (var athlete in athletes)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = $"{athlete.FirstName} {athlete.LastName} ({athlete.BirthDateString()})";
                comboBoxItem.Tag = athlete.AthleteID;
                AthleteSelect.Items.Add(comboBoxItem);
            }
        }

    }
}

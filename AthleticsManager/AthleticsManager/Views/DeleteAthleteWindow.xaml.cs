using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for removing an existing athlete record.
    /// This class handles the selection of an athlete and executes the deletion process via the repository.
    /// </summary>
    public partial class DeleteAthleteWindow : Window
    {
        private AthleteRepository athleteRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteAthleteWindow class.
        /// Sets up the user interface and populates the selection list with current athlete data.
        /// </summary>
        public DeleteAthleteWindow()
        {
            athleteRepository = new AthleteRepository();
            InitializeComponent();
            LoadAthletes();
        }

        /// <summary>
        /// Retrieves the list of all athletes from the repository and populates the selection dropdown.
        /// Formats the display string to include the name and birth date for easy identification.
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

        /// <summary>
        /// Handles the confirmation action to delete the selected athlete.
        /// Retrieves the athlete's ID from the selection, executes the delete operation, and closes the window upon success.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            try
            {
                int athleteID = (int)((ComboBoxItem)AthleteSelect.SelectedItem).Tag;


                athleteRepository.Delete(athleteID);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting athlete: {ex.Message}");
            }
        }
    }
}

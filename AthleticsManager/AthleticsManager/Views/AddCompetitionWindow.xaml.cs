using AthleticsManager.Models;
using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents the window responsible for creating and adding new athletic competitions.
    /// This class handles user input collection and communicates with the repository to persist the new competition record.
    /// </summary>
    public partial class AddCompetitionWindow : Window
    {
        private CompetitionRepositary competitionRepositary;

        /// <summary>
        /// Initializes a new instance of the AddCompetitionWindow class.
        /// Sets up the data repository and initializes the user interface components.
        /// </summary>
        public AddCompetitionWindow()
        {
            competitionRepositary = new CompetitionRepositary();
            InitializeComponent();
        }

        /// <summary>
        /// Handles the submission of the new competition form.
        /// Retrieves values from the input fields, creates a new competition object, and persists it to the database via the repository.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Submit(object sender, RoutedEventArgs e)
        {
            try 
            {
                string name = NameTextBox.Text;
                string venue = VenueTextBox.Text;
                DateTime date = DateTime.Parse(DateSelector.Text);

                ComboBoxItem selectedTypeItem = (ComboBoxItem)TypeSelect.SelectedItem;
                string type = selectedTypeItem.Content.ToString();

                Competition competition = new Competition(name, date, venue, type);

                competition = competitionRepositary.CreateNewCompetition(competition);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new competition: {ex.Message}");
            }
        }

    }
}

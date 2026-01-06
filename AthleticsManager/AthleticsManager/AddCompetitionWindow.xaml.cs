using AthleticsManager.Models;
using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro AddCompetitionWindow.xaml
    /// </summary>
    public partial class AddCompetitionWindow : Window
    {
        private CompetitionRepositary competitionRepositary;

        public AddCompetitionWindow()
        {
            competitionRepositary = new CompetitionRepositary();
            InitializeComponent();
        }
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

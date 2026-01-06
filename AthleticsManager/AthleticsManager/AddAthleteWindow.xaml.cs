using AthleticsManager.Models;
using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro AddAthleteWindow.xaml
    /// </summary>
    public partial class AddAthleteWindow : Window
    {
        private AddClubWindow addClubWindow;
        private ClubRepositary clubRepositary;
        private AthleteRepository athleteRepositary;

        public AddAthleteWindow()
        {
            athleteRepositary = new AthleteRepository();
            clubRepositary = new ClubRepositary();
            InitializeComponent();
            LoadClubs();
        }

        private void RegisterNewClub(object sender, RoutedEventArgs e)
        {
            addClubWindow = new AddClubWindow(this);
            addClubWindow.ShowDialog();
        }

        public void NewClubAdded(Club club)
        {
            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem.Content = club.Name;
            comboBoxItem.Tag = club.ClubID;

            ClubsComboBox.Items.Add(comboBoxItem);
            ClubsComboBox.SelectedIndex = ClubsComboBox.Items.Count - 1;
        }

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

                ComboBoxItem selectedClubItem = (ComboBoxItem)ClubsComboBox.SelectedItem;
                int clubID = (int)selectedClubItem.Tag;

                Athlete athlete = new Athlete(firstName, lastName, dateTime, gender, clubID);

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

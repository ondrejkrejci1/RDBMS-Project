using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro UpdateAthleteWindow.xaml
    /// </summary>
    public partial class UpdateAthleteWindow : Window
    {
        private AthleteRepository athleteRepository ;

        public UpdateAthleteWindow()
        {
            athleteRepository = new AthleteRepository();
            InitializeComponent();
            LoadAthletes();
        }

        private void Confirm(object sender, RoutedEventArgs e)
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

            if (firstNameActive == false && lastNameActive == false && birthDateActive == false && genderActive == false  )
            {
                MessageBox.Show("Please make a change to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int athleteID = (int)((ComboBoxItem)AthleteSelect.SelectedItem).Tag;

            string firstName = null;
            string lastName = null;
            DateTime? birthDate = null;
            string gender = null;

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

            athleteRepository.Update(firstName, lastName, birthDate, gender, athleteID);

            DialogResult = true;
        }

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

using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro DeleteAthleteWindow.xaml
    /// </summary>
    public partial class DeleteAthleteWindow : Window
    {
        private AthleteRepository athleteRepository;

        public DeleteAthleteWindow()
        {
            athleteRepository = new AthleteRepository();
            InitializeComponent();
            LoadAthletes();
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

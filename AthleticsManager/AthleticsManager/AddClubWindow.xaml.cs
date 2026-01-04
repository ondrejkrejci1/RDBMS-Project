using System.Windows;
using System.Windows.Controls;
using AthleticsManager.Models;
using AthleticsManager.Repositories;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro AddClubWindow.xaml
    /// </summary>
    public partial class AddClubWindow : Window
    {
        private AddAthleteWindow addAthleteWindow;
        private ClubRepositary clubRepositary;
        public AddClubWindow(AddAthleteWindow addAthleteWindow)
        {
            this.addAthleteWindow = addAthleteWindow;
            clubRepositary = new ClubRepositary();
            InitializeComponent();
        }


        private void Submit(object sender, RoutedEventArgs e)
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

            addAthleteWindow.NewClubAdded(club);

            DialogResult = true;
        }
    }
}

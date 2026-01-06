using System.Windows;
using AthleticsManager.Models;
using AthleticsManager.Repositories;

namespace AthleticsManager
{
    /// <summary>
    /// Interaction logic for ClubDetailWindow.xaml
    /// </summary>
    public partial class ClubDetailWindow : Window
    {
        public ClubDetailWindow(Club club)
        {
            InitializeComponent();
            TxtTitle.Text = club.Name;
            LoadData(club);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void LoadData(Club club)
        {
            AthleteRepository athRepo = new AthleteRepository();
            List<Athlete> athletes = athRepo.GetAthletesByClub(club.ClubID);

            DgResults.ItemsSource = athletes;

            TxtNumberOfMembers.Text = athletes.Count.ToString();

            RegionRepositary regRepo = new RegionRepositary();
            var regions = regRepo.GetAll();

            var region = regions.FirstOrDefault(r => r.RegionID == club.RegionID);

            TxtRegion.Text = region != null ? region.Name : "Unknown Region";
        }

    }
}

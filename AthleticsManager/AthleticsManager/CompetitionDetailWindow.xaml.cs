using AthleticsManager.Models;
using AthleticsManager.Models.EnumHelpers;
using AthleticsManager.Repositories;
using System.Windows;

namespace AthleticsManager
{
    /// <summary>
    /// Interakční logika pro CompetitionDetailWindow.xaml
    /// </summary>
    public partial class CompetitionDetailWindow : Window
    {
        private Competition competition;

        // Konstruktor přijímá objekt Competition
        public CompetitionDetailWindow(Competition competition)
        {
            InitializeComponent();
            this.competition = competition;

            TxtTitle.Text = competition.Name;
            TxtDate.Text = competition.DateString;
            TxtPlace.Text = competition.Venue;

            LoadResults();
        }

        private void LoadResults()
        {
            try
            {
                ResultRepositary resRepo = new ResultRepositary();

                var rawResults = resRepo.GetAll().Where(r => r.CompetitionID == competition.CompetitionId).ToList();

                if (!rawResults.Any()) return;

                AthleteRepository athRepo = new AthleteRepository();
                var allAthletes = athRepo.GetAll();

                List<CompetitionResultView> viewList = rawResults.Select(r =>
                {
                    var athlete = allAthletes.FirstOrDefault(a => a.AthleteID == r.AthleteID);
                    string athleteFullName = athlete != null ? $"{athlete.FirstName} {athlete.LastName}" : "Unknown Athlete";

                    Discipline discEnum = (Discipline)r.DisciplineID;

                    return new CompetitionResultView
                    {
                        DisciplineName = discEnum.ToFriendlyString(),

                        AthleteName = athleteFullName,

                        Performance = discEnum.FormatPerformance(r.Performance),

                        Unit = discEnum.GetUnit(),

                        Rank = r.Placement ?? 0
                    };
                }).OrderBy(x => x.DisciplineName).ThenBy(x => x.Rank).ToList();

                DgResults.ItemsSource = viewList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading results: " + ex.Message);
            }
        }
    


        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

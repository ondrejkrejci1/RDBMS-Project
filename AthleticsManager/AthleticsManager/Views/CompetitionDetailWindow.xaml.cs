using AthleticsManager.Models;
using AthleticsManager.Models.EnumHelpers;
using AthleticsManager.Repositories;
using System.Windows;

namespace AthleticsManager.Views
{
    /// <summary>
    /// Represents a window that displays detailed information about a specific competition.
    /// This includes the competition's metadata (name, date, venue) and a list of associated athlete results.
    /// </summary>
    public partial class CompetitionDetailWindow : Window
    {
        private Competition competition;

        /// <summary>
        /// Initializes a new instance of the CompetitionDetailWindow class.
        /// Populates the UI with details from the provided competition object and triggers the loading of associated results.
        /// </summary>
        /// <param name="competition">The specific competition object to display details for.</param>
        public CompetitionDetailWindow(Competition competition)
        {
            InitializeComponent();
            this.competition = competition;

            TxtTitle.Text = competition.Name;
            TxtDate.Text = competition.DateString;
            TxtPlace.Text = competition.Venue;

            LoadResults();
        }

        /// <summary>
        /// Retrieves result data associated with the current competition from the repository.
        /// Maps raw result data to a view-friendly format by resolving athlete names and formatting performance metrics based on the discipline.
        /// </summary>
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

        /// <summary>
        /// Handles the event to close the competition detail window.
        /// Sets the dialog result to true, effectively closing the modal.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

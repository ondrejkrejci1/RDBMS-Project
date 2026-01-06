using AthleticsManager.Models;
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
            competition = competition;

            // Vyplnit hlavičku
            TxtTitle.Text = competition.Name;
            TxtDate.Text = competition.DateString;
            TxtPlace.Text = competition.Venue;

            //LoadResults();
        }

        private void LoadResults()
        {
            // Tady potřebujeme získat výsledky. 
            // Protože tabulka Result má jen IDčka, musíme udělat v repozitáři JOIN
            // a vrátit nějaký "View Model" nebo DTO (Data Transfer Object).

            ResultRepositary repo = new ResultRepositary();
            //DgResults.ItemsSource = repo.GetResultsByCompetition(competition.CompetitionId);
        }


        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

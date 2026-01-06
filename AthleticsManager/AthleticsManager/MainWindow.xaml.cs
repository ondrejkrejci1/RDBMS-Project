using AthleticsManager.Models;
using AthleticsManager.Repositories;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // Potřeba pro OpenFileDialog
using System.IO;       // Potřeba pro práci se soubory
using System.Linq;

namespace AthleticsManager
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AthleteRepository athleteRepository;
        private ClubRepositary clubRepositary;
        private CompetitionRepositary competitionRepositary;
        private DisciplineRepositary disciplineRepositary;
        private ResultRepositary resultRepositary;

        private Competition nearestCompetition;
        private List<Competition> allCompetitions;

        private List<Club> allClubsDisplayList;
        private Club topClub;

        public MainWindow()
        {
            InitializeComponent();
            athleteRepository = new AthleteRepository();
            clubRepositary = new ClubRepositary();
            competitionRepositary = new CompetitionRepositary();
            disciplineRepositary = new DisciplineRepositary();
            resultRepositary = new ResultRepositary();
            allCompetitions = new List<Competition>();
        }

        /// <summary>
        /// Handles the navigation logic for the sidebar menu.
        /// </summary>
        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            string viewTag = clickedButton.Tag.ToString();

            HideAllViews();

            switch (viewTag)
            {
                case "Dashboard":
                    TxtPageTitle.Visibility = Visibility.Collapsed;
                    ViewOverview.Visibility = Visibility.Visible;
                    LoadOverviewData();
                    break;

                case "Athletes":
                    TxtPageTitle.Text = "Athletes";
                    TxtPageTitle.Visibility = Visibility.Visible;
                    ViewAthletes.Visibility = Visibility.Visible;
                    LoadAthletesData();
                    break;

                case "Competitions":
                    TxtPageTitle.Text = "Competitions";
                    TxtPageTitle.Visibility = Visibility.Visible;
                    ViewCompetitions.Visibility = Visibility.Visible;
                    LoadCompetitionsData();
                    break;

                case "Results":
                    TxtPageTitle.Text = "Add New Result";
                    TxtPageTitle.Visibility = Visibility.Visible;
                    ViewNewResult.Visibility = Visibility.Visible;
                    LoadComboboxData();
                    break;

                case "Clubs":
                    TxtPageTitle.Text = "Clubs";
                    TxtPageTitle.Visibility = Visibility.Visible;
                    ViewClubs.Visibility = Visibility.Visible;
                    LoadClubsData();
                    break;
            }
        }

        /// <summary>
        /// Helper method to collapse all grid views.
        /// </summary>
        private void HideAllViews()
        {
            ViewWelcome.Visibility = Visibility.Collapsed;
            ViewOverview.Visibility = Visibility.Collapsed;
            ViewAthletes.Visibility = Visibility.Collapsed;
            ViewCompetitions.Visibility = Visibility.Collapsed;
            ViewNewResult.Visibility = Visibility.Collapsed;
            ViewClubs.Visibility = Visibility.Collapsed;
        }


        private void LoadAthletesData()
        {
            try
            {
                var athletes = athleteRepository.GetAll();
                if(athletes != null)
                {
                    var displayList = athletes.Select(a =>
                    {
                        Club foundClub = clubRepositary.GetById(a.ClubID);
                        string ClubName = "Unknown Club";
                        if (foundClub != null)
                        {
                            ClubName = foundClub.Name;
                        }
                        

                        return new
                        {
                            a.FirstName,
                            a.LastName,
                            BirthDate = a.BirthDate,
                            a.Gender,
                            ClubName,
                            a.AthleteID
                        };
                    }).ToList();

                    DgAthletes.ItemsSource = displayList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void LoadComboboxData()
        {
            LoadAthletsToCombobox();
            LoadCompetitionsToCombobox();
            LoadDisciplinesToCombobox();
        }

        private void LoadAthletsToCombobox()
        {
            CmbResAthlete.Items.Clear();

            var athletes = athleteRepository.GetAll();
            foreach(var athlete in athletes)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = $"{athlete.FirstName} {athlete.LastName} ({athlete.BirthDateString()})";
                comboBoxItem.Tag = athlete.AthleteID;

                CmbResAthlete.Items.Add(comboBoxItem);
            }
        }

        private void LoadCompetitionsToCombobox()
        {
            CmbResCompetition.Items.Clear();

            var competitions = competitionRepositary.GetAll();
            foreach(var competition in competitions)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = $"{competition.Name}, {competition.Date.ToString("dd.MM.yyyy")}";
                comboBoxItem.Tag = competition.CompetitionId;

                CmbResCompetition.Items.Add(comboBoxItem);
            }
        }

        private void LoadDisciplinesToCombobox()
        {
            CmbResDiscipline.Items.Clear();

            var disciplines = disciplineRepositary.GetAll();
            foreach (var discipline in disciplines)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = discipline.Name;
                comboBoxItem.Tag = discipline.DisciplineID;

                CmbResDiscipline.Items.Add(comboBoxItem);
            }
        }

        private void LoadCompetitionsData()
        {
            allCompetitions = competitionRepositary.GetAll();

            nearestCompetition = allCompetitions.Where(c => c.Date >= DateTime.Today).OrderBy(c => c.Date).FirstOrDefault();

            if (nearestCompetition != null)
            {
                TxtNextRaceName.Text = nearestCompetition.Name;
                TxtNextRaceDate.Text = $"{nearestCompetition.Date.ToShortDateString()} in {nearestCompetition.Venue}";
                BtnOpenNextRace.IsEnabled = true;
            }
            else
            {
                TxtNextRaceName.Text = "No upcoming races";
                TxtNextRaceDate.Text = "";
                BtnOpenNextRace.IsEnabled = false;
            }

            RefreshCompetitionGrid();
        }

        private void RefreshCompetitionGrid()
        {
            string filter = TxtCompSearch.Text?.ToLower() ?? "";

            var filteredList = allCompetitions.Where(c => c.Name.ToLower().Contains(filter) || c.Venue.ToLower().Contains(filter)).ToList();

            DgCompetitions.ItemsSource = filteredList;
        }

        private void LoadClubsData()
        {
            try
            {
                var allClubsDisplayList = GetClubListWithRegions();

                var topClubData = allClubsDisplayList.OrderByDescending(x => x.AthleteCount).FirstOrDefault();

                if (topClubData != null)
                {
                    topClub = topClubData.OriginalClubObject;

                    TxtTopClubName.Text = topClubData.Name;
                    TxtTopClubCount.Text = topClubData.AthleteCount.ToString();
                    BtnOpenTopClub.IsEnabled = true;
                }
                else
                {
                    TxtTopClubName.Text = "No clubs found";
                    TxtTopClubCount.Text = "0";
                    BtnOpenTopClub.IsEnabled = false;
                }

                RefreshClubsGrid(allClubsDisplayList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clubs: " + ex.Message);
            }
        }
        private void RefreshClubsGrid(List<dynamic> listToDisplay)
        {
            DgClubs.ItemsSource = listToDisplay;
        }

        private List<dynamic> GetClubListWithRegions()
        {
            var regionRepo = new RegionRepositary();

            var clubs = clubRepositary.GetAll();
            var athletes = athleteRepository.GetAll();
            var regions = regionRepo.GetAll();

            var displayList = clubs.Select(c => new
            {
                c.ClubID,
                c.Name,
                c.RegionID,

                AthleteCount = athletes.Count(a => a.ClubID == c.ClubID),

                RegionName = regions.FirstOrDefault(r => r.RegionID == c.RegionID)?.Name ?? "Unknown",

                OriginalClubObject = c
            }).ToList();

            return displayList.Cast<dynamic>().ToList();
        }

        private void LoadOverviewData()
        {
            try
            {
                // 1. ZÍSKÁNÍ POČTŮ (KARTY)
                var allAthletes = athleteRepository.GetAll();
                var allClubs = clubRepositary.GetAll();
                var allCompetitions = competitionRepositary.GetAll();
                var allDisciplines = disciplineRepositary.GetAll();

                TxtDashAthletesCount.Text = allAthletes.Count.ToString();
                TxtDashClubsCount.Text = allClubs.Count.ToString();
                TxtDashRacesCount.Text = allCompetitions.Count.ToString();

                // 2. NEJBLIŽŠÍ ZÁVOD
                var nextRace = allCompetitions
                    .Where(c => c.Date >= DateTime.Today)
                    .OrderBy(c => c.Date)
                    .FirstOrDefault();

                if (nextRace != null)
                {
                    TxtDashNextRaceName.Text = nextRace.Name;
                    TxtDashNextRaceDate.Text = $"{nextRace.Date:dd.MM.yyyy} ({nextRace.Venue})";
                }
                else
                {
                    TxtDashNextRaceName.Text = "No upcoming races";
                    TxtDashNextRaceDate.Text = "";
                }

                var allResults = resultRepositary.GetAll();

                var recentResults = allResults
                    .OrderByDescending(r => r.ResultID) // Předpokládáme, že vyšší ID = novější
                    .Take(10) // Vezmeme posledních 10
                    .Select(r => new RecentResultView
                    {
                        Performance = r.Performance,
                        // Najdeme jméno atleta
                        AthleteName = allAthletes.FirstOrDefault(a => a.AthleteID == r.AthleteID)?.FirstName + " " +
                                      allAthletes.FirstOrDefault(a => a.AthleteID == r.AthleteID)?.LastName ?? "Unknown",

                        // Zde zatím dáváme jen ID, pokud nemáte DisciplineRepository
                        DisciplineName = allDisciplines.FirstOrDefault(d => d.DisciplineID == r.DisciplineID)?.Name ?? "Unknown Disc.",

                        // Datum ze závodu
                        DateString = allCompetitions.FirstOrDefault(c => c.CompetitionId == r.CompetitionID)?.Date.ToShortDateString() ?? "-"
                    })
                    .ToList();

                DgRecentResults.ItemsSource = recentResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message);
            }
        }

        private void SaveResult(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtResPerformance.Text))
                {
                    if (!string.IsNullOrWhiteSpace(TxtNote.Text))
                    {
                        MessageBox.Show("Please enter performance value.");
                        return;
                    }
                }

                decimal performance = decimal.Parse(TxtResPerformance.Text);

                ComboBoxItem selectedAthleteItem = (ComboBoxItem)CmbResAthlete.SelectedItem;
                int athleteID = (int)selectedAthleteItem.Tag;

                ComboBoxItem selectedCompetitionItem = (ComboBoxItem)CmbResCompetition.SelectedItem;
                int competitionID = (int)selectedCompetitionItem.Tag;

                ComboBoxItem selectedDisciplineItem = (ComboBoxItem)CmbResDiscipline.SelectedItem;
                int disciplineID = (int)selectedDisciplineItem.Tag;

                string note = TxtNote.Text;

                float wind = 0;

                if (!string.IsNullOrWhiteSpace(TxtResWind.Text))
                {
                    wind = float.Parse(TxtResWind.Text);
                }

                if (string.IsNullOrWhiteSpace(TxtPlacement.Text))
                {
                    MessageBox.Show("Please enter placement value.");
                    return;
                }

                int placement = int.Parse(TxtPlacement.Text);

                resultRepositary.CreateNewResult(new Result(athleteID, competitionID, disciplineID, performance, wind, placement, note));

                MessageBox.Show("Result saved successfully!");

                CmbResAthlete.SelectedIndex = -1;
                CmbResCompetition.SelectedIndex = -1;
                CmbResDiscipline.SelectedIndex = -1;

                TxtResPerformance.Clear();
                TxtResWind.Clear();
                TxtPlacement.Clear();
                TxtNote.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            LoadAthletesData();
        }

        private void OpenAddAthleteWindow(object sender, RoutedEventArgs e)
        {
            AddAthleteWindow addAthleteWindow = new AddAthleteWindow();
            bool? result = addAthleteWindow.ShowDialog();
            if (result == true)
            {
                LoadAthletesData();
            }

        }
        private void OpenUpdateAthleteWindow(object sender, RoutedEventArgs e)
        {
            UpdateAthleteWindow updateAthleteWindow = new UpdateAthleteWindow();
            bool? result = updateAthleteWindow.ShowDialog();
            if (result == true)
            {
                LoadAthletesData();
            }
        }

        private void OpenAddCompetitionWindow(object sender, RoutedEventArgs e)
        {
            AddCompetitionWindow addCompetitionWindow = new AddCompetitionWindow();
            bool? result = addCompetitionWindow.ShowDialog();
            if (result == true)
            {
                LoadCompetitionsData();
            }
        }

        private void OpenDeleteAthleteWindow(object sender, RoutedEventArgs e)
        {
            DeleteAthleteWindow deleteAthleteWindow = new DeleteAthleteWindow();
            bool? result = deleteAthleteWindow.ShowDialog();
            if (result == true)
            {
                LoadAthletesData();
            }
        }

        private void CompetitionSearch(object sender, TextChangedEventArgs e)
        {
            RefreshCompetitionGrid();
        }

        private void OpenCompetition(object sender, RoutedEventArgs e)
        {
            Competition selectedComp = ((FrameworkElement)sender).DataContext as Competition;

            if (selectedComp != null)
            {
                OpenDetailWindow(selectedComp);
            }
        }
        private void OpenAddClubWindow(object sender, RoutedEventArgs e)
        {
            
            AddClubWindow addClubWindow = new AddClubWindow();
            bool? result = addClubWindow.ShowDialog();
            if (result == true)
            {
                LoadClubsData();
            }
            
        }

            private void OpenNextRace(object sender, RoutedEventArgs e)
        {
            if (nearestCompetition != null)
            {
                OpenDetailWindow(nearestCompetition);
            }
        }

        private void OpenDetailWindow(Competition competition)
        {
            CompetitionDetailWindow detailWin = new CompetitionDetailWindow(competition);
            detailWin.ShowDialog();   
        }

        private void OpenTopClub(object sender, RoutedEventArgs e)
        {
            if (topClub != null)
            {
                // Předpokládám, že už máte vytvořené okno ClubDetailWindow
                ClubDetailWindow win = new ClubDetailWindow(topClub);
                win.ShowDialog();
            }
            else
            {
                MessageBox.Show("No top club loaded.");
            }
        }

        private void TxtClubSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Pokud seznam ještě není načtený, nic neděláme
            var allClubsDisplayList = GetClubListWithRegions();

            if (allClubsDisplayList == null) return;

            string filter = TxtClubSearch.Text?.ToLower() ?? "";

            var filteredList = allClubsDisplayList.Where(x => x.Name.ToLower().Contains(filter) || x.RegionName.ToLower().Contains(filter)).ToList();

            RefreshClubsGrid(filteredList);
        }

        private void OpenClubDetail(object sender, RoutedEventArgs e)
        {
            dynamic selectedRow = ((FrameworkElement)sender).DataContext;

            if (selectedRow != null)
            {
                Club c = selectedRow.OriginalClubObject;
                OpenClubDetailWindow(c);
            }
        }

        private void OpenClubDetailWindow(Club club)
        {
            ClubDetailWindow win = new ClubDetailWindow(club);
            win.ShowDialog();
            LoadClubsData();
        }

        private void BtnImportAthletes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                MessageBox.Show($"File selected: {openFileDialog.FileName}\n(Import logic needs to be implemented)");
                // Zde by byl kód pro čtení souboru
                LoadOverviewData(); // Refresh
            }
        }

        // Tlačítko pro Import Závodů
        private void BtnImportCompetitions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Import Competitions coming soon!");
        }

    }

}

using System.Windows;
using System.Windows.Controls;
using AthleticsManager.Models;
using AthleticsManager.Repositories;

namespace AthleticsManager
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AthleteRepository athleteRepository;
        private CompetitionRepositary competitionRepositary;
        private DisciplineRepositary disciplineRepositary;
        private ResultRepositary resultRepositary;

        public MainWindow()
        {
            InitializeComponent();
            athleteRepository = new AthleteRepository();
            competitionRepositary = new CompetitionRepositary();
            disciplineRepositary = new DisciplineRepositary();
            resultRepositary = new ResultRepositary();
        }

        /// <summary>
        /// Handles the navigation logic for the sidebar menu.
        /// </summary>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            string viewTag = clickedButton.Tag.ToString();

            HideAllViews();

            switch (viewTag)
            {
                case "Dashboard":
                    TxtPageTitle.Text = "Dashboard Overview";
                    ViewDashboard.Visibility = Visibility.Visible;
                    break;

                case "Athletes":
                    TxtPageTitle.Text = "Athletes Management";
                    ViewAthletes.Visibility = Visibility.Visible;
                    LoadAthletesData();
                    break;

                case "Competitions":
                    TxtPageTitle.Text = "Competitions";
                    ViewCompetitions.Visibility = Visibility.Visible;
                    break;

                case "Results":
                    TxtPageTitle.Text = "Add New Result";
                    ViewResults.Visibility = Visibility.Visible;
                    LoadComboboxData();
                    break;

                case "Clubs":
                    TxtPageTitle.Text = "Clubs Registry";
                    ViewClubs.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Helper method to collapse all grid views.
        /// </summary>
        private void HideAllViews()
        {
            ViewWelcome.Visibility = Visibility.Collapsed;
            ViewDashboard.Visibility = Visibility.Collapsed;
            ViewAthletes.Visibility = Visibility.Collapsed;
            ViewCompetitions.Visibility = Visibility.Collapsed;
            ViewResults.Visibility = Visibility.Collapsed;
            ViewClubs.Visibility = Visibility.Collapsed;
        }


        private void LoadAthletesData()
        {
            try
            {
                var athletes = athleteRepository.GetAll();
                if(athletes != null)
                {
                    DgAthletes.ItemsSource = athletes;
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
            addAthleteWindow.ShowDialog();
        }
        private void OpenUpdateAthleteWindow(object sender, RoutedEventArgs e)
        {
            UpdateAthleteWindow updateAthleteWindow = new UpdateAthleteWindow();
            updateAthleteWindow.ShowDialog();
        }

        private void OpenAddCompetitionWindow(object sender, RoutedEventArgs e)
        {
            AddCompetitionWindow addCompetitionWindow = new AddCompetitionWindow();
            addCompetitionWindow.ShowDialog();
        }

    }

}

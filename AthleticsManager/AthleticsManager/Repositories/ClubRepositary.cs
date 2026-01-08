using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace AthleticsManager.Repositories
{
    /// <summary>
    /// Handles database operations for athletic clubs.
    /// This repository provides methods to retrieve, create, and fetch statistical data for clubs.
    /// </summary>
    public class ClubRepositary
    {
        /// <summary>
        /// Retrieves all club records from the database.
        /// </summary>
        /// <returns>A list of all clubs stored in the system.</returns>
        public List<Club> GetAll()
        {
            try
            {
                var clubs = new List<Club>();
                string query = "SELECT ClubID, Name, RegionID FROM Club";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clubs.Add(new Club
                            (
                                (int)reader["ClubID"],
                                reader["Name"].ToString(),
                                (int)reader["RegionID"]
                            ));
                        }
                    }
                }
                return clubs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ClubRepositary.GetAll:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Creates a new club record in the database.
        /// Checks for duplicates based on the club name and region ID before insertion.
        /// </summary>
        /// <param name="newClub">The club object containing the data to be saved.</param>
        /// <returns>The persisted club object with its assigned ID, or the existing object if a duplicate was found.</returns>
        public Club CreateNewClub(Club newClub)
        {
            try
            {
                var allClubs = GetAll();
                if (allClubs != null)
                {
                    foreach (Club club in allClubs)
                    {
                        if (club.Name == newClub.Name && club.RegionID == newClub.RegionID)
                        {
                            return club;
                        }
                    }
                }

                string insertQuery = "INSERT INTO Club (Name, RegionID) VALUES (@Name, @RegionID)";
                using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@Name", newClub.Name);
                    command.Parameters.AddWithValue("@RegionID", newClub.RegionID);
                    command.ExecuteNonQuery();
                }

                string idQuery = "SELECT ClubID FROM Club WHERE Name = @Name AND RegionID = @RegionID";
                using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@Name", newClub.Name);
                    command.Parameters.AddWithValue("@RegionID", newClub.RegionID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newClub.SetClubID((int)reader["ClubID"]);
                        }
                    }
                }
                return newClub;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ClubRepositary.CreateNewClub:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Retrieves a specific club by its unique identifier.
        /// </summary>
        /// <param name="clubID">The unique ID of the club to retrieve.</param>
        /// <returns>The club object if found; otherwise, null.</returns>
        public Club GetById(int clubID)
        {
            try
            {
                Club club = null;
                string query = "SELECT ClubID, Name, RegionID FROM Club WHERE ClubID = @ClubID";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@ClubID", clubID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            club = new Club
                            (
                                (int)reader["ClubID"],
                                reader["Name"].ToString(),
                                (int)reader["RegionID"]
                            );
                        }
                    }
                }
                return club;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ClubRepositary.GetById:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Retrieves statistical data for clubs from the 'ClubStatistics' view.
        /// The results are ordered by gold medals (descending) and then athlete count (descending).
        /// </summary>
        /// <returns>A list of ClubStats objects containing aggregated data.</returns>
        public List<ClubStats> GetClubStatistics()
        {
            try
            {
                var list = new List<ClubStats>();
                string query = "SELECT * FROM ClubStatistics ORDER BY GoldMedals DESC, AthleteCount DESC";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        command.Connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ClubStats
                            {
                                ClubName = reader["ClubName"].ToString(),
                                RegionName = reader["RegionName"].ToString(),
                                AthleteCount = (int)reader["AthleteCount"],
                                TotalEntries = (int)reader["TotalEntries"],
                                GoldMedals = (int)reader["GoldMedals"],
                                OldestAthleteBorn = reader["OldestAthleteBorn"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["OldestAthleteBorn"],
                                YoungestAthleteBorn = reader["YoungestAthleteBorn"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["YoungestAthleteBorn"]
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ClubRepositary.GetClubStatistics:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

    }
}

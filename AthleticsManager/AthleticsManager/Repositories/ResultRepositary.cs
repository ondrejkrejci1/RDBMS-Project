using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace AthleticsManager.Repositories
{
    /// <summary>
    /// Handles database operations for athletic results.
    /// This repository provides methods to retrieve all results, create new entries, import complex result data via stored procedures, 
    /// and fetch top performance statistics.
    /// </summary>
    public class ResultRepositary
    {
        /// <summary>
        /// Retrieves all result records from the database.
        /// Maps the database rows to a list of Result objects, handling nullable fields appropriately.
        /// </summary>
        /// <returns>A list of all results stored in the system.</returns>
        public List<Result> GetAll()
        {
            try
            {
                var results = new List<Result>();
                string query = "SELECT ResultID, AthleteID, CompetitionID, DisciplineID, Performance, Wind, Placement, Note FROM Result";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new Result
                            (
                                (int)reader["ResultID"],
                                (int)reader["AthleteID"],
                                (int)reader["CompetitionID"],
                                (int)reader["DisciplineID"],
                                (decimal)reader["Performance"],
                                reader["Wind"] == DBNull.Value ? null : (double?)reader["Wind"],
                                reader["Placement"] == DBNull.Value ? null : (int?)reader["Placement"],
                                reader["Note"] == DBNull.Value ? null : reader["Note"].ToString()
                            ));
                        }
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ResultRepositary.GetAll:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Creates a new result record in the database.
        /// Checks for duplicates before insertion to ensure data integrity.
        /// If the result is successfully inserted, it retrieves and assigns the new ResultID to the object.
        /// </summary>
        /// <param name="newResult">The result object containing the data to be saved.</param>
        /// <returns>The persisted result object with its assigned ID, or the existing object if a duplicate was found.</returns>
        public Result CreateNewResult(Result newResult)
        {
            try
            {
                var allResults = GetAll();
                if (allResults != null)
                {
                    foreach (Result result in allResults)
                    {
                        if (result.AthleteID == newResult.AthleteID && result.CompetitionID == newResult.CompetitionID && result.DisciplineID == newResult.DisciplineID && result.Performance == newResult.Performance && result.Wind == newResult.Wind && result.Placement == newResult.Placement && result.Note == newResult.Note)
                        {
                            return result;
                        }
                    }
                }

                string insertQuery = "INSERT INTO Result (AthleteID, CompetitionID, DisciplineID, Performance, Wind, Placement, Note) VALUES (@AthleteID, @CompetitionID, @DisciplineID, @Performance, @Wind, @Placement, @Note)";

                using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@AthleteID", newResult.AthleteID);
                    command.Parameters.AddWithValue("@CompetitionID", newResult.CompetitionID);
                    command.Parameters.AddWithValue("@DisciplineID", newResult.DisciplineID);
                    command.Parameters.AddWithValue("@Performance", newResult.Performance);
                    command.Parameters.AddWithValue("@Wind", newResult.Wind.HasValue ? (object)newResult.Wind.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Placement", newResult.Placement.HasValue ? (object)newResult.Placement.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Note", string.IsNullOrEmpty(newResult.Note) ? DBNull.Value : (object)newResult.Note);

                    command.ExecuteNonQuery();
                }

                string idQuery = "SELECT ResultID FROM Result WHERE AthleteID = @AthleteID AND CompetitionID = @CompetitionID AND DisciplineID = @DisciplineID AND Performance = @Performance";
                // Simplified WHERE clause for retrieval to avoid complex null checks in SQL query from code

                using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@AthleteID", newResult.AthleteID);
                    command.Parameters.AddWithValue("@CompetitionID", newResult.CompetitionID);
                    command.Parameters.AddWithValue("@DisciplineID", newResult.DisciplineID);
                    command.Parameters.AddWithValue("@Performance", newResult.Performance);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newResult.SetResultID((int)reader["ResultID"]);
                        }
                    }
                }
                return newResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ResultRepositary.CreateNewResult:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Imports a complex result record using a stored procedure.
        /// This method handles the creation or retrieval of related entities (Athlete, Club, Region, Competition) 
        /// automatically within the database logic.
        /// </summary>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="birthDate">The athlete's date of birth.</param>
        /// <param name="gender">The athlete's gender.</param>
        /// <param name="clubName">The name of the athlete's club.</param>
        /// <param name="regionName">The region of the club.</param>
        /// <param name="compName">The name of the competition.</param>
        /// <param name="compDate">The date of the competition.</param>
        /// <param name="compVenue">The venue of the competition.</param>
        /// <param name="compType">The type of the competition.</param>
        /// <param name="disciplineName">The name of the discipline.</param>
        /// <param name="performance">The performance value achieved.</param>
        /// <param name="wind">The wind reading (nullable).</param>
        /// <param name="placement">The placement achieved (nullable).</param>
        /// <param name="note">Any additional notes (nullable).</param>
        public void ImportResult(string firstName, string lastName, DateTime birthDate, string gender, string clubName, string regionName, string compName, DateTime compDate, string compVenue, string compType, string disciplineName, decimal performance, double? wind, int? placement, string note)
        {
            try
            {
                string query = "AddRaceResutl"; // Assuming Typo in SP name matches previous code
                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@BirthDate", birthDate);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@ClubName", clubName);
                    command.Parameters.AddWithValue("@RegionName", regionName);
                    command.Parameters.AddWithValue("@CompetitionName", compName);
                    command.Parameters.AddWithValue("@CompetitionDate", compDate);
                    command.Parameters.AddWithValue("@CompetitionVenue", compVenue);
                    command.Parameters.AddWithValue("@CompetitionType", compType);
                    command.Parameters.AddWithValue("@DisciplineName", disciplineName);
                    command.Parameters.AddWithValue("@Performance", performance);
                    command.Parameters.AddWithValue("@Wind", wind.HasValue ? (object)wind.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Placement", placement.HasValue ? (object)placement.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Note", string.IsNullOrEmpty(note) ? DBNull.Value : (object)note);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ResultRepositary.ImportResult:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Retrieves a list of top athletic performances from the 'TopPerformances' view.
        /// The results are ordered by performance value in descending order.
        /// </summary>
        /// <returns>A list of TopPerformance objects representing the best results.</returns>
        public List<TopPerformance> GetTopPerformances()
        {
            try
            {
                var list = new List<TopPerformance>();
                string query = "SELECT * FROM TopPerformances ORDER BY Performance DESC";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TopPerformance
                            {
                                DisciplineID = (int)reader["DisciplineID"],
                                Discipline = reader["Discipline"].ToString(),
                                Performance = (decimal)reader["Performance"],
                                AthleteName = reader["AthleteName"].ToString(),
                                CompetitionName = reader["CompetitionName"].ToString(),
                                Date = (DateTime)reader["CompetitionDate"]
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in ResultRepositary.GetTopPerformances:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

    }
}

using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace AthleticsManager.Repositories
{
    /// <summary>
    /// Handles database operations for athletic competitions.
    /// This repository provides methods to retrieve, create, and import competition records.
    /// </summary>
    public class CompetitionRepositary
    {
        /// <summary>
        /// Retrieves all competition records from the database.
        /// </summary>
        /// <returns>A list of all competitions stored in the system.</returns>
        public List<Competition> GetAll()
        {
            try
            {
                var competitions = new List<Competition>();
                string query = "SELECT CompetitionID, Name, Date, Venue, Type FROM Competition";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            competitions.Add(new Competition
                            (
                                (int)reader["CompetitionID"],
                                reader["Name"].ToString(),
                                (DateTime)reader["Date"],
                                reader["Venue"].ToString(),
                                reader["Type"].ToString()
                            ));
                        }
                    }
                }
                return competitions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in CompetitionRepositary.GetAll:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Creates a new competition record in the database.
        /// Checks for duplicates based on name, date, venue, and type before insertion.
        /// </summary>
        /// <param name="newCompetition">The competition object containing the data to be saved.</param>
        /// <returns>The persisted competition object with its assigned ID, or the existing object if a duplicate was found.</returns>
        public Competition CreateNewCompetition(Competition newCompetition)
        {
            try
            {
                var allCompetitions = GetAll();
                if (allCompetitions != null)
                {
                    foreach (Competition competition in allCompetitions)
                    {
                        if (competition.Name == newCompetition.Name && competition.Date == newCompetition.Date && competition.Venue == newCompetition.Venue && competition.Type == newCompetition.Type)
                        {
                            return competition;
                        }
                    }
                }

                string insertQuery = "INSERT INTO Competition (Name, Date, Venue, Type) VALUES (@Name, @Date, @Venue, @Type)";
                using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@Name", newCompetition.Name);
                    command.Parameters.AddWithValue("@Date", newCompetition.Date);
                    command.Parameters.AddWithValue("@Venue", newCompetition.Venue);
                    command.Parameters.AddWithValue("@Type", newCompetition.Type);
                    command.ExecuteNonQuery();
                }

                string idQuery = "SELECT CompetitionID FROM Competition WHERE Name = @Name AND Date = @Date AND Venue = @Venue AND Type = @Type";
                using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@Name", newCompetition.Name);
                    command.Parameters.AddWithValue("@Date", newCompetition.Date);
                    command.Parameters.AddWithValue("@Venue", newCompetition.Venue);
                    command.Parameters.AddWithValue("@Type", newCompetition.Type);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newCompetition.SetCompetitionId((int)reader["CompetitionID"]);
                        }
                    }
                }
                return newCompetition;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in CompetitionRepositary.CreateNewCompetition:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }

        }

        /// <summary>
        /// Imports a competition record using a stored procedure.
        /// This is typically used for bulk data imports or external integrations.
        /// </summary>
        /// <param name="name">The name of the competition.</param>
        /// <param name="date">The date of the event.</param>
        /// <param name="venue">The location where the competition takes place.</param>
        public void ImportCompetition(string name, DateTime date, string venue)
        {
            try
            {
                string query = "ImportCompetition";
                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Venue", venue);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in CompetitionRepositary.ImportCompetition:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

    }
}

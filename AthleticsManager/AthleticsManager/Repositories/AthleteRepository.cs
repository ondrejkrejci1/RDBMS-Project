using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace AthleticsManager.Repositories
{
    /// <summary>
    /// Handles database operations for athletes.
    /// This repository provides methods to manage athlete records, including retrieval, creation, updating, deleting, and importing.
    /// </summary>
    public class AthleteRepository
    {
        /// <summary>
        /// Retrieves all athlete records from the database.
        /// </summary>
        public List<Athlete> GetAll()
        {
            try
            {
                var athletes = new List<Athlete>();
                string query = "SELECT AthleteID, FirstName, LastName, BirthDate, Gender, IsActive, ClubID FROM Athlete";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            athletes.Add(new Athlete
                            (
                                (int)reader["AthleteID"],
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                (DateTime)reader["BirthDate"],
                                reader["Gender"].ToString(),
                                (bool)reader["IsActive"],
                                (int)reader["ClubID"]
                            ));
                        }
                    }
                }
                return athletes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.GetAll:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        // <summary>
        /// Creates a new athlete record in the database.
        /// Checks for duplicates based on personal details (name, birthdate, gender, club) before insertion.
        /// </summary>
        /// <param name="newAthlete">The athlete object containing the data to be saved.</param>
        /// <returns>The persisted athlete object with its assigned ID, or the existing object if a duplicate was found.</returns>
        public Athlete CreateNewAthlete(Athlete newAthlete)
        {
            try
            {
                var allAthletes = GetAll();
                if (allAthletes != null)
                {
                    foreach (Athlete athlete in allAthletes)
                    {
                        // Check logic updated to include IsActive check if necessary, or keep identity check strict
                        if (athlete.FirstName == newAthlete.FirstName &&
                            athlete.LastName == newAthlete.LastName &&
                            athlete.BirthDate == newAthlete.BirthDate &&
                            athlete.Gender == newAthlete.Gender &&
                            athlete.ClubID == newAthlete.ClubID)
                        {
                            return athlete;
                        }
                    }
                }

                // Added IsActive to INSERT
                string insertQuery = "INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, IsActive, ClubID) VALUES (@FirstName, @LastName, @BirthDate, @Gender, @IsActive, @ClubID)";

                using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@FirstName", newAthlete.FirstName);
                    command.Parameters.AddWithValue("@LastName", newAthlete.LastName);
                    command.Parameters.AddWithValue("@BirthDate", newAthlete.BirthDate);
                    command.Parameters.AddWithValue("@Gender", newAthlete.Gender);
                    command.Parameters.AddWithValue("@IsActive", newAthlete.IsActive); // Pass boolean
                    command.Parameters.AddWithValue("@ClubID", newAthlete.ClubID);

                    command.ExecuteNonQuery();
                }

                // Retrieve ID
                string idQuery = "SELECT AthleteID FROM Athlete WHERE FirstName = @FirstName AND Lastname = @LastName AND BirthDate = @BirthDate AND ClubID = @ClubID";

                using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@FirstName", newAthlete.FirstName);
                    command.Parameters.AddWithValue("@LastName", newAthlete.LastName);
                    command.Parameters.AddWithValue("@BirthDate", newAthlete.BirthDate);
                    command.Parameters.AddWithValue("@ClubID", newAthlete.ClubID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newAthlete.SetAthleteID((int)reader["AthleteID"]);
                        }
                    }
                }

                return newAthlete;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.CreateNewAthlete:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Updates an existing athlete's details in the database.
        /// Dynamically builds the SQL update statement based on the provided non-null parameters.
        /// </summary>
        /// <param name="firstName">The new first name (optional).</param>
        /// <param name="lastName">The new last name (optional).</param>
        /// <param name="birthDate">The new birth date (optional).</param>
        /// <param name="gender">The new gender (optional).</param>
        /// <param name="isActive">The new active status (optional).</param>
        /// <param name="athleteID">The ID of the athlete to update.</param>
        public void Update(string firstName, string lastName, DateTime? birthDate, string gender, bool? isActive, int athleteID)
        {
            try
            {
                string query = "UPDATE ATHLETE SET";
                bool firstParam = true;

                // Helper local function to handle commas
                void AddComma() { if (!firstParam) query += ","; else firstParam = false; }

                if (firstName != null) { AddComma(); query += " FirstName = @FirstName"; }
                if (lastName != null) { AddComma(); query += " LastName = @LastName"; }
                if (birthDate != null) { AddComma(); query += " BirthDate = @BirthDate"; }
                if (gender != null) { AddComma(); query += " Gender = @Gender"; }
                if (isActive != null) { AddComma(); query += " IsActive = @IsActive"; } // Handle bool update

                query += " WHERE AthleteID = @AthleteID";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    if (firstName != null) command.Parameters.AddWithValue("@FirstName", firstName);
                    if (lastName != null) command.Parameters.AddWithValue("@LastName", lastName);
                    if (birthDate != null) command.Parameters.AddWithValue("@BirthDate", birthDate);
                    if (gender != null) command.Parameters.AddWithValue("@Gender", gender);
                    if (isActive != null) command.Parameters.AddWithValue("@IsActive", isActive); // Pass value

                    command.Parameters.AddWithValue("@AthleteID", athleteID);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.Update:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Deletes an athlete and their associated results from the database.
        /// Ensures referential integrity by first removing related records in the Result table.
        /// </summary>
        /// <param name="athleteID">The ID of the athlete to delete.</param>
        public void Delete(int athleteID)
        {
            try
            {
                string resultQuery = "DELETE FROM Result WHERE AthleteID = @AthleteID";
                using (var command = new SqlCommand(resultQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@AthleteID", athleteID);
                    command.ExecuteNonQuery();
                }

                string athleteQuery = "DELETE FROM Athlete WHERE AthleteID = @AthleteID";
                using (var command = new SqlCommand(athleteQuery, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@AthleteID", athleteID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.Delete:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Retrieves a list of athletes belonging to a specific club.
        /// </summary>
        /// <param name="clubID">The ID of the club to filter by.</param>
        /// <returns>A list of athletes associated with the given club.</returns>
        public List<Athlete> GetAthletesByClub(int clubID)
        {
            try
            {
                var athletes = new List<Athlete>();
                string query = "SELECT AthleteID, FirstName, LastName, BirthDate, Gender, IsActive, ClubID FROM Athlete WHERE ClubID = @ClubID";

                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    command.Parameters.AddWithValue("@ClubID", clubID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            athletes.Add(new Athlete(
                                (int)reader["AthleteID"],
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                (DateTime)reader["BirthDate"],
                                reader["Gender"].ToString(),
                                (bool)reader["IsActive"],
                                (int)reader["ClubID"]
                            ));
                        }
                    }
                }
                return athletes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.GetAthletesByClub:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return null;
            }
        }

        /// <summary>
        /// Imports an athlete record using a stored procedure.
        /// This method allows for creating an athlete along with their club and region if they do not already exist.
        /// </summary>
        /// <param name="firstName">The first name of the athlete.</param>
        /// <param name="lastName">The last name of the athlete.</param>
        /// <param name="birthDate">The birth date of the athlete.</param>
        /// <param name="gender">The gender of the athlete.</param>
        /// <param name="isActive">Indicates whether the athlete is currently active.</param>
        /// <param name="clubName">The name of the athlete's club.</param>
        /// <param name="clubRegion">The region name associated with the club.</param>
        public void ImportAthlete(string firstName, string lastName, DateTime birthDate, string gender, bool isActive, string clubName, string clubRegion)
        {
            try
            {
                string query = "ImportAthlete";
                using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@BirthDate", birthDate);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@IsActive", isActive); // Pass param
                    command.Parameters.AddWithValue("@ClubName", clubName);
                    command.Parameters.AddWithValue("@ClubRegionName", clubRegion);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Database Error in AthleteRepository.ImportAthlete:\n{ex.Message}\n\nThe application will now close.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

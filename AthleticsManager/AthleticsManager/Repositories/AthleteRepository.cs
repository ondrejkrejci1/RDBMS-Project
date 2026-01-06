using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AthleticsManager.Repositories
{
    public class AthleteRepository
    {
        public List<Athlete> GetAll()
        {
            var athletes = new List<Athlete>();

            string query = "SELECT AthleteID, FirstName, LastName, BirthDate, Gender, ClubID FROM Athlete";

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
                            (int)reader["ClubID"]
                        ));
                    }
                }
            }
            return athletes;
        }

        public Athlete Get(int id)
        {
            Athlete athlete = null;

            string query = "SELECT AthleteID, FirstName, LastName, BirthDate, Gender, ClubID FROM Athlete WHERE AthleteID = @AthleteID";

            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {

                command.Parameters.AddWithValue("@AthleteID", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Read())
                        {
                            athlete = new Athlete((int)reader["AthleteID"],
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            (DateTime)reader["BirthDate"],
                            reader["Gender"].ToString(),
                            (int)reader["ClubID"]);
                        }
                    }
                }
            }

            return athlete;
        }

        public void AddResultWithTransaction(string firstName, string lastName, DateTime birthDate, string gender, int clubId, int competitionId, int disciplineId, decimal performance, float wind = -999)
        {
            using (var command = new SqlCommand("sp_AddRaceResult", DatabaseSingleton.GetInstance()))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@BirthDate", birthDate);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@ClubID", clubId);
                command.Parameters.AddWithValue("@CompetitionID", competitionId);
                command.Parameters.AddWithValue("@DisciplineID", disciplineId);
                command.Parameters.AddWithValue("@Performance", performance);

                if (wind != -999)
                    command.Parameters.AddWithValue("@Wind", wind);
                else
                    command.Parameters.AddWithValue("@Wind", DBNull.Value);

                command.Parameters.AddWithValue("@Placement", DBNull.Value);
                command.Parameters.AddWithValue("@Note", DBNull.Value);

                command.ExecuteNonQuery();
            }

        }

        public Athlete CreateNewAthlete(Athlete newAthlete)
        {
            var allAthletes = GetAll();
            foreach (Athlete athlete in allAthletes)
            {
                if (athlete.FirstName == newAthlete.FirstName && athlete.LastName == newAthlete.LastName && athlete.BirthDate == newAthlete.BirthDate && athlete.Gender == newAthlete.Gender && athlete.ClubID == newAthlete.ClubID)
                {
                    return athlete;
                }
            }

            string insertQuery = "INSERT INTO Athlete (FirstName, LastName, BirthDate, Gender, ClubID) VALUES (@FirstName, @LastName, @BirthDate, @Gender, @ClubID)";

            using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
            {
                command.Parameters.AddWithValue("@FirstName", newAthlete.FirstName);
                command.Parameters.AddWithValue("@LastName", newAthlete.LastName);
                command.Parameters.AddWithValue("@BirthDate", newAthlete.BirthDate);
                command.Parameters.AddWithValue("@Gender", newAthlete.Gender);
                command.Parameters.AddWithValue("@ClubID", newAthlete.ClubID);

                command.ExecuteNonQuery();

            }

            string idQuery = "SELECT AthleteID FROM Athlete WHERE FirstName = '@FirstName' AND Lastname = @LastName AND BirthDate = @BirthDate AND Gender = @Gender AND ClubID = @ClubID";

            using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
            {
                command.Parameters.AddWithValue("@FirstName", newAthlete.FirstName);
                command.Parameters.AddWithValue("@LastName", newAthlete.LastName);
                command.Parameters.AddWithValue("@BirthDate", newAthlete.BirthDate);
                command.Parameters.AddWithValue("@Gender", newAthlete.Gender);
                command.Parameters.AddWithValue("@ClubID", newAthlete.ClubID);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        newAthlete.SetAthleteID((int)reader["ClubID"]);
                    }
                }
            }

            return newAthlete;
        }

        public void Update(string firstName, string lastName, DateTime? birthDate, string gender, int athleteID)
        {
            string query = "UPDATE ATHLETE SET";
            
            if (firstName != null)
            {
                query += " FirstName = @FirstName";
            }
            if (lastName != null)
            {
                if (firstName != null)
                {
                    query += ",";
                }
                query += " LastName = @LastName";
            }
            if (birthDate != null)
            {
                if (firstName != null || lastName != null)
                {
                    query += ",";
                }
                query += " BirthDate = @BirthDate";
            }
            if (gender != null)
            {
                if (firstName != null || lastName != null || birthDate != null)
                {
                    query += ",";
                }
                query += " Gender = @Gender";
            }
            query += " WHERE AthleteID = " + athleteID;
            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {
                if(firstName != null)
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                }
                if (lastName != null)
                {
                    command.Parameters.AddWithValue("@LastName", lastName);
                }
                if (birthDate != null)
                {
                    command.Parameters.AddWithValue("@BirthDate", birthDate);
                }
                if (gender != null)
                {
                    command.Parameters.AddWithValue("@Gender", gender);
                }

                command.ExecuteNonQuery();
            }


        }

        public void Delete(int athleteID)
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

    }
}

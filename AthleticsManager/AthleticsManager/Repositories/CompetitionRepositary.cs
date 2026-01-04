using AthleticsManager.Models;
using Microsoft.Data.SqlClient;

namespace AthleticsManager.Repositories
{
    public class CompetitionRepositary
    {
        public List<Competition> GetAll()
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

        public Competition Get(int id)
        {
            Competition competition = null;

            string query = "SELECT CompetitionID, Name, Date, Venue, Type FROM Competition WHERE CompetitionID = @CompetitionID";

            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {

                command.Parameters.AddWithValue("@CompetitionID", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Read())
                        {
                            competition = new Competition(
                            (int)reader["CompetitionID"],
                            reader["Name"].ToString(), 
                            (DateTime)reader["Date"],
                            reader["Venue"].ToString(),
                            reader["Type"].ToString());
                        }
                    }
                }
            }

            return competition;
        }

        public Competition CreateNewCompetition(Competition newCompetition)
        {
            var allCompetitions = GetAll();
            foreach (Competition competition in allCompetitions)
            {
                if (competition.Name == newCompetition.Name && competition.Date == newCompetition.Date && competition.Venue == newCompetition.Venue && competition.Type == newCompetition.Type)
                {
                    return competition;
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

            string idQuery = "SELECT CompetitionID FROM Competition WHERE Name = '@Name' AND Date = @Date AND Venue = @Venue AND Type = @Type";

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

    }
}

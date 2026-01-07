using AthleticsManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AthleticsManager.Repositories
{
    public class ResultRepositary
    {
        public List<Result> GetAll()
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

        public Result CreateNewResult(Result newResult)
        {
            var allResults = GetAll();
            foreach (Result result in allResults)
            {
                if (result.AthleteID == newResult.AthleteID && result.CompetitionID == newResult.CompetitionID && result.DisciplineID == newResult.CompetitionID && result.Performance == newResult.Performance && result.Wind == newResult.Wind && result.Placement == newResult.Placement && result.Note == newResult.Note)
                {
                    return result;
                }
            }

            string insertQuery = "INSERT INTO Result (AthleteID, CompetitionID, DisciplineID, Performance, Wind, Placement, Note) VALUES (@AthleteID, @CompetitionID, @DisciplineID, @Performance, @Wind, @Placement, @Note)";

            using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
            {
                command.Parameters.AddWithValue("@AthleteID", newResult.AthleteID);
                command.Parameters.AddWithValue("@CompetitionID", newResult.CompetitionID);
                command.Parameters.AddWithValue("@DisciplineID", newResult.DisciplineID);
                command.Parameters.AddWithValue("@Performance", newResult.Performance);
                command.Parameters.AddWithValue("@Wind", newResult.Wind);
                command.Parameters.AddWithValue("@Placement", newResult.Placement);
                command.Parameters.AddWithValue("@Note", newResult.Note);

                command.ExecuteNonQuery();

            }

            string idQuery = "SELECT ResultID FROM Result WHERE AthleteID = @AthleteID AND CompetitionID = @CompetitionID AND DisciplineID = @DisciplineID AND Performance = @Performance AND Wind = @Wind AND Placement = @Placement AND Note = @Note";

            using (var command = new SqlCommand(idQuery, DatabaseSingleton.GetInstance()))
            {
                command.Parameters.AddWithValue("@AthleteID", newResult.AthleteID);
                command.Parameters.AddWithValue("@CompetitionID", newResult.CompetitionID);
                command.Parameters.AddWithValue("@DisciplineID", newResult.DisciplineID);
                command.Parameters.AddWithValue("@Performance", newResult.Performance);
                command.Parameters.AddWithValue("@Wind", newResult.Wind);
                command.Parameters.AddWithValue("@Placement", newResult.Placement);
                command.Parameters.AddWithValue("@Note", newResult.Note);

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

        public void ImportResult(string firstName, string lastName, DateTime birthDate, string gender, string clubName, string regionName, string compName, DateTime compDate, string compVenue, string compType, string disciplineName, decimal performance, double? wind, int? placement, string note)
        {
            string query = "AddRaceResutl";

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

    }
}

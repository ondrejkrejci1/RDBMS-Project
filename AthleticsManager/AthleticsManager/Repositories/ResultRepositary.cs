using AthleticsManager.Models;
using Microsoft.Data.SqlClient;

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
                            (double)reader["Wind"],
                            (int)reader["Placement"],
                            reader["Note"].ToString()
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

    }
}

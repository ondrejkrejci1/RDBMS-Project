using AthleticsManager.Models;
using Microsoft.Data.SqlClient;

namespace AthleticsManager.Repositories
{
    public class DisciplineRepositary
    {
        public List<Discipline> GetAll()
        {
            var disciplines = new List<Discipline>();

            string query = "SELECT DisciplineID, Name, UnitType FROM Discipline";

            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        disciplines.Add(new Discipline
                        (
                            (int)reader["DisciplineID"],
                            reader["Name"].ToString(),
                            reader["UnitType"].ToString()
                        ));
                    }
                }
            }
            return disciplines;
        }

        public Discipline Get(int id)
        {
            Discipline discipline = null;

            string query = "SELECT DisciplineID, Name, UnitType FROM Discipline WHERE DisciplineID = @DisciplineID";

            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {

                command.Parameters.AddWithValue("@DisciplineID", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Read())
                        {
                            discipline = new Discipline(
                            (int)reader["DisciplineID"],
                            reader["Name"].ToString(),
                            reader["UnitType"].ToString());
                        }
                    }
                }
            }
            return discipline;
        }

    }
}

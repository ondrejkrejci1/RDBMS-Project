using AthleticsManager.Models;
using Microsoft.Data.SqlClient;

namespace AthleticsManager.Repositories
{
    public class RegionRepositary
    {
        public List<Region> GetAll()
        {
            var regions = new List<Region>();

            string query = "SELECT RegionID, Name FROM Region";

            using (var command = new SqlCommand(query, DatabaseSingleton.GetInstance()))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        regions.Add(new Region
                        (
                            (int)reader["RegionID"],
                            reader["Name"].ToString()
                        ));
                    }
                }
            }
            return regions;
        }

    }
}

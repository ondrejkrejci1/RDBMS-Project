using AthleticsManager.Models;
using Microsoft.Data.SqlClient;

namespace AthleticsManager.Repositories
{
    public class ClubRepositary
    {
        public List<Club> GetAll()
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

        public Club CreateNewClub(Club newClub)
        {
            var allClubs = GetAll();
            foreach (Club club in allClubs)
            {
                if (club.Name == newClub.Name && club.RegionID == newClub.RegionID)
                {
                    return club;
                }
            }

            string insertQuery = "INSERT INTO Club (Name, RegionID) VALUES (@Name, @RegionID)";

            using (var command = new SqlCommand(insertQuery, DatabaseSingleton.GetInstance()))
            {
                command.Parameters.AddWithValue("@Name", newClub.Name);
                command.Parameters.AddWithValue("@RegionID", newClub.RegionID);

                command.ExecuteNonQuery();

            }

            string idQuery = "SELECT ClubID FROM Club WHERE Name = '@Name' AND RegionID = @RegionID";

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

    }
}

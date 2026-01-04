namespace AthleticsManager.Models
{
    public class Club
    {
        public int ClubID { get; protected set; }
        public string Name { get; protected set; }
        public int RegionID { get; protected set; }

        public Club(string name, int regionID)
        {
            Name = name;
            RegionID = regionID;
        }
        public Club(int clubID, string name, int regionID)
        {
            ClubID = clubID;
            Name = name;
            RegionID = regionID;
        }

        public void SetClubID(int clubID)
        {
            ClubID = clubID;
        }


    }
}

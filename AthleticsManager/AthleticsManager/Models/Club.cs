namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents an athletic club entity.
    /// This class stores the fundamental identity and geographical location (region) of a sports club.
    /// </summary>
    public class Club
    {
        /// <summary>
        /// Gets the unique identifier for the club.
        /// </summary>
        public int ClubID { get; protected set; }

        /// <summary>
        /// Gets the official name of the club.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the identifier corresponding to the club's region (mapped to the Region enum).
        /// </summary>
        public int RegionID { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Club class.
        /// This constructor is typically used when creating a new club that has not yet been saved to the database (ID is not yet assigned).
        /// </summary>
        /// <param name="name">The name of the club.</param>
        /// <param name="regionID">The region identifier.</param>
        public Club(string name, int regionID)
        {
            Name = name;
            RegionID = regionID;
        }

        /// <summary>
        /// Initializes a new instance of the Club class with an existing ID.
        /// This constructor is typically used when loading existing records from the database.
        /// </summary>
        /// <param name="clubID">The unique ID of the club.</param>
        /// <param name="name">The name of the club.</param>
        /// <param name="regionID">The region identifier.</param>
        public Club(int clubID, string name, int regionID)
        {
            ClubID = clubID;
            Name = name;
            RegionID = regionID;
        }

        /// <summary>
        /// Assigns a unique identifier to the club.
        /// This is primarily used to update the object's state after it has been successfully inserted into the database.
        /// </summary>
        /// <param name="clubID">The new club ID.</param>
        public void SetClubID(int clubID)
        {
            ClubID = clubID;
        }


    }
}

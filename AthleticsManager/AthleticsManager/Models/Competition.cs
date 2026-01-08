namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents an athletic competition event.
    /// This class holds details about a specific event including its name, date, venue, and type.
    /// </summary>
    public class Competition
    {
        /// <summary>
        /// Gets the unique identifier for the competition.
        /// </summary>
        public int CompetitionId { get; protected set; }

        /// <summary>
        /// Gets the official name of the competition (e.g., "National Championship").
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the date when the competition takes place.
        /// </summary>
        public DateTime Date { get; protected set; }

        /// <summary>
        /// Gets the location or venue where the competition is held (e.g., "City Stadium").
        /// </summary>
        public string Venue { get; protected set; }

        /// <summary>
        /// Gets the type or category of the competition (e.g., "Outdoor", "Indoor", "Regional").
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Competition class.
        /// This constructor is typically used when creating a new competition that has not yet been saved to the database (ID is not yet assigned).
        /// </summary>
        /// <param name="name">The name of the competition.</param>
        /// <param name="date">The date of the event.</param>
        /// <param name="venue">The venue of the event.</param>
        /// <param name="type">The type of the competition.</param>
        public Competition(string name, DateTime date, string venue, string type) 
        {
            Name = name;
            Date = date;
            Venue = venue;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the Competition class with an existing ID.
        /// This constructor is typically used when loading existing records from the database.
        /// </summary>
        /// <param name="competitionId">The unique ID of the competition.</param>
        /// <param name="name">The name of the competition.</param>
        /// <param name="date">The date of the event.</param>
        /// <param name="venue">The venue of the event.</param>
        /// <param name="type">The type of the competition.</param>
        public Competition(int competitionId, string name, DateTime date, string venue, string type)
        {
            CompetitionId = competitionId;
            Name = name;
            Date = date;
            Venue = venue;
            Type = type;
        }

        /// <summary>
        /// Assigns a unique identifier to the competition.
        /// This is primarily used to update the object's state after it has been successfully inserted into the database.
        /// </summary>
        /// <param name="competitionId">The new competition ID.</param>
        public void SetCompetitionId(int competitionId)
        {
            CompetitionId = competitionId;
        }

        /// <summary>
        /// Gets the date of the competition formatted as a string (dd.MM.yyyy).
        /// Convenient for displaying the date in the user interface.
        /// </summary>
        public string DateString
        {
            get
            {
                return Date.ToString("dd.MM.yyyy");
            }
        }

    }
}
